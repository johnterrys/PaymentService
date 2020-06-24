﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayRefundEventHandler : IWeChatPayRefundEventHandler, ITransientDependency
    {
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IWeChatPayFeeConverter _weChatPayFeeConverter;
        private readonly ServiceProviderPayService _serviceProviderPayService;

        public WeChatPayRefundEventHandler(
            IClock clock,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IRefundRepository refundRepository,
            IPaymentRepository paymentRepository,
            IPaymentRecordRepository paymentRecordRepository,
            IRefundRecordRepository refundRecordRepository,
            IWeChatPayFeeConverter weChatPayFeeConverter,
            ServiceProviderPayService serviceProviderPayService)
        {
            _clock = clock;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
            _paymentRecordRepository = paymentRecordRepository;
            _refundRecordRepository = refundRecordRepository;
            _weChatPayFeeConverter = weChatPayFeeConverter;
            _serviceProviderPayService = serviceProviderPayService;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleEventAsync(WeChatPayRefundEto eventData)
        {
            using (_currentTenant.Change(eventData.TenantId))
            {
                var payment = await _paymentRepository.GetAsync(eventData.PaymentId);
                var paymentRecord = await _paymentRecordRepository.GetByPaymentId(eventData.PaymentId);
                var refundRecordId = _guidGenerator.Create();

                var dict = await RequestWeChatPayRefundAsync(payment, paymentRecord, eventData, refundRecordId.ToString());

                await CreateRefundEntitiesAsync(payment, eventData, dict);

                await RollbackIfFailedAsync(payment, eventData, dict);
            }
        }

        protected virtual async Task RollbackIfFailedAsync(Payment payment, WeChatPayRefundEto eventData, Dictionary<string, string> dict)
        {
            if (dict["result_code"] != "SUCCESS")
            {
                payment.RollbackOngoingRefund();
                
                await _paymentRepository.UpdateAsync(payment, true);
            }
        }

        protected virtual async Task CreateRefundEntitiesAsync(IPaymentEntity payment, WeChatPayRefundEto eventData, Dictionary<string, string> dict)
        {
            foreach (var info in eventData.RefundInfos)
            {
                var refund = new Refund(
                    id: _guidGenerator.Create(),
                    tenantId: _currentTenant.Id,
                    paymentId: payment.Id,
                    paymentItemId: info.PaymentItem.Id,
                    refundPaymentMethod: payment.PaymentMethod,
                    externalTradingCode: dict["refund_id"],
                    currency: payment.Currency,
                    refundAmount: info.RefundAmount,
                    customerRemark: info.CustomerRemark,
                    staffRemark: info.StaffRemark
                );

                if (dict["result_code"] != "SUCCESS")
                {
                    refund.CancelRefund(_clock.Now);
                }
                
                await _refundRepository.InsertAsync(refund, true);
            }
        }
        
        protected virtual async Task CreateWeChatPayRefundRecordEntitiesAsync(Payment payment, WeChatPayRefundEto eventData, Dictionary<string, string> dict)
        {
            var settlementTotalFeeString = dict["settlement_total_fee"];
            var settlementRefundFeeString = dict["settlement_refund_fee"];
            var cashRefundFeeString = dict["cash_refund_fee"];
            var couponRefundFeeString = dict["coupon_refund_fee"];
            var couponRefundCountString = dict["coupon_refund_count"];
            var couponRefundCount = couponRefundCountString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(couponRefundCountString);

            await _refundRecordRepository.InsertAsync(new RefundRecord(
                id: _guidGenerator.Create(),
                tenantId: _currentTenant.Id,
                paymentId: payment.Id,
                returnCode: dict["return_code"],
                returnMsg: dict["return_msg"],
                appId: dict["appid"],
                mchId: dict["mch_id"],
                transactionId: dict["transaction_id"],
                outTradeNo: dict["out_trade_no"],
                refundId: dict["refund_id"],
                outRefundNo: dict["out_refund_no"],
                totalFee: Convert.ToInt32(dict["total_fee"]),
                settlementTotalFee: settlementTotalFeeString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(settlementTotalFeeString),
                refundFee: Convert.ToInt32(dict["refund_fee"]),
                settlementRefundFee: settlementRefundFeeString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(settlementRefundFeeString),
                feeType: dict["fee_type"],
                cashFee: Convert.ToInt32(dict["cash_fee"]),
                cashFeeType: dict["cash_fee_type"],
                cashRefundFee: cashRefundFeeString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(cashRefundFeeString),
                couponRefundFee: couponRefundFeeString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(couponRefundFeeString),
                couponRefundCount: couponRefundCount,
                couponTypes: couponRefundCount != null ? dict.JoinNodesInnerTextAsString("coupon_type_", couponRefundCount.Value) : null,
                couponIds: couponRefundCount != null ? dict.JoinNodesInnerTextAsString("coupon_id_", couponRefundCount.Value) : null,
                couponRefundFees: couponRefundCount != null ? dict.JoinNodesInnerTextAsString("coupon_refund_fee_", couponRefundCount.Value) : null
            ), true);
        }

        private async Task<Dictionary<string, string>> RequestWeChatPayRefundAsync(Payment payment, PaymentRecord paymentRecord, WeChatPayRefundEto eventData, string outRefundNo)
        {
            var refundAmount = eventData.RefundInfos.Sum(model => model.RefundAmount);

            var result = await _serviceProviderPayService.RefundAsync(
                appId: payment.GetProperty<string>("appid"),
                mchId: payment.PayeeAccount,
                subAppId: null,
                subMchId: null,
                transactionId: paymentRecord.TransactionId,
                outTradeNo: payment.Id.ToString("N"),
                outRefundNo: outRefundNo,
                totalFee: paymentRecord.TotalFee,
                refundFee: _weChatPayFeeConverter.ConvertToWeChatPayFee(refundAmount),
                refundFeeType: null,
                refundDesc: eventData.DisplayReason,
                refundAccount: null,
                notifyUrl: null
            );
            
            var dict = new Dictionary<string, string>(result.SelectSingleNode("xml").ToDictionary() ?? throw new NullReferenceException());

            if (dict["return_code"] != "SUCCESS")
            {
                throw new RefundFailedException(dict["return_code"], dict["return_msg"]);
            }

            await CreateWeChatPayRefundRecordEntitiesAsync(payment, eventData, dict);

            return dict;
        }
    }
}