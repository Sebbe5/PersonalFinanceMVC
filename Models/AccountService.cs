using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Models.Entities;

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
    }
}
