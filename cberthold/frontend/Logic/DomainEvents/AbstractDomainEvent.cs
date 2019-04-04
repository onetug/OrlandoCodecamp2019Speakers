using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Events;
using MediatR;

namespace frontend.Logic.DomainEvents
{
    public class AbstractDomainEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public AbstractDomainEvent()
        {
        }
    }
}