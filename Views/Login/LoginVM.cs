using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Login
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
