namespace PersonalFinanceMVC.Views.Compound
{
    public class CalculateVM
    {
        public decimal Principal { get; set; }
        public decimal MonthlyContribution { get; set; }
        public decimal Rate { get; set; }
        public int Years { get; set; }
        public List<CompoundInterestResult> Results { get; set; } = new List<CompoundInterestResult>();

        public class CompoundInterestResult
        {
            public decimal Contribution { get; set; }
            public int Year { get; set; }
            public decimal Interest { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
