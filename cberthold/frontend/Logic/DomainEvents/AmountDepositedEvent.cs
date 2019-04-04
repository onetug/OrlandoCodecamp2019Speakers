using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace frontend.Logic.DomainEvents
{
    public class AmountDepositedEvent : AbstractDomainEvent
    {
        public Guid DepositId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public Guid AccountId { get; private set; }

        public AmountDepositedEvent(
            Guid depositId,
            decimal amount,
            DateTime date,
            Guid accountId)
        {
            DepositId = depositId;
            Amount = amount;
            Date = date;
            AccountId = accountId;
        }
    }
}