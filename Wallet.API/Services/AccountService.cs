using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wallet.Models;

namespace wallet.Services
{
    public class AccountService : IAccountService
    {
        private readonly WalletContext _dbContext;
        private Player Player { get; set; }

        public AccountService(WalletContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> MakeBet(decimal amount)
        {
            Player.Balance -= amount;
            Player.CurrentLoss += amount;

            Transaction transaction = new()
            {
                Amount = -amount
            };
            
            Player.Transactions.Add(transaction);
            
            _dbContext.Update(Player);
            var updatedRows = await _dbContext.SaveChangesAsync();
            
            return updatedRows > 0;
        }
        public async Task<bool> GiveReward(decimal amount)
        {
            Player.Balance += amount;
            Player.CurrentLoss -= amount;
            
            Transaction transaction = new()
            {
                Amount = amount
            };
            
            Player.Transactions.Add(transaction);

            _dbContext.Update(Player);
            var updatedRows = await _dbContext.SaveChangesAsync();
            
            return updatedRows > 0;
        }

        public async Task<bool> AddCredits(decimal amount)
        {
            Player.Balance += amount;

            _dbContext.Update(Player);
            var updatedRows = await _dbContext.SaveChangesAsync();
            
            return updatedRows > 0;
        }

        public async Task<bool> SetLossLimit(decimal lossLimit)
        {
            Player.LossLimit = lossLimit;
            
            _dbContext.Update(Player);
            var updatedRows = await _dbContext.SaveChangesAsync();
            
            return updatedRows > 0;
        }

        public async Task<bool> TryFetchPlayer(Guid id)
        {
            Player = await _dbContext.Players
                .Include(pl => pl.Transactions)
                .FirstOrDefaultAsync(pl => pl.Id == id);

            return Player != null;
        }
        public Player GetPlayer()
        {
            return Player;
        }

        public async Task<List<Player>> GetPlayers()
        {
            return await _dbContext.Players
                .Include(player => player.Transactions)
                .ToListAsync();
        }
        public async Task<List<Transaction>> GetTransactions()
        {
            return await _dbContext.Transactions.ToListAsync();
        }
        
    }
}