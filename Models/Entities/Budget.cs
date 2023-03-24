namespace PersonalFinanceMVC.Models.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Expense> Expenses { get; set; }
    }
}
