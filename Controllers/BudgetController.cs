using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Budget;

namespace PersonalFinanceMVC.Controllers
{
    [Authorize]
    public class BudgetController : Controller
    {   
        // Create an instance of a budget service
        BudgetService budgetService;
        public BudgetController(BudgetService dataService)
        {
            this.budgetService = dataService;
        }

        [HttpGet("budgets")]
        public IActionResult Budgets()
        {
            // Create an instance of a BudgetsVM
            BudgetsVM vm = budgetService.CreateBudgetsVM();
            return View(vm);
        }

        [HttpGet("budgetDetails")]
        public IActionResult BudgetDetails(int id)
        {
            // Create an instance of a BudgetDetailsVM
            BudgetDetailsVM vm = budgetService.CreateBudgetDetailsVM(id);
            return View(vm);
        }

        [HttpGet("createBudget")]
        public IActionResult CreateBudget()
        {
            // TODO: Continue commenting here
            CreateBudgetVM vm = new CreateBudgetVM();
            return View(vm);
        }

        [HttpPost("createBudget")]
        public IActionResult CreateBudget(CreateBudgetVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            budgetService.AddBudgetToDB(vm);

            return RedirectToAction(nameof(Budgets));
        }

        [HttpGet("editBudget")]
        public IActionResult EditBudget(int id)
        {
            Response.Cookies.Append("EditedBudgetId", id.ToString());
            EditBudgetVM vm = budgetService.CreateEditBudgetVM(id);
            return View(vm);
        }

        [HttpPost("editBudget")]
        public IActionResult EditBudget(EditBudgetVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            int id = int.Parse(Request.Cookies["EditedBudgetId"]);

            budgetService.EditBudget(vm, id);

            return RedirectToAction(nameof(BudgetDetails), new {id});
        }

        [HttpPost("removeBudget")]
        public IActionResult RemoveBudget(int id)
        {
            budgetService.RemoveBudget(id);

            return RedirectToAction(nameof(Budgets));
        }
    }
}
