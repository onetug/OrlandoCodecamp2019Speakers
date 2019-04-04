using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace frontend.Logic.DomainEvents
{
    public class AccountOpenedEvent : AbstractDomainEvent
    {
        public Guid AccountId { get; private set; }

        public AccountOpenedEvent(
            Guid accountId)
        {
            AccountId = accountId;
        }
    }
}