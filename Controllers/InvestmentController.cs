using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Investment;

namespace PersonalFinanceMVC.Controllers
{
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
    }
}
