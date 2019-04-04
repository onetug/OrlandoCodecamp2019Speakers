using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using frontend.Models;
using frontend.Logic.Commands;
using frontend.Logic.Queries;
using MediatR;

namespace frontend.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{accountId:guid}/Transactions")]
        public async Task<TransactionModel> GetTransactions(Guid accountId, CancellationToken token)
        {
            var query = new GetTransactionQuery
            {
                AccountId = accountId,
            };

            var result = await mediator.Send(query, token);
            return result;
        }

        [HttpPost("{accountId:guid}/[action]")]
        public async Task<IActionResult> Deposit(Guid accountId, [FromBody] DepositModel model, CancellationToken token)
        {
            var command = new DepositCommand {
                AccountId = accountId,
                Amount = model.Amount,
            };

            var response = await mediator.Send(command, token);
            return Ok(response);
        }

        [HttpPost("{accountId:guid}/[action]")]
        public async Task<IActionResult> Withdrawal(Guid accountId, [FromBody] WithdrawalModel model, CancellationToken token)
        {
            var command = new WithdrawalCommand {
                AccountId = accountId,
                Amount = model.Amount,
            };

            var response = await mediator.Send(command, token);
            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ReplayAllEvents(CancellationToken token)
        {
            var command = new ReplayCommand();
            await mediator.Send(command, token);
            return Ok();
        }
    }
}
