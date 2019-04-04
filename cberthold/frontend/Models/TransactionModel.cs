using System;
using System.Collections.Generic;
using System.Linq;

namespace frontend.Models
{
    public class TransactionModel
    {
        public decimal CurrentBalance {get; set; }
        public IEnumerable<TransactionItem> Transactions { get; set; }
    }
}