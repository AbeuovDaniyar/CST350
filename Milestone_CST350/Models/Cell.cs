using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public int buttonState { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public bool visited { get; set; }
        public bool live { get; set; }
        public int liveNeighbors { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="visited"></param>
        /// <param name="live"></param>
        /// <param name="liveNeighbors"></param>
        public Cell(int row, int column, bool visited, bool live, int liveNeighbors)
        {
            this.row = row;
            this.column = column;
            this.visited = visited;
            this.live = live;
            this.liveNeighbors = liveNeighbors;
            this.buttonState = 10;
        }

        public Cell()
        {
            this.row = -1;
            this.buttonState = 10;
            this.column = -1;
            this.visited = false;
            this.live = false;
            this.liveNeighbors = 0;
        }
    }
}
