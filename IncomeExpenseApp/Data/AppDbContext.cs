using IncomeExpenseApp.Model;
using Microsoft.EntityFrameworkCore;

namespace IncomeExpenseApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options)
            :base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
