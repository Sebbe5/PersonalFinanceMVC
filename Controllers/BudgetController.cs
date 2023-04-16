using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Budget;

namespace PersonalFinanceMVC.Controllers
{
    public class BudgetController : Controller
    {
        DataService dataService;
        public BudgetController(DataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet("budgets")]
        public IActionResult Budgets()
        {
            BudgetsVM vm = dataService.CreateBudgetsVM();
            return View(vm);
        }

        [HttpGet("budgetDetails")]
        public IActionResult BudgetDetails(int id)
        {
            BudgetDetailsVM vm = dataService.CreateBudgetDetailsVM(id);
            return View(vm);
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

            dataService.AddBudgetToDB(vm);

            return RedirectToAction(nameof(Budgets));
        }

        [HttpGet("editBudget")]
        public IActionResult EditBudget(int id)
        {
            Response.Cookies.Append("EditedBudgetId", id.ToString());
            EditBudgetVM vm = dataService.CreateEditBudgetVM(id);
            return View(vm);
        }

        [HttpPost("editBudget")]
        public IActionResult EditBudget(EditBudgetVM vm)
        {
            if (!ModelState.IsValid)
                return View();

            int id = int.Parse(Request.Cookies["EditedBudgetId"]);

            dataService.EditBudget(vm, id);

            return RedirectToAction(nameof(BudgetDetails), new {id});
        }

        // TODO: Implement delete action to budgets
    }
}
