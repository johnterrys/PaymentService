using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Infrastructure;
using EasyAbp.Abp.WeChat.Pay.Infrastructure.OptionResolve;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    [ExposeServices(typeof(IWeChatPayHandler), IncludeSelf = true)]
    public class PaymentServiceWeChatPayRefundHandler : IWeChatPayHandler, ITransientDependency
    {
        public WeChatHandlerType Type { get; } = WeChatHandlerType.Refund;

        private readonly IDataFilter _dataFilter;
        private readonly IPaymentManager _paymentManager;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentServiceWeChatPayRefundHandler(
            IDataFilter dataFilter,
            IPaymentManager paymentManager,
            IRefundRecordRepository refundRecordRepository,
            IRefundRepository refundRepository,
            IPaymentRepository paymentRepository)
        {
            _dataFilter = dataFilter;
            _paymentManager = paymentManager;
            _refundRecordRepository = refundRecordRepository;
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleAsync(WeChatPayHandlerContext context)
        {
            var dict = context.WeChatRequestXmlData.SelectSingleNode("xml").ToDictionary() ?? throw new NullReferenceException();

            if (dict.GetOrDefault("return_code") != "SUCCESS")
            {
                return;
            }
            
            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var record = await _refundRecordRepository.FindAsync(x => x.Id == Guid.Parse(dict.GetOrDefault("out_refund_no")));

            if (record == null)
            {
                return;
            }
            
            var payment = await _paymentRepository.FindAsync(record.PaymentId);
            
            var refund = await _refundRepository.FindByPaymentIdAsync(record.PaymentId);

            if (payment == null || refund == null)
            {
                context.IsSuccess = false;

                return;
            }
            
            record.SetInformationInNotify(
                refundStatus: dict.GetOrDefault("refund_status"),
                successTime: dict.GetOrDefault("success_time"),
                refundRecvAccout: dict.GetOrDefault("refund_recv_accout"),
                refundAccount: dict.GetOrDefault("refund_account"),
                refundRequestSource: dict.GetOrDefault("refund_request_source"),
                settlementRefundFee: Convert.ToInt32(dict.GetOrDefault("settlement_refund_fee")));

            await _refundRecordRepository.UpdateAsync(record, true);

            if (dict.GetOrDefault("refund_status") == "SUCCESS")
            {
                await _paymentManager.CompleteRefundAsync(payment, refund);
            }
            else
            {
                await _paymentManager.RollbackRefundAsync(payment, refund);
            }

            context.IsSuccess = true;
        }
    }
}