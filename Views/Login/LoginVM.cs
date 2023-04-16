using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Login
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [Display(Prompt = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [Display(Prompt = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
