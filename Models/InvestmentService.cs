using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;
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
            // Fetch user investments and convert them to an InvestmentItemVM
            var investments = GetUserInvestments()
                .Select(i => new InvestmentsVM.InvestmentItemVM
                {
                    Id = i.Id,
                    Name = i.Name,
                    Value = i.Value,
                })
                .ToList();

            // Return view model
            return new InvestmentsVM
            {
                Investments = investments
            };
        }
        internal void AddInvestmentDB(CreateInvestmentVM vm)
        {
            // Create new investment
            Investment newInvestment = new Investment
            {
                Name = CheckIfNameExist(vm.Name),
                InitialValue = vm.InitialValue,
                Value = vm.InitialValue,
                RecurringDeposit = vm.MonthlyContribution,
                ExpectedAnnualInterest = (decimal)vm.AnnualInterest,
                ExpectedYearsInvested = vm.ExpectedYearsInvested,
                ApplicationUserId = userId,
            };

            // Add investment to DB list of investments
            context.Investments.Add(newInvestment);

            // Save changes to DB
            context.SaveChanges();
        }
        internal InvestmentDetailsVM CreateInvestmentDetailsVM(int id)
        {
            // Create an instance of an investment detail
            var investment = GetUserInvestments()
                .Select(i => new InvestmentDetailsVM
                {
                    Id = id,
                    Name = i.Name,
                    InitialValue = i.InitialValue,
                    RecurringDeposit = i.RecurringDeposit,
                    ExpectedAnnualInterest = i.ExpectedAnnualInterest,
                    ExpectedYearsInvested = i.ExpectedYearsInvested,
                })
                .FirstOrDefault();

            // Initiate total contribution to the initial value of the investment
            double totalContribution = investment.InitialValue;

            // Initiate total interest to 0
            double totalInterest = 0;

            // Create Investment prediction list
            CreateInvestmentPredictionList(investment, ref totalContribution, ref totalInterest);

            // Return investment view model
            return investment;
        }
        internal EditInvestmentVM CreateEditInvestmentVM(int id)
        {
            // Create and return an instance of an editInvestment view model
            return GetUserInvestments()
                .Select(i => new EditInvestmentVM
                {
                    Name = i.Name,
                    InitialValue = i.InitialValue,
                    MonthlyContribution = i.RecurringDeposit,
                    AnnualInterest = (double)i.ExpectedAnnualInterest,
                    ExpectedYearsInvested = i.ExpectedYearsInvested
                })
                .FirstOrDefault();
        }
        internal void EditInvestment(EditInvestmentVM vm, int id)
        {
           // Fetch the first investment to edit from the DB
            var investmentToEdit = context.Investments.SingleOrDefault(i => i.Id == id);

            // Set the properties of the investment
            investmentToEdit.Name = investmentToEdit.Name == vm.Name ? investmentToEdit.Name : CheckIfNameExist(vm.Name);
            investmentToEdit.InitialValue = vm.InitialValue;
            investmentToEdit.RecurringDeposit = vm.MonthlyContribution;
            investmentToEdit.ExpectedAnnualInterest = (decimal)vm.AnnualInterest;
            investmentToEdit.ExpectedYearsInvested = vm.ExpectedYearsInvested;

            // Save the changes to the DB
            context.SaveChanges();
        }
        internal void RemoveInvestment(int id)
        {
            // Remove the first investment from the DB with the matching id
            context.Investments.Remove(context.Investments.SingleOrDefault(i => i.Id == id));

            // Save changes to DB
            context.SaveChanges();
        }
        private IQueryable<Investment> GetUserInvestments() => context.Investments.Where(i => i.ApplicationUserId == userId);
        private static void CreateInvestmentPredictionList(InvestmentDetailsVM investment, ref double totalContribution, ref double totalInterest)
        {
            // Loop thorugh list to fill
            for (int i = 0; i < investment.ExpectedYearsInvested; i++)
            {
                // Set the current year of the loop
                investment.YearLabels.Add("Year " + (i + 1).ToString());

                // Continue commenting here
                totalInterest += (totalInterest + totalContribution) * (double)(investment.ExpectedAnnualInterest / 100);
                investment.Profits.Add(totalInterest);

                totalContribution += investment.RecurringDeposit * 12;
                investment.Contributions.Add(totalContribution);

                investment.TotalAmounts.Add(totalContribution + totalInterest);
            }
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
    }
}
