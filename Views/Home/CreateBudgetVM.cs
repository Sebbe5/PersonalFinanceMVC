

using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Home
{
    public class CreateBudgetVM
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }
    }
}
