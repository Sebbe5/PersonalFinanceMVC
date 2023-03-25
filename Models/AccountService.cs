using Microsoft.AspNetCore.Identity;
using PersonalFinance.Views.Home;
using PersonalFinanceMVC.Models;

namespace PersonalFinance.Models
{
    public class AccountService
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        string userId;

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

        public async Task<string> TryRegister(RegisterVM viewModel)
        {
            var user = new ApplicationUser
            {
                UserName = viewModel.Username,
            };

            IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);

            bool wasUserCreated = result.Succeeded;
            return wasUserCreated ? null : "Failed to create user";
        }

        public async Task<bool> TryLogin(LoginVM vm)
        {
            SignInResult result = await signInManager.PasswordSignInAsync(
                vm.Username,
                vm.Password,
                isPersistent: false,
                lockoutOnFailure: false
                );

            return result.Succeeded;
        }

        internal async void LogOut()
        {
            await signInManager.SignOutAsync();
        }

        
    }
}
