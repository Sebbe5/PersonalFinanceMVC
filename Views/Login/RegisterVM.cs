using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Login
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Prompt = "Username")]
        [StringLength(12, ErrorMessage = "The {0} must not exceed {1} characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Prompt = "E-mail")]
        [EmailAddress(ErrorMessage = "The {0} field is not a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Prompt = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Prompt = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
