using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Views.Compound;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalFinanceMVC.Models
{
    public class CompoundService
    {
        UserManager<ApplicationUser> userManager;
        readonly string userId;

        private readonly ApplicationContext context;
        public CompoundService(
            ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor accessor
            )
        {
            this.context = context;
            this.userManager = userManager;
            userId = userManager.GetUserId(accessor.HttpContext.User);
        }

        internal CalculateVM UpdateCalculateVM(CalculateVM vm)
        {
            // Set necessary values
            int compounds = 12;
            decimal principal = vm.Principal;
            decimal interestRate = vm.Rate / 100;

            // Fill the Results collection in the view model with the calculated results
            for (int i = 0; i < vm.Years; i++)
            {
                decimal compoundAmountThisYear = principal * (decimal)Math.Pow((1 + (double)(interestRate / compounds)), (compounds));
                var result = new CalculateVM.CompoundInterestResult
                {
                    Year = i,
                    Principal = principal,
                    Amount = compoundAmountThisYear,
                    Interest = compoundAmountThisYear - principal,
                };
                vm.Results.Add(result);
                principal = compoundAmountThisYear;
            }
            return vm;
        }
    }
}
