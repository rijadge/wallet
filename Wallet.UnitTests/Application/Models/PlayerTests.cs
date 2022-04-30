using wallet.Models;
using Xunit;

namespace Wallet.UnitTests.Application.Models
{
    public class PlayerTests
    {
        [Theory]
        [InlineData(200, 100, 99, 1)]
        [InlineData(200, 100, 100,1)]
        [InlineData(200, 0, 0, 1)]
        [InlineData(200, 0, 199, 1)]
        public void WillReachLossLimit_LossLimitReached_ReturnTrue(decimal balance, decimal lossLimit, decimal currentLoss, decimal amount)
        {
            //arrange
            var player = CreatePlayer(balance, lossLimit, currentLoss);

            //act
            var isLossLimitReached = player.WillReachLossLimit(amount);
            
            //assert
            Assert.True(isLossLimitReached);
        }
        [Theory]
        [InlineData(200, 100, 98, 1)] 
        [InlineData(200, 22, 20, 1)]
        [InlineData(200, 23, 22, 0)]
        public void WillReachLossLimit_LossLimitNotReached_ReturnFalse(decimal balance, decimal lossLimit, decimal currentLoss, decimal amount)
        {
            //arrange
            var player = CreatePlayer(balance, lossLimit, currentLoss);

            //act
            var isLossLimitReached = player.WillReachLossLimit(amount);
            
            //assert
            Assert.False(isLossLimitReached);
        }
        
        [Theory]
        [InlineData(200, 199)]
        [InlineData(200, 200)]
        public void HasEnoughBalance_EnoughBalance_ReturnTrue(decimal balance, decimal amountToBet)
        {
            //arrange
            var player = CreatePlayer(balance, 0, 0);

            //act
            var hasEnoughBalance = player.HasEnoughBalance(amountToBet);
            
            //assert
            Assert.True(hasEnoughBalance);
        }

        [Theory]
        [InlineData(200, 299)]
        [InlineData(200, 201)]
        public void HasEnoughBalance_NotEnoughBalance_ReturnFalse(decimal balance, decimal amountToBet)
        {
            //arrange
            var player = CreatePlayer(balance, 0, 0);

            //act
            var hasEnoughBalance = player.HasEnoughBalance(amountToBet);
            
            //assert
            Assert.False(hasEnoughBalance);
        }


        private static Player CreatePlayer(decimal balance, decimal lossLimit, decimal currentLoss)
        {
            return new Player
            {
                Balance = balance,
                LossLimit = lossLimit,
                CurrentLoss = currentLoss
            };
        }
    }
}