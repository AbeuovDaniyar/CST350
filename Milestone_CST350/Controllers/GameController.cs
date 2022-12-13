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
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Milestone_CST350.Controllers
{
    public class GameController : Controller
    {
        GameService service = new GameService();
        public static Board gameBoard { get; set; }

        const string SessionGameDifficulty = "_GameDifficulty";

        public static string difficulty { get; set; }

        public static PlayerStats player;

        public IActionResult Index(Board game)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString(SessionGameDifficulty)))
            {
                HttpContext.Session.SetString(SessionGameDifficulty, "medium");
                difficulty = "medium";
            }
            else
            {
                difficulty = HttpContext.Session.GetString(SessionGameDifficulty);
            }

            if (gameBoard == null)
            {
                gameBoard = newGame();
            }
            else 
            {
                gameBoard = game;
            }           

            string userName = HttpContext.Session.GetString("_Name");
            player = service.setPlayer(userName, difficulty);
            return View(gameBoard);
        }

        public IActionResult ChangeDifficulty(string difficultyLvlv) 
        {
            HttpContext.Session.SetString(SessionGameDifficulty, difficultyLvlv);
            gameBoard = newGame();
            player.difficulty = difficultyLvlv;
            difficulty = difficultyLvlv;
            return View("Index", gameBoard);
        }

        public IActionResult SaveGame()
        {
            //if currently playing a saved game then we update
            if (HttpContext.Session.GetInt32("_GameId") > 0)
            {
                //set date and time
                gameBoard.DateTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
                gameBoard.GameData = Newtonsoft.Json.JsonConvert.SerializeObject(gameBoard);
                gameBoard.UserId = (int)HttpContext.Session.GetInt32("_UserId");

                if(service.UpdateGame((int)HttpContext.Session.GetInt32("_GameId"), gameBoard))
                    return View("Index", gameBoard);
            }
            else 
            {
                //set date and time
                gameBoard.DateTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
                gameBoard.GameData = Newtonsoft.Json.JsonConvert.SerializeObject(gameBoard);
                gameBoard.UserId = (int)HttpContext.Session.GetInt32("_UserId");

                //save game to db an return view with game id (SaveGame method will return an integer of the last inserted game id)
                if (service.SaveGame(gameBoard.UserId, gameBoard) > -1)
                    return View("Index", gameBoard);
            }
            return View("Index", gameBoard);
        }

        public IActionResult LoadGame(int gameId)
        {
            HttpContext.Session.SetInt32("_GameId", gameId);
            gameBoard = service.LoadGame(service.FindGameById(gameId));
            return View("Index", gameBoard);
        }

        public bool UpdateGame(Board gameBoard)
        {
            return service.UpdateGame((int)HttpContext.Session.GetInt32("_GameId"), gameBoard);
        }

        public IActionResult DeleteGame(int gameId) 
        {
            if (service.DeleteGame(gameId))
            {
                gameBoard = newGame();
                return View("Index", gameBoard);
            }
            else 
            {
                return RedirectToAction("ShowGames");
            }
        }

        public IActionResult ShowGames()
        {
            List<Board> games = service.GetAllUserGames((int)HttpContext.Session.GetInt32("_UserId"));
            return View("ShowGames", games);
        }

        /*
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
                    gameBoard.revealAll();
                    return View("GameOver", player);
                }
                if (service.HasGameWon(gameBoard))
                {
                    gameBoard.revealAll();
                    return View("GameWon", player);
                }
            }

            return View("Index", gameBoard);
        }
        */
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

            

            if (gameBoard.grid[row, col].live && gameBoard.grid[row, col].buttonState != 11)
            {
                gameBoard.revealAll();

                playerStatsString = "<hr /> <dl class='row'> <dt class='col - sm - 2'> Player: </dt> <dd class='col - sm - 10'> " + player.playerName + " </dd> <dt class='col - sm - 2'> Difficulty: </dt> <dd class='col - sm - 10'> " + player.difficulty + " </dd> <dt class='col - sm - 2'> Score: </dt> <dd class='col - sm - 10'> " + player.score + " </dd> </dl>";
                view = RenderRazorViewToString(this, "ShowOneButton", gameBoard);
                message = "<h1>You Lost!</h1>";
                gameOver = "1";
                package = new { part1 = view, part2 = message, part3 = playerStatsString, part4 = gameOver };
                return Json(package);
            }
            if (service.HasGameWon(gameBoard))
            {
                gameBoard.revealAll();

                playerStatsString = "<hr /> <dl class='row'> <dt class='col - sm - 2'> Player: </dt> <dd class='col - sm - 10'> " + player.playerName + " </dd> <dt class='col - sm - 2'> Difficulty: </dt> <dd class='col - sm - 10'> " + player.difficulty + " </dd> <dt class='col - sm - 2'> Score: </dt> <dd class='col - sm - 10'> " + player.score + " </dd> </dl>";
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
            Board newBoard = new Board(10);
            newBoard.setupLiveNeighbors(difficulty);
            //reset game id
            HttpContext.Session.SetInt32("_GameId", -1);
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
