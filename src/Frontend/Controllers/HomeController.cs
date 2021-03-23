using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Frontend.Models;
using Ingredients.Protos;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IngredientsService.IngredientsServiceClient _client;
        private readonly ILogger<HomeController> _log;

        public HomeController(IngredientsService.IngredientsServiceClient client, ILogger<HomeController> log)
        {
            _client = client;
            _log = log;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            _log.LogInformation("Index");
            var toppingsResponse = await _client.GetToppingsAsync(new GetToppingsRequest());
            var toppings = toppingsResponse.Toppings
                .Select(t => new ToppingViewModel(t.Topping.Id, t.Topping.Name, Convert.ToDecimal(t.Topping.Price)))
                .ToList();
            
            var crustsResponse = await _client.GetCrustAsync(new GetCrustRequest());
            var crusts = crustsResponse.Crusts
                .Select(t => new CrustViewModel(t.Crust.Id, t.Crust.Name, t.Crust.Size, Convert.ToDecimal(t.Crust.Price)))
                .ToList();
            
            var viewModel = new HomeViewModel(toppings, crusts);
            return View(viewModel);
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
    }
}
