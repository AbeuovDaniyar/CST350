using Milestone_CST350.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Services
{
    public class GameService
    {
        public double score;
        public PlayerStats setPlayer(string user, string difficulty)
        {
            calcScore(difficulty);
            PlayerStats player = new PlayerStats(user, difficulty, score);
            return player;
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
            visitedCount += gameBoard.calcVisited();
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
