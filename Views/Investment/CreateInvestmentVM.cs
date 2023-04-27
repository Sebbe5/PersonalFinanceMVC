using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Investment
{
    public class CreateInvestmentVM
    {

        [Display(Name = "Investment Name: ")]
        [Required(ErrorMessage = "A name is required!")]
        public string Name { get; set; }

        [Display(Name = "Initial Amount: ")]
        public double InitialValue { get; set; }

        [Display(Name = "Monthly Contribution: ")]
        public double MonthlyContribution { get; set; }

        [Display(Name = "Expected Annual Interest (%): ")]
        public double AnnualInterest { get; set; }
    }
}
