using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Compound;
using PersonalFinanceMVC.Views.Investment;

namespace PersonalFinanceMVC.Models
{

    // TODO: Add details view
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

        internal void AddInvestmentDB(CreateInvestmentVM vm)
        {
            Investment newInvestment = new Investment
            {
                Name = CheckIfNameExist(vm.Name),
                InitialValue = vm.InitialValue,
                Value = vm.InitialValue,
                RecurringDeposit = vm.MonthlyContribution,
                ExpectedAnnualInterest = (decimal)vm.AnnualInterest,
                ApplicationUserId = userId,
            };

            context.Investments.Add(newInvestment);

            context.SaveChanges();
        }

        private string CheckIfNameExist(string name)
        {
            // Get the names of all budgets
            HashSet<string> investmentNames = new HashSet<string>(context.Investments.Select(b => b.Name), StringComparer.OrdinalIgnoreCase);

            // Check if name already exist
            bool isExisting = investmentNames.Contains(name);

            int counter = 2;

            // Loop until the name does not exist
            while (isExisting)
            {
                // If the loop have passed the first iteration, remove the last char, other wise don't
                if (counter > 2)
                    name = name.Substring(0, name.Length - 1) + $"{counter}";
                else
                    name += $"{counter}";

                isExisting = investmentNames.Contains(name, StringComparer.OrdinalIgnoreCase);
                counter++;
            }

            return name;
        }

        internal InvestmentDetailsVM CreateInvestmentDetailsVM(int id)
        {
            var investment = context.Investments
                .Where(b => b.Id == id)
                .Select(i => new InvestmentDetailsVM
                {
                    Id = id,
                    Name = i.Name,
                    InitialValue = i.InitialValue,
                    RecurringDeposit = i.RecurringDeposit,
                    ExpectedAnnualInterest = i.ExpectedAnnualInterest,
                })
                .FirstOrDefault();

            double totalContribution = investment.InitialValue;
            double interest = 0;

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    totalContribution += investment.RecurringDeposit;
                    interest += totalContribution * (double)(investment.ExpectedAnnualInterest/1200);
                }

                investment.Contributions.Add(totalContribution);
                investment.Profits.Add(interest);
                investment.TotalAmounts.Add(totalContribution + interest);
            }
            return investment;
        }
    }
}
