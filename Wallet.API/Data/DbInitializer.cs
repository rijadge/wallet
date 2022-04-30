using System;
using System.Collections.Generic;
using System.Linq;
using wallet.Models;

namespace wallet.Data
{
    public static class DbInitializer
    {   
        private const int NumPlayers = 3;

        public static void Initialize(WalletContext context)
        {
            context.Database.EnsureCreated();

            if (context.Players.Any())
            {
                return;
            }

            List<Player> players = new();
            
            for(var i=1; i<=NumPlayers; i++)
            {
                var player = new Player()
                {
                    Balance = 0,
                    Id = Guid.NewGuid(),
                    Name = "Player " + i,
                    LossLimit = 100 + i*10,
                    Transactions = new List<Transaction>()
                    {
                        new()
                        {
                            Amount = 120 + i,
                            Id = Guid.NewGuid()
                        },
                        new()
                        {
                            Amount = -120 - i,
                            Id = Guid.NewGuid()
                        }
                    }
                };
                
                players.Add(player);
            }
            
            context.AddRange(players);
            context.SaveChanges();
        }
        
    }
}