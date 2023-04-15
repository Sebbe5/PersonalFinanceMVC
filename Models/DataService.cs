using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models.Entities;

namespace PersonalFinanceMVC.Models
{
    public class DataService
    {
        UserManager<ApplicationUser> userManager;
        readonly string userId;

        private readonly ApplicationContext context;
        public DataService(
            ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor accessor
            )
        {
            this.context = context;
            this.userManager = userManager;
            userId = userManager.GetUserId(accessor.HttpContext.User);
        }
    }
}
