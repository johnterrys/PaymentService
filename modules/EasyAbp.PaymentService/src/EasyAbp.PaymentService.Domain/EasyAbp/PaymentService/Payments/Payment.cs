using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    public class Payment : FullAuditedAggregateRoot<Guid>, IPaymentEntity
    {
        public virtual Guid? TenantId { get; protected set; }
        
        public virtual Guid UserId { get; protected set; }
        
        [NotNull]
        public virtual string PaymentMethod { get; protected set; }
        
        [CanBeNull]
        public virtual string PayeeAccount { get; protected set; }
        
        [CanBeNull]
        public virtual string ExternalTradingCode { get; protected set; }
        
        [NotNull]
        public virtual string Currency { get; protected set; }
        
        public virtual decimal OriginalPaymentAmount { get; protected set; }

        public virtual decimal PaymentDiscount { get; protected set; }
        
        public virtual decimal ActualPaymentAmount { get; protected set; }
        
        public virtual decimal RefundAmount { get; protected set; }
        
        public virtual DateTime? CompletionTime { get; protected set; }
        
        public virtual DateTime? CancelledTime { get; protected set; }
        
        public virtual List<PaymentItem> PaymentItems { get; protected set; }

        protected Payment()
        {
            PaymentItems = new List<PaymentItem>();
        }

        public Payment(
            Guid id,
            Guid? tenantId,
            Guid userId,
            [NotNull] string paymentMethod,
            [NotNull] string currency,
            decimal originalPaymentAmount,
            List<PaymentItem> paymentItems
        ) :base(id)
        {
            TenantId = tenantId;
            UserId = userId;
            PaymentMethod = paymentMethod;
            Currency = currency;
            OriginalPaymentAmount = originalPaymentAmount;
            ActualPaymentAmount = originalPaymentAmount;
            PaymentItems = paymentItems;
            RefundAmount = 0;
        }

        public void SetPayeeAccount([NotNull] string payeeAccount)
        {
            PayeeAccount = payeeAccount;
        }

        public void SetExternalTradingCode([NotNull] string externalTradingCode)
        {
            CheckPaymentIsNotFinished();

            ExternalTradingCode = externalTradingCode;
        }

        public void SetPaymentDiscount(decimal paymentDiscount)
        {
            CheckPaymentIsNotFinished();

            PaymentDiscount = paymentDiscount;
            ActualPaymentAmount -= paymentDiscount;
        }

        public void CompletePayment(DateTime completionTime)
        {
            CheckPaymentIsNotFinished();

            CompletionTime = completionTime;
        }
        
        public void CancelPayment(DateTime cancelledTime)
        {
            CheckPaymentIsNotFinished();

            CancelledTime = cancelledTime;
        }

        private void CheckPaymentIsNotFinished()
        {
            if (CompletionTime.HasValue || CancelledTime.HasValue)
            {
                throw new PaymentHasAlreadyBeenCompletedException(Id);
            }
        }
    }
}
