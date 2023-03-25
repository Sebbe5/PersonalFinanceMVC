namespace PersonalFinanceMVC.Views.Home
{
    public class EditBudgetVM
    {
        public string BudgetName { get; set; }
        public List<ExpenseItemVM> Expenses { get; set; } = new List<ExpenseItemVM>();
        
        public class ExpenseItemVM
        {
            public string Name { get; set; }
            public double Amount { get; set; }
        }
    }
}
