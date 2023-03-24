using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;
using System.Data;

namespace PersonalFinanceMVC.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Money)
                .HasColumnType(SqlDbType.Money.ToString());
        }
    }
}
