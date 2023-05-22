using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Views.Shared.Compound;
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

        internal _PredictionCalculatorVM Update_PredictionCalculatorVM(_PredictionCalculatorVM vm)
        {
            // Set necessary values
            decimal principal = vm.Principal;
            decimal monthlyContributions = vm.MonthlyContribution;
            decimal interestRate = vm.Rate / 100;
            decimal totalContribution = principal;
            decimal totalInterest = 0;

            // Fill the Results collection in the view model with the calculated results
            for (int i = 0; i < vm.Years; i++)
            {
                var result = new _PredictionCalculatorVM.CompoundInterestResult();

                // Set year and principal
                result.Year = i + 1;

                decimal interest = (totalContribution + totalInterest) * interestRate;
                result.Interest = interest;
                totalInterest += interest;

                totalContribution += monthlyContributions * 12;
                result.Contribution = totalContribution;

                result.Amount = totalContribution + totalInterest;

                vm.Results.Add(result);
            }
            return vm;
        }

        internal _GoalCalculatorVM Update_GoalCalculatorVM(_GoalCalculatorVM vm)
        {
            // TODO: Fixa matten
            decimal annualInterestRate = vm.Rate / 100;
            int totalMonths = vm.Years * 12;

            vm.MonthlyContribution = (vm.Goal - vm.Principal * (decimal)Math.Pow(1 + (double)annualInterestRate, vm.Years)) / (totalMonths);
            
            return vm;
        }
    }
}
