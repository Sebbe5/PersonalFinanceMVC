using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace PersonalFinanceMVC.Views.Investment
{
    public class InvestmentDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<double> Contributions { get; set; } = new List<double>();
        public List<double> Profit { get; set; } = new List<double>();

        // TODO: Continue with this vm
    }
}
