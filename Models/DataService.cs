using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Budget;

namespace PersonalFinanceMVC.Models
{
    public class DataService
    {
        UserManager<ApplicationUser> userManager;
        readonly string userId;

        private readonly ApplicationContext context;
        public DataService(
            ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor accessor
            )
        {
            this.context = context;
            this.userManager = userManager;
            userId = userManager.GetUserId(accessor.HttpContext.User);
        }

        public Budget[] GetUserBudgets() => context.Budgets.Where(b => b.ApplicationUserId == userId).ToArray();

        internal BudgetsVM CreateBudgetsVM()
        {
            // Get all budgets mathing the user ID from database
            var budgets = GetUserBudgets();

            // Map budgets to BudgetItemVM objects
            var budgetItems = budgets.Select(b => new BudgetsVM.BudgetItemVM
            {
                Id = b.Id,
                Name = b.Name,
            })
            .ToArray();

            // Create the BudgetVM, set its properties and return it from the method
            return new BudgetsVM
            {
                Budgets = budgetItems
            };
        }

        internal BudgetDetailsVM CreateBudgetDetailsVM(int id)
        {
            var budgets = GetUserBudgets();
        }
    }
}
