namespace PersonalFinance.Views.Home
{
    public class BudgetVM
    {
        public List<BudgetItemVM> budgets = new List<BudgetItemVM>();
        public class BudgetItemVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
