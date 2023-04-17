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
            if (vm.Compounds == 0) return vm;
            // Set necessary values
            decimal principal = vm.Principal;
            decimal interestRate = vm.Rate / 100;
            int compounds = vm.Compounds;
            //decimal compoundAmount = principal * (decimal)Math.Pow((1 + (double)(interestRate / compounds)), (compounds * years));
            //decimal compoundInterest = compoundAmount - principal;

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
