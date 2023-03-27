using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Home;

namespace PersonalFinanceMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        AccountService accountService;
        DataService dataService;
        public HomeController(AccountService accountService, DataService dataService)
        {
            this.accountService = accountService;
            this.dataService = dataService;
        }

        [HttpGet("member")]
        public IActionResult Member()
        {
            return View();
        }

        [HttpGet("budget")]
        public IActionResult Budget()
        {
            return View(accountService.GetUserBudgets());
        }

        [HttpGet("createBudget")]
        public IActionResult CreateBudget()
        {
            return View();
        }

        [HttpPost("createBudget")]
        public IActionResult CreateBudget(CreateBudgetVM vm)
        {
            if (!ModelState.IsValid)
                return View();
            accountService.AddBudgetToUser(vm);
            return RedirectToAction(nameof(Budget));
        }

        [HttpGet("editBudget")]
        public IActionResult EditBudget(int id)
        {
            Response.Cookies.Append("BudgetId", id.ToString());
            return View(dataService.GetBudgetNameAndExpenses(id));
        }

        [HttpGet("deleteBudget")]
        public IActionResult DeleteBudget(int id)
        {
            dataService.DeleteBudget(id);
            return RedirectToAction(nameof(Budget));
        }

        [HttpGet("deleteExpense")]
        public IActionResult DeleteExpense(int id)
        {
            dataService.DeleteExpense(id);
            return RedirectToAction(nameof(EditBudget), new { id = int.Parse(Request.Cookies["BudgetId"]) });
        }

        // TODO: Add Expense
    }
}
