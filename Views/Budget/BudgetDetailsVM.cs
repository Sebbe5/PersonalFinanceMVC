namespace PersonalFinanceMVC.Views.Budget
{
    public class BudgetDetailsVM
    {
        public string Name { get; set; }

        public class ExpenseItemVM
        {
            public string Name { get; set; }
            public double Amount { get; set; }
        }
    }
}
