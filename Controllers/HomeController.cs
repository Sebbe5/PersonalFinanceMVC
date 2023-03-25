﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Home;

namespace PersonalFinanceMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        AccountService accountService;
        public HomeController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("member")]
        public IActionResult Member()
        {
            return View();
        }

        [HttpGet("budget")]
        public IActionResult Budget()
        {
            return View(accountService.GetUserBudgets());
        }

        [HttpGet("createBudget")]
        public IActionResult CreateBudget()
        {
            return View();
        }

        [HttpPost("createBudget")]
        public IActionResult CreateBudget(CreateBudgetVM vm)
        {
            if (!ModelState.IsValid)
                return View();
            accountService.AddBudgetToUser(vm);
            return RedirectToAction(nameof(Budget));
        }

        [HttpGet("editBudget")]
        public IActionResult EditBudget(CreateBudgetVM vm, int id)
        {
            if (!ModelState.IsValid)
                return View();
            accountService.AddBudgetToUser(vm);
            return RedirectToAction(nameof(Budget));
        }


    }
}
