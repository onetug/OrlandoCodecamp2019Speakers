using System;
using System.Collections.Generic;
using System.Linq;

namespace frontend.Models
{
    public class TransactionItem
    {
        public Guid Id { get; set; }
        public string DateFormatted { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Summary { get; set; }
    }
}