using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Infrastructure;
using frontend.Logic.Commands;
using MediatR;

namespace frontend.Logic.CommandHandlers
{
    public class ReplayCommandHandler : IRequestHandler<ReplayCommand>
    {
        private readonly IReplayEventStore eventStore;
        public ReplayCommandHandler(IReplayEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task<Unit> Handle(ReplayCommand request, CancellationToken cancellationToken)
        {
            await eventStore.ReplayAllEvents(cancellationToken);
            return Unit.Value;
        }
    }
}
