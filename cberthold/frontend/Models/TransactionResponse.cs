using System;
using System.Collections.Generic;
using System.Linq;

namespace frontend.Models
{
    public class TransactionResponse {
        public string Type { get; set; }
        public decimal Amount {get; set; }
    }
}