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
            if (viewModel.Password == null)
            {
                return null;
            }
            SignInResult result = await signInManager.PasswordSignInAsync(
                viewModel.Username,
                viewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false);
            if (result.Succeeded)
                return null;
            else
                return "Either the username or password is incorrect";
        }

        public async Task<string> TryRegisterAsync(RegisterVM viewModel)
        {
            // TODO: Start commenting here
            var user = new ApplicationUser
            {
                UserName = viewModel.Username,
                Email = viewModel.Email,
            };

            if (viewModel.Password == null)
            {
                return null;
            }
            IdentityResult result = await
                userManager.CreateAsync(user, viewModel.Password);

            return result.Errors.FirstOrDefault()?.Description;
        }

        internal void SignOut()
        {
            var result = signInManager.SignOutAsync();
        }

    }
}
