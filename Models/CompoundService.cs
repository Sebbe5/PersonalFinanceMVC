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
            decimal principal = vm.Principal;
            decimal monthlyContributions = vm.MonthlyContribution;
            int compounds = 12;
            decimal interestRate = vm.Rate / 100;
            decimal ratePerMonth = interestRate / compounds;
            decimal totalContribution = principal;
            decimal interest = 0;

            // Fill the Results collection in the view model with the calculated results
            for (int i = 0; i < vm.Years; i++)
            {
                var result = new CalculateVM.CompoundInterestResult();
                
                // Set year and principal
                result.Year = i;

                // Loop through months
                for (int j = 0; j < 12; j++)
                {
                    totalContribution += monthlyContributions;
                    interest += totalContribution * ratePerMonth;
                }

                result.Contribution = totalContribution;
                result.Interest = interest;

                result.Amount = totalContribution + interest;

                vm.Results.Add(result);

                principal = result.Amount;
            }
            return vm;
        }
    }
}
