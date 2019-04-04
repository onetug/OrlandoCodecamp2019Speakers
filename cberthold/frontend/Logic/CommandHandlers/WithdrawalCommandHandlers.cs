using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using frontend.Logic.Commands;
using MediatR;
using frontend.Data;
using frontend.Logic.DomainEvents;
using CQRSlite.Domain;

namespace frontend.Logic.CommandHandlers
{
    public class WithdrawalCommandHandler : IRequestHandler<WithdrawalCommand, TransactionResponse>
    {
        private readonly ISession session;

        public WithdrawalCommandHandler(ISession session)
        {
            this.session = session;
        }

        public async Task<TransactionResponse> Handle(WithdrawalCommand request, CancellationToken cancellationToken)
        {
            var account = await session.GetOrCreateAccount(request.AccountId, cancellationToken);
            
            var withdrawal = new Withdrawal
            {
                Date = DateTime.Today,
                Amount = request.Amount,
                AccountId = request.AccountId,
            };

            account.Withdraw(withdrawal);

            await session.Commit(cancellationToken);

            var response = new TransactionResponse
            {
                Type = "Withdrawal",
                Amount = request.Amount,
            };

            return response;
        }
    }
}