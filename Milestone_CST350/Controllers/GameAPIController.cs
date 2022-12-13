using Microsoft.AspNetCore.Mvc;
using Milestone_CST350.Models;
using Milestone_CST350.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace Milestone_CST350.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameAPIController : ControllerBase
    {
        GameService service = new GameService();

        [HttpGet]
        [ResponseType(typeof(List<Board>))]
        public ActionResult<List<Board>> Index() 
        {
            return service.GetAllGames();
        }

        [HttpGet("getallUsergames/{UserId}")]
        [ResponseType(typeof(List<Board>))]
        public ActionResult<List<Board>> GetAllUserGames(int userId)
        {
            return service.GetAllUserGames(userId);
        }

        [HttpGet("game/{GameId}")]
        [ResponseType(typeof(Board))]
        public ActionResult<Board> FindGameById(int gameId)
        {
            return service.FindGameById(gameId);
        }


        [HttpPost("savegame")]
        public string SaveGame(Board gameBoard)
        {
            if (service.SaveGame(gameBoard.UserId, gameBoard) > 0)
            {
                return "Game was successfully saved";
            }
            else
            {
                return "Failed to save game";
            }
        }

        [HttpDelete("delete/{GameId}")]
        public string DeleteGame(int gameId)
        {
            if (service.DeleteGame(gameId))
            {
                return "Game was successfully deleted";
            }
            else
            {
                return "Failed to delete game";
            }
        }

        [HttpPut("update/{GameId}")]
        public string UpdateGame(int GameId, Board gameBoard)
        {
            if (GameId != gameBoard.Id)
            {
                return "Bad Request";
            }
            else 
            {
                if (service.UpdateGame(GameId, gameBoard))
                {
                    return "Game was successfully updated";
                }
                else
                {
                    return "Failed to update game";
                }
            }
        }
    }
}
