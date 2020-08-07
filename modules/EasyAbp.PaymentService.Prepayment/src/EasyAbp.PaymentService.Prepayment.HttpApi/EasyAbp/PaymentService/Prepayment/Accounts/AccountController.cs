using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    [RemoteService(Name = "EasyAbpPaymentServicePrepayment")]
    [Route("/api/paymentService/prepayment/account")]
    public class AccountController : PrepaymentController, IAccountAppService
    {
        private readonly IAccountAppService _service;

        public AccountController(IAccountAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("{id}/change/balance")]
        public Task<AccountDto> ChangeBalanceAsync(Guid id, ChangeBalanceInput input)
        {
            return _service.ChangeBalanceAsync(id, input);
        }

        [HttpPost]
        [Route("{id}/change/lockedBalance")]
        public Task<AccountDto> ChangeLockedBalanceAsync(Guid id, ChangeLockedBalanceInput input)
        {
            return _service.ChangeLockedBalanceAsync(id, input);
        }

        [HttpPost]
        [Route("{id}/recharge")]
        public Task RechargeAsync(Guid id, RechargeInput input)
        {
            return _service.RechargeAsync(id, input);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<AccountDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<AccountDto>> GetListAsync(GetAccountListInput input)
        {
            return _service.GetListAsync(input);
        }
    }
}