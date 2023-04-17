namespace PersonalFinanceMVC.Views.Budget
{
    public class BudgetsVM
    {
        public BudgetItemVM[] Budgets { get; set; }
        public class BudgetItemVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double TotalAmount { get; set; }
        }
    }
}
