using Microsoft.EntityFrameworkCore;

namespace BinaryParkingClientServerNiverovskyi.Models.Transaction
{
    public class TransactionContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transaction> MinuteTransactions { get; set; }

        public TransactionContext(DbContextOptions options) : base(options)
        {
        }
    }
}