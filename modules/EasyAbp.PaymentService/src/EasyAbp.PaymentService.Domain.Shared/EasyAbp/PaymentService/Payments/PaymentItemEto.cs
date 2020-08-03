﻿using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentItemEto : IPaymentItem, IHasExtraProperties
    {
        public Guid Id { get; set; }
        
        public string ItemType { get; set; }

        public Guid ItemKey { get; set; }

        public string Currency { get; set; }

        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }

        public decimal ActualPaymentAmount { get; set; }

        public decimal RefundAmount { get; set; }
        
        public decimal PendingRefundAmount { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}