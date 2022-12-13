using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Milestone_CST350.Models;
using Milestone_CST350.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Milestone_CST350.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        const string SessionUserId = "_UserId";
        const string SessionUserName = "_Name";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetInt32(SessionUserId, -1);
            HttpContext.Session.SetString(SessionUserName, "");
            return View();
        }

        public IActionResult Authenticate(User user)
        {
            UserService userService = new UserService();
            int result = userService.FindByUsernameAndPassword(user.UserName, user.Password);

            if (result != -1)
            {
                //redirect to game board set session variables
                HttpContext.Session.SetInt32(SessionUserId, result);
                HttpContext.Session.SetString(SessionUserName, user.UserName);
                return RedirectToAction("Index", "Game");
            }
            else
            {
                //login failed
                return View("LoginFailure", user.UserName);
            }
        }

        public IActionResult Register() 
        {
            return View();
        }
        public IActionResult RegisterUser(User user)
        {
            UserService securityService = new UserService();
            int result = securityService.RegisterUser(user);

            if (result != -1)
            {
                return View("Index");
            }
            else
            {
                return View("Register");
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
