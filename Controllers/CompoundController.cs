using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Compound;

namespace PersonalFinanceMVC.Controllers
{
    // TODO: Add a calculator for how much you need to deposit each month in order to reach a certain goal
    // TODO: Add the choice to save as investment
    public class CompoundController : Controller
    {
        CompoundService compoundService;
        public CompoundController(CompoundService dataService)
        {
            this.compoundService = dataService;
        }

        [HttpGet("/calculate")]
        public IActionResult Calculate()
        {
            CalculateVM vm = new CalculateVM();
            return View(vm);
        }

        [HttpPost("/calculate")]
        public IActionResult Calculate(CalculateVM vm)
        {
            vm = compoundService.UpdateCalculateVM(vm);
            return View(vm);
        }
    }
}
