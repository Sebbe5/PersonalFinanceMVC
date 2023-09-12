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
            // Serialize the 'vm' object using the JsonConvert class from the JSON.NET library.
            string serializedVm = JsonConvert.SerializeObject(vm);

            // Create a new instance of the CookieOptions class to configure cookie properties.
            var cookieOptions = new CookieOptions
            {
                // Set the expiration date for the cookie to be 1 hour from the current date and time.
                Expires = DateTime.Now.AddHours(1)
            };

            // Append a cookie named "_PredictionCalculatorVM" with the serialized VM data to the response.
            Response.Cookies.Append("_PredictionCalculatorVM", serializedVm, cookieOptions);

            // Append a cookie named "LastCalculator" with the value "prediction" to the response.
            Response.Cookies.Append("LastCalculator", "prediction", cookieOptions);

            // Redirect the user to the "Calculate" action method.
            // This typically means sending an HTTP 302 redirect response to the client's browser
            // to navigate to the action method named "Calculate".
            return RedirectToAction(nameof(Calculate));

        }

        [HttpGet("/GoalCalculator")]
        public IActionResult GoalCalculator()
        {
            // This line retrieves a serialized string named "_GoalCalculatorVM" from the HTTP cookies of the request.
            string serializedVm = Request.Cookies["_GoalCalculatorVM"] as string;

            // Check if the retrieved serialized string is not null or empty.
            if (!string.IsNullOrEmpty(serializedVm))
            {
                // Deserialize the serialized string using JsonConvert into an instance of the _GoalCalculatorVM class.
                var vm = JsonConvert.DeserializeObject<_GoalCalculatorVM>(serializedVm);

                // Call the compoundService's method to update the deserialized object.
                vm = compoundService.Update_GoalCalculatorVM(vm);

                // Return a partial view named "_GoalCalculator" along with the updated deserialized object.
                return PartialView("Compound/_GoalCalculator", vm);
            }
            else
            {
                // If the serialized string is null or empty, create a new instance of the _GoalCalculatorVM class.
                _GoalCalculatorVM vm = new _GoalCalculatorVM();

                // Return a partial view named "_GoalCalculator" along with the newly created object.
                return PartialView("Compound/_GoalCalculator", vm);
            }

        }

        [HttpPost("/GoalCalculator")]
        public IActionResult GoalCalculator(_GoalCalculatorVM vm)
        {
            // This line sets a serialized string named "_GoalCalculatorVM" from the HTTP cookies of the request.
            string serializedVm = JsonConvert.SerializeObject(vm);

            // Set a new cookie option
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1) // Set the cookie expiration date as desired
            };

            // Append a cookie named "_GoalCalculatorVM" with the serialized VM data to the response.
            Response.Cookies.Append("_GoalCalculatorVM", serializedVm, cookieOptions);

            // Append a cookie named "LastCalculator" with "goal" to the response.
            Response.Cookies.Append("LastCalculator", "goal", cookieOptions);
            return RedirectToAction(nameof(Calculate));
        }
    }
}
