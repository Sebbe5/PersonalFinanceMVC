using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Compound;
using PersonalFinanceMVC.Views.Shared.Compound;

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
            CalculateVM vm =  new CalculateVM { CalculatorUsed = Request.Cookies["LastCalculator"] };
            return View(vm);
        }

        [HttpGet("/PredictionCalculator")]
        public IActionResult PredictionCalculator()
        {
            string serializedVm = Request.Cookies["_PredictionCalculatorVM"] as string;

            if (!string.IsNullOrEmpty(serializedVm))
            {
                var vm = JsonConvert.DeserializeObject<_PredictionCalculatorVM>(serializedVm);
                vm = compoundService.Update_PredictionCalculatorVM(vm);
                return PartialView("Compound/_PredictionCalculator", vm);
            }
            else
            {
                _PredictionCalculatorVM vm = new _PredictionCalculatorVM();
                return PartialView("Compound/_PredictionCalculator", vm);
            }
        }

        [HttpPost("/PredictionCalculator")]
        public IActionResult PredictionCalculator(_PredictionCalculatorVM vm)
        {
            string serializedVm = JsonConvert.SerializeObject(vm);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1) // Set the cookie expiration date as desired
            };
            Response.Cookies.Append("_PredictionCalculatorVM", serializedVm, cookieOptions);
            Response.Cookies.Append("LastCalculator", "prediction", cookieOptions);
            return RedirectToAction(nameof(Calculate));
        }

        [HttpGet("/GoalCalculator")]
        public IActionResult GoalCalculator()
        {
            string serializedVm = Request.Cookies["_GoalCalculatorVM"] as string;

            if (!string.IsNullOrEmpty(serializedVm))
            {
                var vm = JsonConvert.DeserializeObject<_GoalCalculatorVM>(serializedVm);
                vm = compoundService.Update_GoalCalculatorVM(vm);

                return PartialView("Compound/_GoalCalculator", vm);
            }
            else
            {
                _GoalCalculatorVM vm = new _GoalCalculatorVM();
                return PartialView("Compound/_GoalCalculator", vm);
            }
        }

        [HttpPost("/GoalCalculator")]
        public IActionResult GoalCalculator(_GoalCalculatorVM vm)
        {
            string serializedVm = JsonConvert.SerializeObject(vm);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1) // Set the cookie expiration date as desired
            };
            Response.Cookies.Append("_GoalCalculatorVM", serializedVm, cookieOptions);
            Response.Cookies.Append("LastCalculator", "goal", cookieOptions);
            return RedirectToAction(nameof(Calculate));
        }
    }
}
