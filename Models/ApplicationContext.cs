using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;

namespace PersonalFinanceMVC.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    }
}
