using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Budget;

namespace PersonalFinanceMVC.Models
{
    public class BudgetService
    {
        // TODO: Continue commenting here

        UserManager<ApplicationUser> userManager;
        readonly string userId;

        private readonly ApplicationContext context;
        public BudgetService(
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
            var budgetItems = GetUserBudgets().Select(b => new BudgetsVM.BudgetItemVM
            {
                Id = b.Id,
                Name = b.Name,
                TotalAmount = b.Expenses.Where(e => e.IsActive).Sum(e => e.Money)
            })
            .ToArray();

            // Create the BudgetsVM, set its properties and return it from the method
            return new BudgetsVM { Budgets = budgetItems };
        }
        internal BudgetDetailsVM CreateBudgetDetailsVM(int id)
        {
            // Fetch budget
            var budgetToReturn = GetBudgetById(id);

            // Fetch budget expenses
            var expenses = GetExpenses(budgetToReturn);

            // Create categories (create model for this in the future)
            var categories = new[] { "Housing", "Transportation", "Food", "Utilities", "Health and Fitness", "Entertainment", "Personal Care", "Education", "Savings", "Others", "Uncategorized" };
            double[] categoryAmounts = new double[categories.Count()];

            // Sort and sum expenses in categories
            SortAndSumExpenses(expenses, categoryAmounts);

            // Return view model
            return new BudgetDetailsVM
            {
                Id = budgetToReturn.Id,
                Name = budgetToReturn.Name,
                Expenses = expenses.Select(e => new BudgetDetailsVM.ExpenseItemVM
                {
                    Name = e.Name,
                    Amount = e.Money,
                    Category = e.Category != null ? e.Category : string.Empty,
                    IsActive = e.IsActive,
                }).ToArray(),
                TotalAmount = expenses.Where(e => e.IsActive).Sum(e => e.Money),
                Categories = categories,
                CategoryAmounts = categoryAmounts
            };
        }
        internal EditBudgetVM CreateEditBudgetVM(int id)
        {
            // Return view model from budget table included with expense table
            return context.Budgets.Include(b => b.Expenses.Where(e => e.BudgetId == b.Id))
                .Where(b => b.Id == id)
                .Select(b => new EditBudgetVM
                {
                    Name = b.Name,
                    Expenses = b.Expenses.Select(e => new EditBudgetVM.ExpenseItemVM
                    {
                        Name = e.Name,
                        Amount = e.Money,
                        Category = e.Category,
                        IsActive = e.IsActive
                    })
                    .ToList()
                })
                .SingleOrDefault();
        }
        internal void AddBudgetToDB(CreateBudgetVM vm)
        {
            // Check if name exists
            string name = AlterNonUniqueName(vm.Name);

            // Create an instance of a budget and set its values
            Budget newBudget = new Budget() { Name = name, ApplicationUserId = userId };

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
                        Category = expenseItemVM.Category,
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
            budgetToEdit.Name = budgetToEdit.Name == vm.Name ? budgetToEdit.Name : AlterNonUniqueName(vm.Name);

            // Clear all expenses since it's hard to track if some expenses where deleted from the list
            budgetToEdit.Expenses.Clear();

            // Fetch the names of all existing expenses of the budget to be edited
            var existingExpenses = new HashSet<string>(budgetToEdit.Expenses.Select(e => e.Name));

            ValidateExpense(vm, id, budgetToEdit, existingExpenses);

            // Save DB changes
            context.SaveChanges();
        }
        internal void RemoveBudget(int id)
        {
            // Remove the budget and all its related expenses
            context.Budgets.Remove(context.Budgets.SingleOrDefault(b => b.Id == id));

            // Save changes to DB
            context.SaveChanges();
        }
        private string AlterNonUniqueName(string name)
        {
            // Get the names of all budgets
            HashSet<string> budgetNames = new HashSet<string>(context.Budgets.Select(b => b.Name), StringComparer.OrdinalIgnoreCase);

            // Check if name already exist
            bool isExisting = budgetNames.Contains(name);

            // Declaring the number that will be appended to the name if the name already exists
            int NumberToAppend = 2;

            // Loop until the name does not exist
            while (isExisting)
            {
                // If the loop have passed the first iteration, remove the last char, other wise don't
                if (NumberToAppend > 2)
                    name = name.Substring(0, name.Length - 1) + $"{NumberToAppend}";
                else
                    name += $"{NumberToAppend}";

                isExisting = budgetNames.Contains(name, StringComparer.OrdinalIgnoreCase);
                NumberToAppend++;
            }

            return name;
        }
        private List<Expense> GetExpenses(Budget budgetToReturn) => context.Expenses.Where(e => e.BudgetId == budgetToReturn.Id).ToList();
        private Budget GetBudgetById(int id) => context.Budgets.Where(b => b.Id == id).FirstOrDefault();
        private IQueryable<Budget> GetUserBudgets() => context.Budgets.Where(b => b.ApplicationUserId == userId);
        private void ValidateExpense(EditBudgetVM vm, int id, Budget budgetToEdit, HashSet<string> existingExpenses)
        {
            // Check if there are any expenses in the view model list
            if (vm.Expenses != null)
            {
                // Loop through each expense in the view model
                foreach (var expenseItemVM in vm.Expenses)
                {
                    // Check if the name of an expense exists and that the amount of the expense is not 0
                    if (expenseItemVM.Name != null && expenseItemVM.Amount != 0)
                    {
                        // Check if expense already exists
                        if (existingExpenses.Contains(expenseItemVM.Name, StringComparer.OrdinalIgnoreCase))
                        {
                            // If expense exists, update its properties
                            var existingExpense = budgetToEdit.Expenses.First(e => String.Equals(e.Name, expenseItemVM.Name, StringComparison.OrdinalIgnoreCase));
                            existingExpense.Money = expenseItemVM.Amount;
                        }
                        else
                        {
                            // If expense does not exist, add to DB
                            context.Expenses.Add(new Expense
                            {
                                Name = expenseItemVM.Name,
                                Money = expenseItemVM.Amount,
                                Category = expenseItemVM.Category,
                                IsActive = expenseItemVM.IsActive,
                                BudgetId = id,
                            });

                            // Add expense to DB table
                            existingExpenses.Add(expenseItemVM.Name);
                        }
                    }
                }
            }
        }
        private static void SortAndSumExpenses(List<Expense> expenses, double[] categoryAmounts)
        {
            Dictionary<string, int> categoryIndices = new Dictionary<string, int>()
            {
                { "Housing", 0 },
                { "Transportation", 1 },
                { "Food", 2 },
                { "Utilities", 3 },
                { "Health and Fitness", 4 },
                { "Entertainment", 5 },
                { "Personal Care", 6 },
                { "Education", 7 },
                { "Savings", 8 },
                { "Others", 9 }
            };

            foreach (var expense in expenses)
            {
                if (expense.IsActive)
                {
                    if (categoryIndices.TryGetValue(expense.Category, out int categoryIndex))
                    {
                        categoryAmounts[categoryIndex] += expense.Money;
                    }
                    else
                    {
                        categoryAmounts[10] += expense.Money;
                    }
                }
            }
        }
    }
}
