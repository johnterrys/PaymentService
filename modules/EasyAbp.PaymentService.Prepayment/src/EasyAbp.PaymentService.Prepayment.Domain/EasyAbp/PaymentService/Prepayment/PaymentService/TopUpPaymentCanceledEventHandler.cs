using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class TopUpPaymentCanceledEventHandler : IDistributedEventHandler<PaymentCanceledEto>, ITransientDependency
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentTenant _currentTenant;

        public TopUpPaymentCanceledEventHandler(
            IAccountRepository accountRepository,
            ICurrentTenant currentTenant)
        {
            _accountRepository = accountRepository;
            _currentTenant = currentTenant;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleEventAsync(PaymentCanceledEto eventData)
        {
            var payment = eventData.Payment;

            using var currentTenant = _currentTenant.Change(payment.TenantId);

            foreach (var item in payment.PaymentItems.Where(item => item.ItemType == PrepaymentConsts.TopUpPaymentItemType))
            {
                var account = await _accountRepository.GetAsync(Guid.Parse(item.ItemKey));
                
                account.SetPendingTopUpPaymentId(null);

                await _accountRepository.UpdateAsync(account, true);
            }
        }
    }
}