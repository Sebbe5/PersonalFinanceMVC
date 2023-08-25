using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Compound;
using PersonalFinanceMVC.Views.Shared.Compound;

namespace PersonalFinanceMVC.Controllers
{

    // TODO: Add function to let the user put in how much they want to live on per month...
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
            // Create an instance of an that is fetched from the Cookie "LastCalculator" (If it exists)
            CalculateVM vm =  new CalculateVM { CalculatorUsed = Request.Cookies["LastCalculator"] };
            return View(vm);
        }

        [HttpGet("/PredictionCalculator")]
        public IActionResult PredictionCalculator()
        {
            // Retrieve the serialized ViewModel from the "_PredictionCalculatorVM" cookie.
            string serializedVm = Request.Cookies["_PredictionCalculatorVM"] as string;

            // Check if the serialized ViewModel is not null or empty.
            if (!string.IsNullOrEmpty(serializedVm))
            {
                // Deserialize the serialized ViewModel using JsonConvert from the JSON string.
                var vm = JsonConvert.DeserializeObject<_PredictionCalculatorVM>(serializedVm);

                // Call the compoundService to update the ViewModel.
                vm = compoundService.Update_PredictionCalculatorVM(vm);

                // Return a partial view named "_PredictionCalculator" with the updated ViewModel.
                return PartialView("Compound/_PredictionCalculator", vm);
            }
            else
            {
                // If the serialized ViewModel is null or empty, create a new instance of _PredictionCalculatorVM.
                _PredictionCalculatorVM vm = new _PredictionCalculatorVM();

                // Return a partial view named "_PredictionCalculator" with the new ViewModel.
                return PartialView("Compound/_PredictionCalculator", vm);
            }
        }

        [HttpPost("/PredictionCalculator")]
        public IActionResult PredictionCalculator(_PredictionCalculatorVM vm)
        {
            // TODO: Continue Commenting here
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
