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
            return View();
        }
    }
}
