using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Compound;

namespace PersonalFinanceMVC.Controllers
{

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
