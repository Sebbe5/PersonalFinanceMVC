using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Compound
{
    public class CalculateVM
    {
        [Display(Name = "Principal Amount")]
        public decimal Principal { get; set; }

        [Display(Name = "Monthly Contribution")]
        public decimal MonthlyContribution { get; set; }

        [Display(Name = "Annual interest rate (%)")]
        public decimal Rate { get; set; }

        [Display(Name = "Number of years")]
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
