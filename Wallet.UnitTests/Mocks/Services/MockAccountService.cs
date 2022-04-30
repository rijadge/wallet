using System;
using System.Collections.Generic;
using Moq;
using wallet.Models;
using wallet.Services;

namespace Wallet.UnitTests.Mocks.Services
{
    public class MockAccountService : Mock<IAccountService>
    {

        public MockAccountService MockAddCredits(bool areCreditsAdded)
        {
            Setup(x => x.AddCredits(It.IsAny<decimal>()))
                .ReturnsAsync(areCreditsAdded);

            return this;
        }

        public MockAccountService MockGiveReward(bool isRewardGiven)
        {
            Setup(x => x.GiveReward(It.IsAny<decimal>()))
                .ReturnsAsync(isRewardGiven);

            return this;
        }

        public MockAccountService MockMakeBet(bool isBetMade)
        {
            Setup(x => x.MakeBet(It.IsAny<decimal>()))
                .ReturnsAsync(isBetMade);

            return this;
        }

        public MockAccountService MockTryFetchPlayer(bool isPlayerFetched)
        {
            Setup(x => x.TryFetchPlayer(It.IsAny<Guid>()))
                .ReturnsAsync(isPlayerFetched);

            return this;
        }
        
        public MockAccountService MockGetPlayer(Player player)
        {
            Setup(x => x.GetPlayer())
                .Returns(player);
            
            return this;
        }

        public MockAccountService MockGetPlayers(List<Player> players)
        {
            Setup(x => x.GetPlayers())
                .ReturnsAsync(players);

            return this;
        }
        
    }
}