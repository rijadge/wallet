using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wallet.Models;
using wallet.Services;

namespace wallet.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class WalletController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public WalletController(IAccountService accountService)
        { 
            _accountService = accountService;
        }

        /// <summary>
        /// Method to add extra money to <see cref="Player"/> balance.
        /// </summary>
        /// <param name="id">Id of player.</param>
        /// <param name="amount">Amount to add.</param>
        /// <returns>The updated <see cref="Player"/> instance.</returns>
        [HttpPut("pay/{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Pay(Guid id, [FromBody] decimal amount)
        {
            var isPlayerFetched = await _accountService.TryFetchPlayer(id);
            
            if (!isPlayerFetched)
                return NotFound("Player could not be found!");
            
            var areCreditsAdded = await _accountService.AddCredits(amount);
            
            return areCreditsAdded ? Ok(_accountService.GetPlayer()) : 
                Problem("Credits could not be saved!", nameof(Player), (int)HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Method to check the <see cref="Player"/> balance.
        /// </summary>
        /// <param name="id">Id of player.</param>
        /// <returns>Returns the balance.</returns>
        [HttpGet("players/{id:guid}/balance")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<ActionResult<decimal>> CurrentBalance(Guid id)
        {
            var isPlayerFetched = await _accountService.TryFetchPlayer(id);
            
            if (!isPlayerFetched)
                return NotFound();

            return Ok(_accountService.GetPlayer().Balance);
        }

        /// <summary>
        /// Method to make a bet.
        /// </summary>
        /// <param name="id">Id of player.</param>
        /// <param name="amount">Amount to bet.</param>
        /// <returns>Returns the last <see cref="Transaction"/>.</returns>
        [HttpPost("players/{id:guid}/bet")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Bet(Guid id, [FromBody]decimal amount)
        {
            var isPlayerFetched = await _accountService.TryFetchPlayer(id);
            
            if (!isPlayerFetched)
                return NotFound();
            
            if (!_accountService.GetPlayer().HasEnoughBalance(amount))
                return Problem("Not enough balance!", nameof(Player));
            if (_accountService.GetPlayer().WillReachLossLimit(amount))
                return Problem("Loss limit reached!", nameof(Player));

            var isBetMade = await _accountService.MakeBet(amount);
            
            return isBetMade ? Ok(_accountService.GetPlayer().Transactions.Last()) : 
                Problem("Value could not be saved!", nameof(Player), (int)HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Method to give a reward.
        /// </summary>
        /// <param name="id">Id of player.</param>
        /// <param name="amount">Amount to reward.</param>
        /// <returns>Returns the last <see cref="Transaction"/>.</returns>
        [HttpPost("players/{id:guid}/win")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Reward(Guid id, [FromBody]decimal amount)
        {
            var isPlayerFetched = await _accountService.TryFetchPlayer(id);

            if (!isPlayerFetched)
                return NotFound();
            
            var isRewardGiven = await _accountService.GiveReward(amount);

            return isRewardGiven ? Ok(_accountService.GetPlayer().Transactions.Last()) : 
                Problem("Value could not be saved!", nameof(Player), (int)HttpStatusCode.InternalServerError);
        }
        
        /// <summary>
        /// Method to set loss limit of player.
        /// </summary>
        /// <param name="id">Id of player.</param>
        /// <param name="limit">Limit to set.</param>
        /// <returns><see cref="Player"/> with updated loss limit.</returns>
        [HttpPut("players/{id:guid}/loss-limit")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SetLossLimit(Guid id, decimal limit)
        {
            var isPlayerFetched = await _accountService.TryFetchPlayer(id);

            if (!isPlayerFetched)
                return NotFound();
            
            var isLossLimitSet = await _accountService.SetLossLimit(limit);

            return isLossLimitSet ? Ok(_accountService.GetPlayer()) : 
                Problem("Value could not be saved!", nameof(Player), (int)HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Method to read all players. Read id of any player and use it in the other endpoints.
        /// </summary>
        /// <returns>List of <see cref="Player"/>.</returns>
        [HttpGet("players")]
        public async Task<List<Player>> Players()
        {
            return await _accountService.GetPlayers();
        }
        
        /// <summary>
        /// Method to see all transactions of all players.
        /// </summary>
        /// <returns>List of <see cref="Transaction"/>.</returns>
        [HttpGet("transactions")]
        public async Task<List<Transaction>> Transactions()
        {
            return await _accountService.GetTransactions();
        }
        
        
    }
}