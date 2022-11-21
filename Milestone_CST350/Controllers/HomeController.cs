using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Milestone_CST350.Models;
using Milestone_CST350.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        UserService securityService = new UserService();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //login
        public IActionResult Authenticate(User user)
        {
            UserService userService = new UserService();
            int result = userService.FindByUsernameAndPassword(user.UserName, user.Password);

            if (result != -1)
            {
                TempData["user"] = user.UserName;
                //redirect to game board
                return RedirectToAction("Index", "Game");
            }
            else
            {
                //login failed
                TempData["ErrorMessage"] = "Username or password is wrong";
                return View("Index");
            }
        }

        //register
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult RegisterUser(User user)
        {
            var result = securityService.RegisterUser(user);

            if (result)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                return View("Register", user);
            }
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
