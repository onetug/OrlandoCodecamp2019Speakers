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
    public class DepositCommandHandler : IRequestHandler<DepositCommand, TransactionResponse>
    {
        private readonly ISession session;

        public DepositCommandHandler(ISession session)
        {
            this.session = session;
        }

        public async Task<TransactionResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var account = await session.GetOrCreateAccount(request.AccountId, cancellationToken);
            var deposit = new Deposit
            {
                Date = DateTime.Today,
                Amount = request.Amount,
                AccountId = request.AccountId,
            };
            account.Deposit(deposit);
            await session.Commit(cancellationToken);
            

            var response = new TransactionResponse
            {
                Type = "Deposit",
                Amount = request.Amount,
            };

            return response;
        }
    }
}