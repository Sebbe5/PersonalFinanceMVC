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
            var q = context.Budgets.FirstOrDefault(b => b.Id == id);
            vm.BudgetName = q.Name;
            for (int i = 0; i < q.Expenses.Count; i++)
            {
                vm.Expenses[i] = new EditBudgetVM.ExpenseItemVM();
                vm.Expenses[i].Name = q.Expenses[i].Name;
                vm.Expenses[i].Amount = q.Expenses[i].Money;
            }
            return vm;

        }
    }
}
