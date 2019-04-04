using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using MediatR;

namespace frontend.Logic.Commands
{
    public class WithdrawalCommand : IRequest<TransactionResponse>
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}