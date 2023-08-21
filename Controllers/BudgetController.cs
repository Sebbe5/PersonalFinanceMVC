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
            // TODO: Ability to reset the list of paid items with a button.


            // Create an instance of a BudgetDetailsVM
            BudgetDetailsVM vm = budgetService.CreateBudgetDetailsVM(id);
            return View(vm);
        }

        [HttpGet("createBudget")]
        public IActionResult CreateBudget()
        {
            // Create an instance of a CreateBudgetsVM
            CreateBudgetVM vm = new CreateBudgetVM();
            return View(vm);
        }

        [HttpPost("createBudget")]
        public IActionResult CreateBudget(CreateBudgetVM vm)
        {
            // If the model state is invalid, go back to the view
            if (!ModelState.IsValid)
                return View(vm);

            // Add budget to DB
            budgetService.AddBudgetToDB(vm);

            return RedirectToAction(nameof(Budgets));
        }

        [HttpGet("editBudget")]
        public IActionResult EditBudget(int id)
        {
            // Set the cookie of EditedBudgetId to the id from the input
            Response.Cookies.Append("EditedBudgetId", id.ToString());
            
            // Create an instance of an EditBudgetVM
            EditBudgetVM vm = budgetService.CreateEditBudgetVM(id);
            return View(vm);
        }

        [HttpPost("editBudget")]
        public IActionResult EditBudget(EditBudgetVM vm)
        {
            // If the model state is invalid, go back to the view
            if (!ModelState.IsValid)
                return View(vm);

            // Parsing the integer value stored in the "EditedBudgetId" cookie from the HTTP request.
            int id = int.Parse(Request.Cookies["EditedBudgetId"]);

            // Calling the "EditBudget" method of the budgetService, passing in the "vm" (ViewModel) and the parsed "id".
            budgetService.EditBudget(vm, id);

            // Redirecting to the "BudgetDetails" action, passing the "id" as a route parameter.
            return RedirectToAction(nameof(BudgetDetails), new { id });
        }

        [HttpPost("removeBudget")]
        public IActionResult RemoveBudget(int id)
        {
            // Call the RemoveBudget method from the budgetService to delete a budget entry based on the specified ID.
            budgetService.RemoveBudget(id);

            // Redirect to the "Budgets" action method to display the updated list of budgets after removal
            return RedirectToAction(nameof(Budgets));

        }

        [HttpPost("updateIsPaidStatus")]
        public IActionResult UpdateIsPaidStatus(int id, bool isPaid)
        {
            try
            {
              
                // Update IsPaid status
                budgetService.UpdateIsPaidStatus(id, isPaid);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // If an exception is caught during the execution of the 'try' block,
                // this 'catch' block is executed to handle the exception.

                // A JSON response is created to indicate failure, and the error message
                // from the caught exception is included in the response.
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
