using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using frontend.Logic.DomainEvents;
using System.Threading;
using System.Threading.Tasks;
using frontend.Data;

namespace frontend.Logic.DomainEventHandlers
{
    public class TransactionViewDomainEventHandler 
        : INotificationHandler<AmountDepositedEvent>,
        INotificationHandler<AmountWithdrawnEvent>
    {

        private readonly IBankAccountsContext context;

        public TransactionViewDomainEventHandler(IBankAccountsContext context)
        {
            this.context = context;
        }

        public async Task Handle(AmountDepositedEvent notification, CancellationToken cancellationToken)
        {
            var transaction = new TransactionsView
            {
                TransactionId = notification.DepositId,
                Amount = notification.Amount,
                Date = notification.Date,
                AccountId = notification.AccountId,
                Type = "Deposit",
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(AmountWithdrawnEvent notification, CancellationToken cancellationToken)
        {
            var transaction = new TransactionsView
            {
                TransactionId = notification.WithdrawalId,
                Amount = notification.Amount * -1,
                Date = notification.Date,
                AccountId = notification.AccountId,
                Type = "Withdrawal"
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}