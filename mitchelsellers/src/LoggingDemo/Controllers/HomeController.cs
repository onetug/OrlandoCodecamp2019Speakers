using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoggingDemo.Models;
using Microsoft.Extensions.Logging;

namespace LoggingDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

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
            _logger.LogWarning("{Username} visited privacy page at {Timestamp}", User.Identity?.Name, DateTime.UtcNow);
            //Calls ToString
            _logger.LogInformation("Request details {Request}", Request);
            //Attempts a dump
            _logger.LogInformation("Full User Identity Details {@Request}", User.Identity);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Bad error happened!");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
