namespace PersonalFinanceMVC.Models.Entities
{
    public class Investment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double InitialValue { get; set; }
        public double Value { get; set; }
        public int ExpectedYearsInvested { get; set; }
        public double RecurringDeposit { get; set; }
        public decimal ExpectedAnnualInterest { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
