using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Compound
{
    public class CalculateVM
    {
        [Display(Name = "Principal Amount:")]
        public decimal Principal { get; set; }

        [Display(Name = "Monthly Contribution:")]
        public decimal MonthlyContribution { get; set; }

        [Display(Name = " Annual interest rate (%):")]
        public decimal Rate { get; set; }

        [Display(Name = "Amount of years:")]
        public int Years { get; set; }

        public List<CompoundInterestResult> Results { get; set; } = new List<CompoundInterestResult>();

        public class CompoundInterestResult
        {
            [Display(Name = "Total contribution:")]
            public decimal Contribution { get; set; }

            [Display(Name = "Year")]
            public int Year { get; set; }

            [Display(Name = "Interest")]
            public decimal Interest { get; set; }

            [Display(Name = "Amount")]
            public decimal Amount { get; set; }
        }
    }
}
