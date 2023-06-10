using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;
using System.Data;

namespace PersonalFinanceMVC.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        // Constructor that takes DbContextOptions as a parameter, which is used to configure the context.
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        // DbSet represents a collection of entities (tables) in the database.
        public DbSet<Budget> Budgets { get; set; }       // Represents the "Budgets" table in the database.
        public DbSet<Expense> Expenses { get; set; }     // Represents the "Expenses" table in the database.
        public DbSet<Todo> Todos { get; set; }           // Represents the "Todos" table in the database.
        public DbSet<Investment> Investments { get; set; } // Represents the "Investments" table in the database.

        // The following method is called when the model for the context is being created.
        // It is used to configure the entities and their relationships, as well as any additional settings.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base class implementation to perform any base model configuration.
            base.OnModelCreating(modelBuilder);

            // Configure the "Expense" entity.
            modelBuilder.Entity<Expense>()
                .Property(e => e.Money)
                .HasColumnType(SqlDbType.Money.ToString()); // Set the column type of "Money" property to the SQL Server money type.

            // Configure the "Investment" entity.
            modelBuilder.Entity<Investment>()
                .Property(i => i.InitialValue)
                .HasColumnType(SqlDbType.Money.ToString()); // Set the column type of "InitialValue" property to the SQL Server money type.

            modelBuilder.Entity<Investment>()
                .Property(i => i.RecurringDeposit)
                .HasColumnType(SqlDbType.Money.ToString()); // Set the column type of "RecurringDeposit" property to the SQL Server money type.

            modelBuilder.Entity<Investment>()
                .Property(i => i.Value)
                .HasColumnType(SqlDbType.Money.ToString()); // Set the column type of "Value" property to the SQL Server money type.

            modelBuilder.Entity<Investment>()
                .Property(i => i.ExpectedAnnualInterest)
                .HasColumnType(SqlDbType.Decimal.ToString()) // Set the column type of "ExpectedAnnualInterest" property to the SQL Server decimal type.
                .HasPrecision(18, 2); // Set the precision and scale for the decimal property.

            // Configure the "ApplicationUser" entity.
            modelBuilder.Entity<ApplicationUser>().Property(x => x.UserName).HasMaxLength(12); // Set the maximum length of the "UserName" property to 12 characters.
        }
    }
}
