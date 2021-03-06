using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    [RemoteService(Name = PaymentServiceRemoteServiceConsts.RemoteServiceName)]
    [Route("/api/payment-service/prepayment/account")]
    public class AccountController : PrepaymentController, IAccountAppService
    {
        private readonly IAccountAppService _service;

        public AccountController(IAccountAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("{id}/change/balance")]
        public virtual Task<AccountDto> ChangeBalanceAsync(Guid id, ChangeBalanceInput input)
        {
            return _service.ChangeBalanceAsync(id, input);
        }

        [HttpPost]
        [Route("{id}/change/locked-balance")]
        public virtual Task<AccountDto> ChangeLockedBalanceAsync(Guid id, ChangeLockedBalanceInput input)
        {
            return _service.ChangeLockedBalanceAsync(id, input);
        }

        [HttpPost]
        [Route("{id}/top-up")]
        public virtual Task TopUpAsync(Guid id, TopUpInput input)
        {
            return _service.TopUpAsync(id, input);
        }

        [HttpPost]
        [Route("{id}/withdraw")]
        public virtual Task WithdrawAsync(Guid id, WithdrawInput input)
        {
            return _service.WithdrawAsync(id, input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<AccountDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<AccountDto>> GetListAsync(GetAccountListInput input)
        {
            return _service.GetListAsync(input);
        }
    }
}
