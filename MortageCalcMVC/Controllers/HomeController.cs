using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MortgageCalcMVC.Models;
using MortgageCalcMVC.Helpers;

namespace MortgageCalcMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpGet]
    public IActionResult App()
    {
        Loan loan = new()
        {
            Payment = 0.0m,
            TotalInterest = 0.0m,
            TotalCost = 0.0m,
            Rate = 15.0m,
            Amount = 15000m,
            Term = 60
        };

        return View(loan);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult App(Loan loan)
    {
        //Calculate the loan and get the payment
        Loan newLoan = LoanHelper.GetPayments(loan);

        return View(newLoan);
    }
}