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

        // TODO: Continue clean up here
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
            var a = vm.Goal;
            var p = vm.Principal;
            var r = vm.Rate / 100;
            var t = vm.Years;
            var n = 12;

            decimal epsilon = 0.01m; // Toleransnivå för approximation
            decimal lowerBound = 0; // Nedre gräns för insättningsbeloppet
            decimal upperBound = a; // Övre gräns för insättningsbeloppet
            decimal PMT = 0;

            int maxIterations = 1000; // Maximal antal iterationer
            decimal maxDifference = 0.01m; // Maximal differens mellan de nedre och övre gränserna

            int iterations = 0; // Räknare för antal iterationer

            // Bisektionsmetoden för att approximera PMT
            while (Math.Abs(a - CalculateFutureValue(p, r, t, n, PMT)) > epsilon)
            {
                PMT = (lowerBound + upperBound) / 2;

                decimal futureValue = CalculateFutureValue(p, r, t, n, PMT);

                if (futureValue > a)
                {
                    upperBound = PMT;
                }
                else
                {
                    lowerBound = PMT;
                }

                iterations++;

                // Avsluta loopen om maxIterations nås eller om differensen mellan de nedre och övre gränserna är för liten
                if (iterations >= maxIterations || Math.Abs(upperBound - lowerBound) < maxDifference)
                {
                    break;
                }
            }

            vm.MonthlyContribution = PMT;
            return vm;
        }

        public static decimal CalculateFutureValue(decimal P, decimal r, int t, int n, decimal PMT)
        {
            decimal futureValue = P * (decimal)Math.Pow(1 + (double)(r / n), n * t);

            for (int i = 1; i <= t * n; i++)
            {
                futureValue += PMT * (decimal)Math.Pow(1 + (double)(r / n), n * t - i);
            }

            return futureValue;
        }
    }
}
