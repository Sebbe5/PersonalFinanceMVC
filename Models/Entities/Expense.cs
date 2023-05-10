namespace PersonalFinanceMVC.Models.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Money { get; set; }
        public string? Category { get; set; }
        public int BudgetId { get; set; }
        public Budget Budget { get; set; }
    }
}
