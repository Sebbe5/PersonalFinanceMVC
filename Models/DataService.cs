using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Home;

namespace PersonalFinanceMVC.Models
{
    public class DataService
    {
        private readonly ApplicationContext context;
        public DataService(ApplicationContext context)
        {
            this.context = context;
        }

        public EditBudgetVM GetBudgetNameAndExpenses(int id)
        {
            EditBudgetVM vm = new EditBudgetVM();
            vm.BudgetName = context.Budgets
                .Select(b => new
                {
                    b.Id,
                    b.Name
                })
                .FirstOrDefault(b => b.Id == id)
                .Name;
            var q = GetAllExpenses(id);
            for (int i = 0; i < q.Length; i++)
                vm.Expenses.Add(new EditBudgetVM.ExpenseItemVM { Name = q[i].Name, Amount = q[i].Money });
            
            return vm;
        }

        public Expense[] GetAllExpenses(int id) => context.Expenses.Where(e => e.BudgetId == id).ToArray();

    }
}
