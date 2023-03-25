namespace PersonalFinanceMVC.Views.Home
{
    public class EditBudgetVM
    {
        public string BudgetName { get; set; }
        public ExpenseItemVM[] Expenses { get; set; }
        
        public class ExpenseItemVM
        {
            public string Name { get; set; }
            public double Amount { get; set; }
        }
    }
}
