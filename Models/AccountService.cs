using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Login;

namespace PersonalFinanceMVC.Models
{
    public class AccountService
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        readonly string userId;

        private readonly ApplicationContext context;
        
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor accessor,
            ApplicationContext context)

        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
            userId = userManager.GetUserId(accessor.HttpContext.User);
        }

        public async Task<string> TryLoginAsync(LoginVM viewModel)
        {
            // Check if the password is null
            if (viewModel.Password == null)
            {
                return null; // Return null if the password is null
            }

            // Try to sign in using the provided username and password
            SignInResult result = await signInManager.PasswordSignInAsync(
                viewModel.Username,
                viewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            // Check if the sign-in attempt succeeded
            if (result.Succeeded)
                return null; // Return null if the sign-in was successful
            else
                return "Either the username or password is incorrect"; // Return an error message if the sign-in failed
        }
        public async Task<string> TryRegisterAsync(RegisterVM viewModel)
        {
            // Create a new instance of the ApplicationUser with the provided username and email
            var user = new ApplicationUser
            {
                UserName = viewModel.Username,
                Email = viewModel.Email,
            };

            // Check if the password is null
            if (viewModel.Password == null)
            {
                return null; // Return null if the password is null
            }

            // Attempt to create a new user with the provided user object and password
            IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);

            // Check if the user creation was successful
            return result.Errors.FirstOrDefault()?.Description;
            // Return the description of the first error, if any, or null if the creation was successful
        }
        internal void SignOut()
        {
            // Initiate the sign out process asynchronously
            var result = signInManager.SignOutAsync();
        }
    }
}
