using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using CQRSlite;
using Newtonsoft.Json;
using CQRSlite.Events;
using Microsoft.Extensions.Configuration;

namespace frontend.Infrastructure
{
    public class SqlEventStore : IEventStore, IReplayEventStore, IDisposable
    {
        private const string INSERT_SQL =
@"
INSERT INTO {0} ([CommitId],[AggregateId],[Timestamp],[Version],[EventType],[EventData]) VALUES (@CommitId,@AggregateId,@Timestamp,@Version,@EventType,@EventData)
";

        private const string SELECT_SQL =
@"SELECT CommitId, AggregateId, TimeStamp, Version, EventType, EventData
  FROM {0}
  WHERE AggregateId = @AggregateId AND [Version] > @FromVersion
  ORDER BY [Version]
";

        private const string GETALL_SELECT_SQL =
 @"SELECT CommitId, AggregateId, TimeStamp, Version, EventType, EventData
  FROM {0}
  ORDER BY [Offset]
";

        private readonly string eventStoreTable;
        private readonly string connectionString;
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.Objects,
        };

        private readonly JsonSerializer serializer;
        private readonly IEventPublisher publisher;

        public SqlEventStore(IConfiguration configuration, IEventPublisher publisher)
        {
            eventStoreTable = "EventStore";
            connectionString = configuration.GetConnectionString("BankAccountContext");
            serializer = JsonSerializer.Create(settings);
            this.publisher = publisher;
        }

        public async Task ReplayAllEvents(CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(string.Format(GETALL_SELECT_SQL, eventStoreTable), cancellationToken: cancellationToken);
            using (var connection = await _GetConnection(cancellationToken))
            {
                var eventsQuery = await connection.QueryAsync<DocumentData>(command);

                // create a function to deserialize event data
                Func<string, string, IEvent> func = (eventType, data) =>
                {
                    var output = DeserializeData<IEvent>(data, Type.GetType(eventType));
                    return output;
                };

                // deserialize the events from our documents
                var events = from e in eventsQuery
                             select func(e.EventType, e.EventData);

                foreach (var @event in events)
                {
                    await publisher.PublishAsync(@event, cancellationToken);
                }
            }
        }

        private async Task<SqlConnection> _GetConnection(CancellationToken cancellationToken)
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new CommandDefinition(string.Format(SELECT_SQL, eventStoreTable), parameters: new { AggregateId = aggregateId, FromVersion = fromVersion }, cancellationToken: cancellationToken);
            using (var connection = await _GetConnection(cancellationToken))
            {
                var eventsQuery = await connection.QueryAsync<DocumentData>(command);

                // create a function to deserialize event data
                Func<string, string, IEvent> func = (eventType, data) =>
                {
                    var output = DeserializeData<IEvent>(data, Type.GetType(eventType));
                    return output;
                };

                // deserialize the events from our documents
                var events = from e in eventsQuery
                             select func(e.EventType, e.EventData);

                // return the data as events
                return events.Cast<IEvent>();
            }
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            var commitId = Guid.NewGuid();

            // batch events so we don't overrun the 2100 parms issue
            var batchedDocuments = (from e in events
                                    select new DocumentData()
                                    {
                                        CommitId = commitId,
                                        AggregateId = e.Id,
                                        Version = e.Version,
                                        TimeStamp = e.TimeStamp,
                                        EventType = e.GetType().AssemblyQualifiedName,
                                        EventData = SerializeData(e),
                                    }).Batch(400);

            using (var connection = await _GetConnection(cancellationToken))
            {
                 // start up a transaction
                using (var tx = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    // run through each batch
                    foreach (var batch in batchedDocuments)
                    {
                        var documentList = batch.ToArray();

                        // create command definition to run on Dapper
                        var command = new CommandDefinition(string.Format(INSERT_SQL, eventStoreTable), parameters: documentList, transaction: tx, cancellationToken: cancellationToken, commandType: CommandType.Text);

                        // execute the sql
                        await connection.ExecuteAsync(command);
                    }

                    tx.Commit();
                }
            }

            foreach(var @event in events)
            {
                await publisher.PublishAsync(@event, cancellationToken);
            }
        }

        private string SerializeData<TObject>(TObject obj)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        private TObject DeserializeData<TObject>(string data, Type type)
        {
            return (TObject)JsonConvert.DeserializeObject(data, type, settings);
        }

        public class DocumentData
        {
            public Guid CommitId { get; set; }

            public Guid AggregateId { get; set; }

            public DateTimeOffset TimeStamp { get; set; }

            public int Version { get; set; }

            public string EventType { get; set; }

            public string EventData { get; set; }

        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SqlEventStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}