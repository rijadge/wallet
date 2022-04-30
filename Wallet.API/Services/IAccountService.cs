using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wallet.Models;

namespace wallet.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Method used to read all transactions.
        /// </summary>
        /// <returns>List of <see cref="Transaction"/></returns>
        Task<List<Transaction>> GetTransactions();
        
        /// <summary>
        /// Method used to give a reward to the player.
        /// </summary>
        /// <param name="amount">Amount to give.</param>
        /// <returns>True if the player was rewarded, otherwise, false.</returns>
        Task<bool> GiveReward(decimal amount);
        
        /// <summary>
        /// Method to make a bet.
        /// </summary>
        /// <param name="amount">Amount to bet.</param>
        /// <returns>True if bet was successful, otherwise, false.</returns>
        Task<bool> MakeBet(decimal amount);
        
        /// <summary>
        /// Fetches a player and saves it internally in the <see cref="AccountService"/>.
        /// </summary>
        /// <param name="id">Id of player to fetch.</param>
        /// <returns>Returns true if player successfully read, otherwise, false.</returns>
        Task<bool> TryFetchPlayer(Guid id);
        
        /// <summary>
        /// Method to add credits in player's balance.
        /// </summary>
        /// <param name="amount">Amount to add.</param>
        /// <returns>True if added, otherwise, false.</returns>
        Task<bool> AddCredits(decimal amount);
        
        /// <summary>
        /// Method to set loss limit of player.
        /// </summary>
        /// <param name="lossLimit">Loss limit to set.</param>
        /// <returns>True if set, otherwise, false.</returns>
        Task<bool> SetLossLimit(decimal lossLimit);
        
        /// <summary>
        /// Method to get a player from db.
        /// </summary>
        /// <returns>Instance of <see cref="Player"/></returns>
        Player GetPlayer();
        
        /// <summary>
        /// Method to get all player from db.
        /// </summary>
        /// <returns>List of <see cref="Player"/></returns>
        Task<List<Player>> GetPlayers();
    }
}