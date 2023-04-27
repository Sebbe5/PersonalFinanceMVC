using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Views.Investment;

namespace PersonalFinanceMVC.Models
{
    public class InvestmentService
    {
        UserManager<ApplicationUser> userManager;
        readonly string userId;

        private readonly ApplicationContext context;
        public InvestmentService(
            ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor accessor
            )
        {
            this.context = context;
            this.userManager = userManager;
            userId = userManager.GetUserId(accessor.HttpContext.User);
        }

        internal InvestmentsVM CreateInvestmentsVM()
        {
            var investments = context.Investments
                .Where(i => i.ApplicationUserId == userId)
                .Select(i => new InvestmentsVM.InvestmentItemVM
                {
                    Id = i.Id,
                    Name = i.Name,
                    Value = i.Value,
                })
                .ToList();

            return new InvestmentsVM
            {
                Investments = investments
            };
        }
    }
}
