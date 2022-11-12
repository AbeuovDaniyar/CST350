using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Models
{
    public class PlayerStats
    {
        public string playerName { get; set; }
        public string difficulty { get; set; }
        public double score { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="difficulty"></param>
        /// <param name="score"></param>
        public PlayerStats(string playerName, string difficulty, double score)
        {
            this.playerName = playerName;
            this.difficulty = difficulty;
            this.score = score;
        }

        public PlayerStats(string playerName)
        {
            this.playerName = playerName;
            this.difficulty = "";
            this.score = 0;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            PlayerStats otherPlayer = obj as PlayerStats;

            if (this.score > otherPlayer.score)
            {
                return 1;
            }
            else if (this.score < otherPlayer.score)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public override string ToString()
        {
            return "Player name: " + playerName + " Difficulty played: " + difficulty + " Score: " + score;
        }
    }
}
