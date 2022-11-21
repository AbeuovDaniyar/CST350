﻿using Microsoft.AspNetCore.Mvc;
using Milestone_CST350.Models;
using Milestone_CST350.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Milestone_CST350.Controllers
{
    public class GameController : Controller
    {
        GameService service = new GameService();
        public static Board gameBoard { get; set; }

        public static PlayerStats player;

        public IActionResult Index()
        {
            gameBoard = newGame();
            string userName = (string)TempData["user"];

            player = service.setPlayer(userName, "medium");
            return View(gameBoard);
        }

        public IActionResult ButtonClick(string rowcol)
        {
            string[] separate = rowcol.Split('+');
            int row = Convert.ToInt32(separate[0]);
            int col = Convert.ToInt32(separate[1]);

            if (gameBoard.grid[row, col].buttonState != 11) 
            {
                gameBoard.floodFill(row, col);

                if (service.HasGameLost(gameBoard, row, col))
                {
                    return View("GameOver", player);
                }
                if (service.HasGameWon(gameBoard))
                {
                    return View("GameWon", player);
                }
            }

            return View("Index", gameBoard);
        }

        public IActionResult ShowOneButton(string rowcol)
        {
            string[] separate = rowcol.Split('+');
            int row = Convert.ToInt32(separate[0]);
            int col = Convert.ToInt32(separate[1]);


            if (gameBoard.grid[row, col].buttonState != 11) 
            {
                gameBoard.floodFill(row, col);
            }

            string view = RenderRazorViewToString(this, "ShowOneButton", gameBoard);
            string message = "<h1>the game is in progress...</h1>";
            string playerStatsString = "<hr /> <dl class='row'> <dt class='col - sm - 2'> Player: </dt> <dd class='col - sm - 10'> " + player.playerName + " </dd> <dt class='col - sm - 2'> Difficulty: </dt> <dd class='col - sm - 10'> " + player.difficulty + " </dd> <dt class='col - sm - 2'> Score: </dt> <dd class='col - sm - 10'> " + 0 + " </dd> </dl>";
            string gameOver = "0";
            var package = new { part1 = view, part2 = message, part3 = playerStatsString, part4 = gameOver };

            

            if (service.HasGameLost(gameBoard, row, col) && gameBoard.grid[row, col].buttonState != 11)
            {
                playerStatsString = "<hr /> <dl class='row'> <dt class='col - sm - 2'> Player: </dt> <dd class='col - sm - 10'> " + player.playerName + " </dd> <dt class='col - sm - 2'> Difficulty: </dt> <dd class='col - sm - 10'> " + player.difficulty + " </dd> <dt class='col - sm - 2'> Score: </dt> <dd class='col - sm - 10'> " + player.score + " </dd> </dl>";
                gameBoard.revealAll();
                view = RenderRazorViewToString(this, "ShowOneButton", gameBoard);
                message = "<h1>You Lost!</h1>";
                gameOver = "1";
                package = new { part1 = view, part2 = message, part3 = playerStatsString, part4 = gameOver };
                return Json(package);
            }
            if (service.HasGameWon(gameBoard))
            {
                playerStatsString = "<hr /> <dl class='row'> <dt class='col - sm - 2'> Player: </dt> <dd class='col - sm - 10'> " + player.playerName + " </dd> <dt class='col - sm - 2'> Difficulty: </dt> <dd class='col - sm - 10'> " + player.difficulty + " </dd> <dt class='col - sm - 2'> Score: </dt> <dd class='col - sm - 10'> " + player.score + " </dd> </dl>";
                gameBoard.revealAll();
                view = RenderRazorViewToString(this, "ShowOneButton", gameBoard);
                message = "<h1>You Won!</h1>";
                gameOver = "1";
                package = new { part1 = view, part2 = message, part3 = playerStatsString, part4 = gameOver };
                return Json(package);
            }
            return Json(package);
        }

        public IActionResult RightClick(string rowcol)
        {
            string[] separate = rowcol.Split('+');
            int row = Convert.ToInt32(separate[0]);
            int col = Convert.ToInt32(separate[1]);

            if (gameBoard.grid[row, col].buttonState == 10)
            {
                gameBoard.grid[row, col].buttonState = 11;
            }
            else if (gameBoard.grid[row, col].buttonState == 11)
            {
                gameBoard.grid[row, col].buttonState = 10;
            }

            string view = RenderRazorViewToString(this, "ShowOneButton", gameBoard);
            string message = "<h1>the game is in progress...</h1>";
            var package = new { part1 = view, part2 = message };

            return Json(package);
        }

        public IActionResult restart()
        {
            gameBoard = newGame();
            return View("Index", gameBoard);
        }

        public Board newGame()
        {
            Board newBoard = new Board(10, "medium");
            newBoard.setupLiveNeighbors();
            return newBoard;
        }

        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine =
                    controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as
                        ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
