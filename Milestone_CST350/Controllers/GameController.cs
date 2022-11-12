using Microsoft.AspNetCore.Mvc;
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

namespace Milestone_CST350.Controllers
{
    public class GameController : Controller
    {
        GameService service = new GameService();
        public static Board gameBoard { get; set; }
        public IActionResult Index()
        {
            gameBoard = newGame();
            return View(gameBoard);
        }

        public IActionResult ButtonClick(string rowcol)
        {
            string[] separate = rowcol.Split('+');
            int row = Convert.ToInt32(separate[0]);
            int col = Convert.ToInt32(separate[1]);

            gameBoard.floodFill(row, col);

            if (service.HasGameLost(gameBoard, row, col))
            {
                PlayerStats player = service.setPlayer("danny", "medium");
                return View("GameOver", player);
            }
            if (service.HasGameWon(gameBoard))
            {
                PlayerStats player = service.setPlayer("danny", "medium");
                return View("GameWon", player);
            }

            return View("Index", gameBoard);
        }

        public IActionResult ShowOneButton(string rowcol)
        {
            string[] separate = rowcol.Split('+');
            int row = Convert.ToInt32(separate[0]);
            int col = Convert.ToInt32(separate[1]);

            gameBoard.floodFill(row, col);

            string view = RenderRazorViewToString(this, "ShowOneButton", gameBoard);
            string message = "<h1>the game is in progress...</h1>";
            string playerStatsString = "<hr /> <dl class='row'> <dt class='col - sm - 2'> Player: </dt> <dd class='col - sm - 10'> " + "" + " </dd> <dt class='col - sm - 2'> Difficulty: </dt> <dd class='col - sm - 10'> " + "" + " </dd> <dt class='col - sm - 2'> Score: </dt> <dd class='col - sm - 10'> " + "" + " </dd> </dl>";
            var package = new { part1 = view, part2 = message, part3 = playerStatsString };

            if (service.HasGameLost(gameBoard, row, col))
            {
                PlayerStats player = service.setPlayer("danny", "medium");
                playerStatsString = "<hr /> <dl class='row'> <dt class='col - sm - 2'> Player: </dt> <dd class='col - sm - 10'> " + player.playerName + " </dd> <dt class='col - sm - 2'> Difficulty: </dt> <dd class='col - sm - 10'> " + player.difficulty + " </dd> <dt class='col - sm - 2'> Score: </dt> <dd class='col - sm - 10'> " + player.score + " </dd> </dl>";
                gameBoard.revealAll();
                view = RenderRazorViewToString(this, "ShowOneButton", gameBoard);
                message = "<h1>You Lost!</h1>";
                package = new { part1 = view, part2 = message, part3 = playerStatsString };
                return Json(package);
                //return PartialView("GameOver", player);
            }
            if (service.HasGameWon(gameBoard))
            {
                PlayerStats player = service.setPlayer("danny", "medium");
                playerStatsString = "<hr /> <dl class='row'> <dt class='col - sm - 2'> Player: </dt> <dd class='col - sm - 10'> " + player.playerName + " </dd> <dt class='col - sm - 2'> Difficulty: </dt> <dd class='col - sm - 10'> " + player.difficulty + " </dd> <dt class='col - sm - 2'> Score: </dt> <dd class='col - sm - 10'> " + player.score + " </dd> </dl>";

                gameBoard.revealAll();

                view = RenderRazorViewToString(this, "ShowOneButton", gameBoard);
                message = "<h1>You Won!</h1>";
                package = new { part1 = view, part2 = message, part3 = playerStatsString };
                return Json(package);
                //return PartialView("GameWon", player);
            }
            return Json(package);
            //return PartialView("ShowOneButton", gameBoard);
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
            //return PartialView("ShowOneButton", gameBoard);
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
