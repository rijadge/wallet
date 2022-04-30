using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using Moq;
using wallet.Controllers;
using wallet.Data;
using wallet.Models;
using wallet.Services;
using Wallet.UnitTests.Mocks.Services;
using Xunit;

namespace Wallet.UnitTests.Application
{
    public class WalletControllerTest
    {
        private readonly DbContextOptions<WalletContext> _dbOptions;
        
        public WalletControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<WalletContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;
            
            using (var dbContext = new WalletContext(_dbOptions))
            {
                DbInitializer.Initialize(dbContext);
            }
        }
        
        [Fact]
        public async Task GetPlayers_NonEmptyList_NotEmpty()
        {
            //arrange
            var dbContext = new WalletContext(_dbOptions);
            var accountServiceMock = new MockAccountService().MockGetPlayers(dbContext.Players.ToList());

            //act
            var walletController = new WalletController(accountServiceMock.Object);
            var players = await walletController.Players();

            //assert
            Assert.NotEmpty(players);
        }
        
        
        [Fact]
        public async Task Pay_PlayerNotFetched_PlayerNotFound()
        {
            //arrange
            var dbContext = new WalletContext(_dbOptions);
            var player = dbContext.Players.FirstOrDefault();
            var accountServiceMock = new MockAccountService().MockTryFetchPlayer(false);

            //act
            var walletController = new WalletController(accountServiceMock.Object);
            var playerAfterPayment = (NotFoundObjectResult)await walletController.Pay(player.Id, 1000);
            
            //assert
            Assert.Equal(404, playerAfterPayment.StatusCode);
        }
        
        [Fact]
        public async Task Pay_CreditsNotAdded_CreditsNotSaved()
        {
            //arrange
            var dbContext = new WalletContext(_dbOptions);
            var player = dbContext.Players.FirstOrDefault();
            var accountServiceMock = new MockAccountService().MockTryFetchPlayer(true).MockAddCredits(false);

            //act
            var walletController = new WalletController(accountServiceMock.Object);
            var playerAfterPayment = (ObjectResult)await walletController.Pay(player.Id, 1000);
            
            //assert
            Assert.Equal(500, playerAfterPayment.StatusCode);
        }
        
        [Fact]
        public async Task Pay_CreditsAdded_ResponseOk()
        {
            //arrange
            var dbContext = new WalletContext(_dbOptions);
            var player = dbContext.Players.FirstOrDefault();
            var accountServiceMock = new MockAccountService().MockTryFetchPlayer(true).MockAddCredits(true);

            //act
            var walletController = new WalletController(accountServiceMock.Object);
            var playerAfterPayment = (ObjectResult)await walletController.Pay(player.Id, 1000);
            
            //assert
            Assert.Equal(200, playerAfterPayment.StatusCode);
        }
        
    }
}