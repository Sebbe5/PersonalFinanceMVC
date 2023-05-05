using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Shared
{
    public class _CompoundCalculatorInputVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
    }
}
