using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;
using System.Data;

namespace PersonalFinanceMVC.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        // TODO: Continue commenting here
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Investment> Investments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Money)
                .HasColumnType(SqlDbType.Money.ToString());

            modelBuilder.Entity<Investment>()
                .Property(i => i.InitialValue)
                .HasColumnType(SqlDbType.Money.ToString());

            modelBuilder.Entity<Investment>()
                .Property(i => i.RecurringDeposit)
                .HasColumnType(SqlDbType.Money.ToString());

            modelBuilder.Entity<Investment>()
                .Property(i => i.Value)
                .HasColumnType(SqlDbType.Money.ToString());

            modelBuilder.Entity<Investment>()
                .Property(i => i.ExpectedAnnualInterest)
                .HasColumnType(SqlDbType.Decimal.ToString())
                .HasPrecision(18, 2);

            modelBuilder.Entity<ApplicationUser>().Property(x => x.UserName).HasMaxLength(12);

            
        }
    }
}
