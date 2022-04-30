using Microsoft.EntityFrameworkCore;

namespace wallet.Models
{
    public class WalletContext : DbContext
    {
        public WalletContext(DbContextOptions<WalletContext> options) : base(options)
        {
        }
        
        public DbSet<Player> Players { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


    }
}