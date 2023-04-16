using PersonalFinanceMVC.Models.Entities;

namespace PersonalFinanceMVC.Views.Budget
{
    public class EditBudgetVM
    {
        public string Name { get; set; }
        public List<ExpenseItemVM> Expenses { get; set; }
        public class ExpenseItemVM
        {
            public string Name { get; set; }
            public double Amount { get; set; }
        }
    }
}
