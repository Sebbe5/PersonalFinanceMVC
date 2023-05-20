using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Investment;

namespace PersonalFinanceMVC.Controllers
{

    // TODO: Add a calculator for how much you need to deposit each month in order to reach a certain goal
    [Authorize]
    public class InvestmentController : Controller
    {
        InvestmentService investmentService;
        public InvestmentController(InvestmentService dataService)
        {
            this.investmentService = dataService;
        }

        [HttpGet("/investments")]
        public IActionResult Investments()
        {
            InvestmentsVM vm = investmentService.CreateInvestmentsVM();
            return View(vm);
        }

        [HttpGet("/createInvestments")]
        public IActionResult CreateInvestment()
        {
            CreateInvestmentVM vm = new CreateInvestmentVM() { Name = ""};
            return View(vm);
        }

        [HttpPost("/createInvestments")]
        public IActionResult CreateInvestment(CreateInvestmentVM vm)
        {
            if(!ModelState.IsValid)
                return View(vm);

            investmentService.AddInvestmentDB(vm);
            
            return RedirectToAction(nameof(Investments));
        }

        [HttpGet("/editInvestments")]
        public IActionResult EditInvestment(int id)
        {
            EditInvestmentVM vm = investmentService.CreateEditInvestmentVM(id);
            Response.Cookies.Append("EditedInvestmentId", id.ToString());
            return View(vm);
        }

        [HttpPost("/editInvestments")]
        public IActionResult EditInvestment(EditInvestmentVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            int id = int.Parse(Request.Cookies["EditedInvestmentId"]);
            investmentService.EditInvestment(vm, id);
            return RedirectToAction(nameof(InvestmentDetails), new { id });
        }

        [HttpGet("/investmentDetails")]
        public IActionResult InvestmentDetails(int id)
        {
            var vm = investmentService.CreateInvestmentDetailsVM(id);
            return View(vm);
        }


        [HttpPost("/removeInvestment")]
        public IActionResult RemoveInvestment(int id)
        {
            investmentService.RemoveInvestment(id);
            return RedirectToAction(nameof(Investments));
        }
    }
}
