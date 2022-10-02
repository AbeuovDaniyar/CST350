using Microsoft.AspNetCore.Mvc;
using Milestone_CST350.Models;
using Milestone_CST350.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Controllers
{
    public class UserController : Controller
    {
        UserService securityService = new UserService();
        public IActionResult Index()
        {
            return View();
        }

        //login
        public IActionResult Authenticate(User user)
        {
            UserService userService = new UserService();
            var result = userService.FindByUsernameAndPassword(user.UserName, user.Password);

            if (result)
            {
                //redirect to game board
                //return RedirectToAction("Index", "Game");
                return RedirectToAction("Index", "Home");
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
                //return View("~/Views/GameBoard/Index.cshtml");
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                return View("Register", user);
            }
        }
    }
}
