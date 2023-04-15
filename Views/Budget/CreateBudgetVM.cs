using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Budget
{
    public class CreateBudgetVM
    {
        [Required(ErrorMessage = "Please enter a budget name.")]
        public string Name { get; set; }
        public List<ExpenseItemVM> Expenses { get; set; } = new List<ExpenseItemVM>();

        public class ExpenseItemVM
        {
            public string Name { get; set; }
            public double Amount { get; set; }
        }
    }
}
