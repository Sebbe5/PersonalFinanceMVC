﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Budget;

namespace PersonalFinanceMVC.Models
{
    public class BudgetService
    {
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
            var budgetItems = context.Budgets
               .Include(b => b.Expenses) // Lets me have one database quey instead of two
               .Where(b => b.ApplicationUserId == userId)
               .Select(b => new BudgetsVM.BudgetItemVM
               {
                   Id = b.Id,
                   Name = b.Name,
                   TotalAmount = b.Expenses.Sum(e => e.Money)
               })
            .ToArray();

            // Create the BudgetsVM, set its properties and return it from the method
            return new BudgetsVM
            {
                Budgets = budgetItems,
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
                Id = id,
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
            string name = CheckIfNameExist(vm.Name);

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

            budgetToEdit.Name = budgetToEdit.Name == vm.Name ? budgetToEdit.Name : CheckIfNameExist(vm.Name);

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

        internal void RemoveBudget(int id)
        {
            // Remove the budget and all its related expenses
            context.Budgets.Remove(context.Budgets.SingleOrDefault(b => b.Id == id));
            context.SaveChanges();
        }

        private string CheckIfNameExist(string name)
        {
            // Get the names of all budgets
            var budgetNames = context.Budgets.Select(b => b.Name).ToList();

            // Check if name already exist
            bool isExisting = budgetNames.Contains(name);

            int counter = 2;

            // Loop until the name does not exist
            while (isExisting)
            {
                // If the loop have passed the first iteration, remove the last char, other wise don't
                if (counter > 2)
                    name = name.Substring(0, name.Length - 1) + $"{counter}";
                else
                    name += $"{counter}";

                isExisting = budgetNames.Contains(name);
                counter++;
            }

            return name;
        }
    }
}
