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
            decimal principal = vm.Principal; // Starting value
            decimal monthlyContributions = vm.MonthlyContribution; // Monthly contribution
            decimal interestRate = vm.Rate / 100; // Expected interest rate
            decimal totalContribution = principal; // Total Contribution
            decimal totalInterest = 0; // Total Interest

            // Fill the results list of the VM
            FillResults(vm, monthlyContributions, interestRate, ref totalContribution, ref totalInterest);
            return vm;
        }
        internal _GoalCalculatorVM Update_GoalCalculatorVM(_GoalCalculatorVM vm)
        {
            var a = vm.Goal; // Goal amount on investment
            var p = vm.Principal; // Starting amount of investment
            var r = vm.Rate / 100; //Expected interest rate
            var t = vm.Years; // Amount of years to invest
            var n = 12; // Amount of iterations (equal to the amount of months in a year)

            decimal epsilon = 0.01m; // Toleransnivå för approximation
            decimal lowerBound = 0; // Nedre gräns för insättningsbeloppet
            decimal upperBound = a; // Övre gräns för insättningsbeloppet
            decimal PMT = 0;

            int maxIterations = 1000; // Maximal antal iterationer
            decimal maxDifference = 0.01m; // Maximal differens mellan de nedre och övre gränserna

            int iterations = 0; // Räknare för antal iterationer

            BisectionMethod(a, p, r, t, n, epsilon, ref lowerBound, ref upperBound, ref PMT, maxIterations, maxDifference, ref iterations);

            vm.MonthlyContribution = PMT;
            return vm;
        }
        public static decimal CalculateFutureValue(decimal P, decimal r, int t, int n, decimal PMT)
        {
            // Calculate future value formula
            decimal futureValue = P * (decimal)Math.Pow(1 + (double)(r / n), n * t);
            // TODO: Continue commenting here
            for (int i = 1; i <= t * n; i++)
            {
                futureValue += PMT * (decimal)Math.Pow(1 + (double)(r / n), n * t - i);
            }

            return futureValue;
        }
        private static void BisectionMethod(decimal a, decimal p, decimal r, int t, int n, decimal epsilon, ref decimal lowerBound, ref decimal upperBound, ref decimal PMT, int maxIterations, decimal maxDifference, ref int iterations)
        {
            // Bisection method to approximate PMT
            while (Math.Abs(a - CalculateFutureValue(p, r, t, n, PMT)) > epsilon)
            {
                // Calculate the midpoint of the interval
                PMT = (lowerBound + upperBound) / 2;

                // Calculate the future value based on the current PMT
                decimal futureValue = CalculateFutureValue(p, r, t, n, PMT);

                // Adjust the interval based on the comparison between futureValue and the target value a
                if (futureValue > a)
                {
                    // futureValue is greater than the target, so update the upperBound
                    upperBound = PMT;
                }
                else
                {
                    // futureValue is less than or equal to the target, so update the lowerBound
                    lowerBound = PMT;
                }

                // Increment the number of iterations
                iterations++;

                // Exit the loop if the maximum number of iterations is reached or the interval is sufficiently small
                if (iterations >= maxIterations || Math.Abs(upperBound - lowerBound) < maxDifference)
                {
                    break;
                }
            }
        }
        private static void FillResults(_PredictionCalculatorVM vm, decimal monthlyContributions, decimal interestRate, ref decimal totalContribution, ref decimal totalInterest)
        {
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
        }
    }
}
