using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Budget
{
    public class EditBudgetVM
    {
        [Display(Name = "Budget Name:")]
        [Required(ErrorMessage = "Please enter a budget name.")]
        public string Name { get; set; }
        public List<ExpenseItemVM> Expenses { get; set; }
        public class ExpenseItemVM
        {
            public string Name { get; set; }
            public double Amount { get; set; }
            public string Category { get; set; }
        }
    }
}
