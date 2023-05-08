namespace PersonalFinanceMVC.Views.Investment
{
    public class InvestmentsVM
    {

        // TODO: Add expected years invested (and add everything that should follow from it)
        public List<InvestmentItemVM> Investments { get; set; } = new List<InvestmentItemVM>();
        public class InvestmentItemVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }
    }
}
