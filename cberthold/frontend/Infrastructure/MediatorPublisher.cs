using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using MediatR;

namespace frontend.Infrastructure
{
    public class MediatorPublisher : IEventPublisher
    {
        private readonly IMediator mediator;
        public MediatorPublisher(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        public async Task PublishAsync(IEvent @event, CancellationToken token)
        {
            var notification = @event as INotification;

            if(notification == null)
            {
                return;
            }

            await mediator.Publish(notification, token);
        }
    }
}