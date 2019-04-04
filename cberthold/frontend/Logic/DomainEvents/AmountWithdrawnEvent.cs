using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace frontend.Logic.DomainEvents
{
    public class AmountWithdrawnEvent : AbstractDomainEvent
    {
        public Guid WithdrawalId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public Guid AccountId { get; private set; }

        public AmountWithdrawnEvent(
            Guid withdrawalId,
            decimal amount,
            DateTime date,
            Guid accountId)
        {
            WithdrawalId = withdrawalId;
            Amount = amount;
            Date = date;
            AccountId = accountId;
        }
    }
}