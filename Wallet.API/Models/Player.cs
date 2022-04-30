using System;
using System.Collections.Generic;

namespace wallet.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal LossLimit { get; set; }
        public decimal CurrentLoss { get; set; }
        public List<Transaction> Transactions { get; set; }

        public bool WillReachLossLimit(decimal amount)
        {
            return CurrentLoss + amount >= LossLimit;
        }

        public bool HasEnoughBalance(decimal amount)
        {
            return Balance >= amount;
        }
        
    }
}