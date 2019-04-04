using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Models;
using frontend.Logic.Queries;
using MediatR;
using frontend.Data;
using Microsoft.EntityFrameworkCore;

namespace frontend.Logic.QueryHandlers
{
    public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, TransactionModel>
    {
        private readonly IBankAccountsContext context;
        public GetTransactionQueryHandler(IBankAccountsContext context)
        {
            this.context = context;
        }
        public async Task<TransactionModel> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {

            var itemsFromDb = await (from t in context.Transactions
                              where t.AccountId == request.AccountId
                              orderby 
                                t.Date descending, 
                                t.Type,
                                t.TransactionId
                              select t).ToArrayAsync(cancellationToken);


            var items = itemsFromDb.Select(item => new TransactionItem
            {
                Id = item.TransactionId,
                DateFormatted = item.Date.ToString("d"),
                Type = item.Type,
                Amount =  item.Amount,
                Summary = string.Empty,
            });

            var tx = new TransactionModel()
            {
                CurrentBalance = items.Sum(a=> a.Amount),
                Transactions = items,
            };

            return tx;
        }
    }
}