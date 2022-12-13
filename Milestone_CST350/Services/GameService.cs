using Milestone_CST350.DataServices;
using Milestone_CST350.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Services
{
    public class GameService
    {
        DBConnection database = new DBConnection();
        GameDAO service = new GameDAO();
        public double score;

        public List<Board> GetAllGames() 
        {
            return service.GetAllGames(database.DbConnection());
        }

        public int SaveGame(int userId, Board gameBoard)
        {
            return service.SaveGame(userId, gameBoard, database.DbConnection());
        }

        public List<Board> GetAllUserGames(int userId)
        {
            return service.GetAllUserGames(userId, database.DbConnection());
        }

        public bool DeleteGame(int gameId)
        {
            return service.DeleteGame(gameId, database.DbConnection());
        }

        public bool UpdateGame(int gameId, Board gameBoard)
        {
            return service.UpdateGame(gameId, gameBoard, database.DbConnection());
        }

        public Board FindGameById(int Id)
        {
            return service.FindGameById(Id, database.DbConnection());
        }

        public PlayerStats setPlayer(string user, string difficulty)
        {
            calcScore(difficulty);
            PlayerStats player = new PlayerStats(user, difficulty, score);
            return player;
        }

        public Board LoadGame(Board gameBoard) 
        {
            Board newGame = Newtonsoft.Json.JsonConvert.DeserializeObject<Board>(gameBoard.GameData);
            newGame.Id = gameBoard.Id;
            newGame.UserId = gameBoard.UserId;
            newGame.DateTime = gameBoard.DateTime;

            return newGame;
        }

        public void calcScore(string difficultyLvl)
        {
            var rand = new System.Random();
            if (difficultyLvl == "easy" || difficultyLvl == "Easy")
            {
                score = 10 * rand.Next(51);
            }
            else if (difficultyLvl == "medium" || difficultyLvl == "Medium")
            {
                score = 10 * rand.Next(101);
            }
            else if (difficultyLvl == "hard" || difficultyLvl == "Hard")
            {
                score = 10 * rand.Next(151);
            }
        }
        public bool HasGameWon(Board gameBoard)
        {
            int visitedCount = 0;
            visitedCount = gameBoard.calcVisited();

            if (visitedCount >= (10 * 10) - gameBoard.minesCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasGameLost(Board gameBoard, int r, int c)
        {
            if (gameBoard.grid[r, c].live)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
