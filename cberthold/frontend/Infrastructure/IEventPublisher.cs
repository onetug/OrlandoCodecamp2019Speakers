using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;

namespace frontend.Infrastructure
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEvent @event, CancellationToken token);
    }
}