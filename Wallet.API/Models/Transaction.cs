using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace wallet.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Decimal Amount { get; set; }
    }
}