using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace PersonalFinanceMVC.Views.Investment
{
    public class InvestmentDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double InitialValue { get; set; }
        public double RecurringDeposit { get; set; }
        public decimal ExpectedAnnualInterest { get; set; }
        public int ExpectedYearsInvested { get; set; }
        public List<double> Contributions { get; set; } = new List<double>();
        public List<double> Profits { get; set; } = new List<double>();
        public List<double> TotalAmounts { get; set; } = new List<double>();
    }
}
