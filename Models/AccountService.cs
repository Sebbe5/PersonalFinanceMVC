using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Home;
using PersonalFinanceMVC.Views.Login;
using static PersonalFinanceMVC.Views.Home.BudgetVM;

namespace PersonalFinanceMVC.Models
{
    public class AccountService
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        readonly string userId;

        private readonly ApplicationContext context;
        
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor accessor,
            ApplicationContext context)

        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
            userId = userManager.GetUserId(accessor.HttpContext.User);
        }

        public async Task<string> TryRegister(RegisterVM viewModel)
        {
            var user = new ApplicationUser
            {
                UserName = viewModel.Username,
            };

            IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);

            bool wasUserCreated = result.Succeeded;
            return wasUserCreated ? null : "Failed to create user";
        }

        public async Task<bool> TryLogin(LoginVM vm)
        {
            SignInResult result = await signInManager.PasswordSignInAsync(
                vm.Username,
                vm.Password,
                isPersistent: false,
                lockoutOnFailure: false
                );

            return result.Succeeded;
        }

        internal async void LogOut()
        {
            await signInManager.SignOutAsync();
        }

        public BudgetVM GetUserBudgets()
        {
            var q = context.Budgets
                .Where(b => b.ApplicationUserId == userId)
                .ToArray();

            BudgetVM vm = new BudgetVM();
            vm.budgets = new BudgetItemVM[q.Length];
            for (int i = 0; i < q.Length; i++)
            {
                vm.budgets[i] = new BudgetItemVM();
                vm.budgets[i].Name = q[i].Name;
                vm.budgets[i].Id = q[i].Id;
            }
            return vm;
        }

        public int AddBudgetToUser(CreateBudgetVM vm)
        {
            Budget budget = new Budget();
            budget.ApplicationUserId = userId;
            budget.Name = vm.Name;
            context.Budgets.Add(budget);
            context.SaveChanges(); //The id of the budget is set here and can be returned on the next row
            return budget.Id;
        }
    }
}
