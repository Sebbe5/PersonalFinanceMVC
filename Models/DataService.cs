using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Budget;

namespace PersonalFinanceMVC.Models
{
    // TODO: Split up the services in several so that this service does not contain all logged in functionality
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

        internal EditBudgetVM CreateEditBudgetVM(int id)
        {
            // Map expenses to ExpenseItemVM objects
            var expenseItems = context.Expenses.Where(e => e.BudgetId == id).Select(e => new EditBudgetVM.ExpenseItemVM
            {
                Name = e.Name,
                Amount = e.Money
            })
            .ToList();

            // Create the EditDetailsVM, set its properties and return it from the method
            return new EditBudgetVM
            {
                Name = context.Budgets.SingleOrDefault(b => b.Id == id).Name,
                Expenses = expenseItems
            };
        }

        internal void AddBudgetToDB(CreateBudgetVM vm)
        {
            // Create an instance of a budget and set its values
            Budget newBudget = new Budget() { Name = vm.Name, ApplicationUserId = userId };

            // Add the budget instance to the list of budgets in the context (for the database)
            context.Budgets.Add(newBudget);

            // Save the changes and store in the database
            // This will give the budget an ID
            context.SaveChanges();

            // For each expense in the view model, create a new instance of an expense, set its values and add the instances to the list of expenses in the context (for the database)
            foreach (var expenseItemVM in vm.Expenses)
            {
                if (expenseItemVM.Name != null && expenseItemVM.Amount != 0)
                {
                    context.Expenses.Add(new Expense
                    {
                        Name = expenseItemVM.Name,
                        Money = expenseItemVM.Amount,
                        BudgetId = newBudget.Id,
                    });
                }
            }

            // Save the changes and store in the database
            context.SaveChanges();
        }

        internal void EditBudget(EditBudgetVM vm, int id)
        {
            // Retrieve the budget to be edited
            var budgetToEdit = context.Budgets.Include(b => b.Expenses).SingleOrDefault(b => b.Id == id);

            // Update name of budget
            budgetToEdit.Name = vm.Name;

            // Clear all expenses since it's hard to track if some expenses where deleted from the list
            budgetToEdit.Expenses.Clear();

            // Check if there are any expenses in the view model list
            if (vm.Expenses != null)
            {
                foreach (var expenseItemVM in vm.Expenses)
                {
                    if (expenseItemVM.Name != null && expenseItemVM.Amount != 0)
                    {
                        // Store expense if expense already exists in relation to the budget, otherwise store null
                        var existingExpense = budgetToEdit.Expenses.FirstOrDefault(e => e.Name == expenseItemVM.Name);

                        // Check if expense already exists
                        if (existingExpense != null)
                        {
                            // If expense exists, update its properties
                            existingExpense.Money = expenseItemVM.Amount;
                        }
                        else
                        {
                            context.Expenses.Add(new Expense
                            {
                                Name = expenseItemVM.Name,
                                Money = expenseItemVM.Amount,
                                BudgetId = id,
                            });

                        }
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
