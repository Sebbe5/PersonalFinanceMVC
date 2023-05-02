using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Investment;

namespace PersonalFinanceMVC.Controllers
{
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
            return View();
        }

        [HttpGet("/investmentDetails")]
        public IActionResult InvestmentDetails(int id)
        {
            var vm = investmentService.CreateInvestmentDetailsVM(id);
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
    }
}
