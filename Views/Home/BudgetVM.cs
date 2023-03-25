namespace PersonalFinance.Views.Home
{
    public class BudgetVM
    {
        public BudgetItemVM[] budgets;
        public class BudgetItemVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
