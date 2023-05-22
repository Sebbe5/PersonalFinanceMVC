using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            return View();
        }

        [HttpGet("/PredictionCalculator")]
        public IActionResult PredictionCalculator()
        {
            string serializedVm = Request.Cookies["CalculateVM"] as string;

            if (!string.IsNullOrEmpty(serializedVm))
            {
                var vm = JsonConvert.DeserializeObject<CalculateVM>(serializedVm);
                vm = compoundService.UpdateCalculateVM(vm);
                return PartialView("Compound/_PredictionCalculator", vm);
            }
            else
            {
                CalculateVM vm = new CalculateVM();
                return PartialView("Compound/_PredictionCalculator", vm);
            }
        }

        [HttpPost("/PredictionCalculator")]
        public IActionResult PredictionCalculator(CalculateVM vm)
        {
            string serializedVm = JsonConvert.SerializeObject(vm);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1) // Set the cookie expiration date as desired
            };
            Response.Cookies.Append("CalculateVM", serializedVm, cookieOptions);
            return RedirectToAction(nameof(Calculate));
        }

        [HttpGet("/GoalCalculator")]
        public IActionResult GoalCalculator()
        {
            string serializedVm = Request.Cookies["CalculateVM"] as string;

            if (!string.IsNullOrEmpty(serializedVm))
            {
                var vm = JsonConvert.DeserializeObject<CalculateVM>(serializedVm);
                return PartialView("Compound/_GoalCalculator", vm);
            }
            else
            {
                CalculateVM vm = new CalculateVM();
                return PartialView("Compound/_GoalCalculator", vm);
            }
        }

        [HttpPost("/GoalCalculator")]
        public IActionResult GoalCalculator(CalculateVM vm)
        {
            vm = compoundService.UpdateCalculateVM(vm);
            string serializedVm = JsonConvert.SerializeObject(vm);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1) // Set the cookie expiration date as desired
            };
            Response.Cookies.Append("CalculateVM", serializedVm, cookieOptions);
            return RedirectToAction(nameof(Calculate));
        }
    }
}
