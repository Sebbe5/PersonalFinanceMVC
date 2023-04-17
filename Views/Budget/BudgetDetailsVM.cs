namespace PersonalFinanceMVC.Views.Budget
{
    public class BudgetDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExpenseItemVM[] Expenses{ get; set; }

        public class ExpenseItemVM
        {
            public string Name { get; set; }
            public double Amount { get; set; }
        }
    }
}
