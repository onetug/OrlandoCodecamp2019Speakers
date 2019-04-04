using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;

namespace frontend.Infrastructure
{
    public interface IReplayEventStore
    {
        Task ReplayAllEvents(CancellationToken cancellationToken);
    }
}
