using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Home
{
    public class EditBudgetVM
    {
        public string BudgetName { get; set; }

        [Display(Name = "Amount", Prompt = "e.g. 5000")]
        [Range(0, int.MaxValue)]// TODO: This validation does not work properly
        public double NewAmount { get; set; }

        [Display(Name = "Name", Prompt = "e.g. Transport")]
        [Required(ErrorMessage = "Please enter a name")]
        public string NewName { get; set; }
        public List<ExpenseItemVM> Expenses { get; set; } = new List<ExpenseItemVM>();
        
        public class ExpenseItemVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Amount { get; set; }
        }
    }
}
