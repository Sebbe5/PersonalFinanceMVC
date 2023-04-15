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
        internal BudgetsVM CreateBudgetsVM()
        {
            // Map budgets to BudgetItemVM objects
            var budgetItems = context.Budgets.Where(b => b.ApplicationUserId == userId).Select(b => new BudgetsVM.BudgetItemVM
            {
                Id = b.Id,
                Name = b.Name,
            })
            .ToArray();

            // Create the BudgetsVM, set its properties and return it from the method
            return new BudgetsVM
            {
                Budgets = budgetItems
            };
        }

        internal BudgetDetailsVM CreateBudgetDetailsVM(int id)
        {
            // Map expenses to ExpenseItemVM objects
            var expenseItems = context.Expenses.Where(e => e.BudgetId == id).Select(e => new BudgetDetailsVM.ExpenseItemVM
            {
                Name = e.Name,
                Amount = e.Money
            })
            .ToArray();

            // Create the BudgetDetailsVM, set its properties and return it from the method
            return new BudgetDetailsVM
            {
                Name = context.Budgets.SingleOrDefault(b => b.Id == id).Name,
                Expenses = expenseItems
            };
        }

        internal void AddBudgetToDB(CreateBudgetVM vm)
        {
            // TODO: Comment what you have done
            Budget newBudget = new Budget() { Name = vm.Name, ApplicationUserId = userId};
            context.Budgets.Add(newBudget);
            context.SaveChanges();

            foreach (var expense in vm.Expenses)
            {
                if (expense.Name != null && expense.Amount != 0)
                {
                    context.Expenses.Add(new Expense
                    {
                        Name = expense.Name,
                        Money = expense.Amount,
                        BudgetId = newBudget.Id,
                    });
                }
            }

            context.SaveChanges();
        }
    }
}
