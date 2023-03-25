using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Login
{
    public class RegisterVM
    {
        [Display(Name = "Username", Prompt = "Max 12 characters")]
        [Required]
        [MaxLength(12)]
        public string Username { get; set; }

        [Display(Name = "Password", Prompt = "1 capital, 1 special character")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare(nameof(Password))]
        public string PasswordRepeat { get; set; }
    }
}
