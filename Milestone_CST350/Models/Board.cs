using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string DateTime { get; set; }
        public int UserId { get; set; }
        public string GameData { get; set; }
        public int size { get; set; }
        public int minesCount { get; set; }
        public Cell[,] grid;
        public decimal difficulty { get; set; }

        public static Cell newCell;
        public Cell copyCell = new Cell();
        public bool setBoard { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size"></param>
        /// <param name="grid"></param>
        /// <param name="difficulty"></param>
        public Board(int sizeVal)
        {
            this.size = sizeVal;
            grid = new Cell[sizeVal, sizeVal];
            setBoard = true;

            //populate 2d array with cell objects and the value for liveNeighbors to 0
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    newCell = new Cell(i, j, false, false, 0);
                    grid[i, j] = newCell;
                }
            }

        }

        public Board()
        {
        }

        public void setupLiveNeighbors(string difficultyLvl)
        {
            //get random number in range of the board eg. num x num. 
            //set the cell pos and count the number of mines placed and set the status of that cell to islive
            Random rand = new Random();
            int placedMines = 0;

            //set up difficulty percentage based on the passed parameter
            if (difficultyLvl == "easy" || difficultyLvl == "Easy")
            {
                difficulty = Convert.ToDecimal(size) * 0.4m;
            }
            else if (difficultyLvl == "medium" || difficultyLvl == "Medium")
            {
                difficulty = Convert.ToDecimal(size) * 0.7m;
            }
            else if (difficultyLvl == "hard" || difficultyLvl == "Hard")
            {
                difficulty = Convert.ToDecimal(size) * 1.0m;
            }

            //loop to place mines based on the percentage derived from diff
            while (placedMines < difficulty)
            {
                int mineI = 0, mineJ = 0;
                mineI = rand.Next(0, size);
                mineJ = rand.Next(0, size);

                copyCell = new Cell(mineI, mineJ, false, true, placedMines);
                grid[mineI, mineJ] = copyCell;
                placedMines++;
            }
            minesCount = placedMines;
        }

        public int calculateLiveNeighbors(int x, int y)
        {
            //nc = neighbor cell
            int nc = 0;

            //if on mine then set neighbor count to 9
            /*if (grid[x, y].live)
            {
                //where the mine is
                nc = 9;
                grid[x, y].buttonState = 9;
            }*/
            //check the neighbor cells (0-8) and calculate neighbor mines count
            if (x - 1 >= 0 && grid[x - 1, y].live)
            {
                nc++;
                grid[x, y].liveNeighbors = nc;
            }
            if (x - 1 >= 0 && y - 1 >= 0 && grid[x - 1, y - 1].live)
            {
                nc++;
                grid[x, y].liveNeighbors = nc;
            }
            if (x - 1 >= 0 && y + 1 < size && grid[x - 1, y + 1].live)
            {
                nc++;
                grid[x, y].liveNeighbors = nc;
            }
            if (y - 1 >= 0 && grid[x, y - 1].live)
            {
                nc++;
                grid[x, y].liveNeighbors = nc;
            }
            if (y + 1 < size && grid[x, y + 1].live)
            {
                nc++;
                grid[x, y].liveNeighbors = nc;
            }
            if (x + 1 < size && y - 1 >= 0 && grid[x + 1, y - 1].live)
            {
                nc++;
                grid[x, y].liveNeighbors = nc;
            }
            if (x + 1 < size && grid[x + 1, y].live)
            {
                nc++;
                grid[x, y].liveNeighbors = nc;
            }
            if (x + 1 < size && y + 1 < size && grid[x + 1, y + 1].live)
            {
                nc++;
                grid[x, y].liveNeighbors = nc;
            }

            grid[x, y].buttonState = nc;
            return nc;
        }

        //fills all the empty cells that were not visited and dont have a liveNeighbor count
        public void floodFill(int row, int col)
        {
            if (row < 0 || row >= size) { return; }
            if (col < 0 || col >= size) { return; }

            //int count = calculateLiveNeighbors(row, col);

            if (grid[row, col].live)
            {
                revealAll();
                return;
            }
            else if (calculateLiveNeighbors(row, col) > 0)
            {
                grid[row, col].visited = true;
                return;
            }
            else 
            {
                if (grid[row, col].visited == false)
                {
                    grid[row, col].visited = true;
                    floodFill((row + 1), col);
                    floodFill((row - 1), col);
                    floodFill(row, (col + 1));
                    floodFill(row, (col - 1));
                    floodFill((row + 1), (col + 1));
                    floodFill((row + 1), (col - 1));
                    floodFill((row - 1), (col + 1));
                    floodFill((row - 1), (col - 1));
                }
                else
                {
                    return;
                }
            }
            /*
            int count = calculateLiveNeighbors(row, col);

            //check to see if the cell is has a liveneighbor count if it does simply set it visited
            if (count > 0)
            {
                grid[row, col].visited = true;
            }
            //check to see if the cell was not visited then set it to visited and check other cells in all directions
            if (grid[row, col].visited == false)
            {
                grid[row, col].visited = true;
                floodFill((row + 1), col);
                floodFill((row - 1), col);
                floodFill(row, (col + 1));
                floodFill(row, (col - 1));
                floodFill((row + 1), (col + 1));
                floodFill((row + 1), (col - 1));
                floodFill((row - 1), (col + 1));
                floodFill((row - 1), (col - 1));
            }
            else
            {
                return;
            }*/
        }

        //get the total number of visited cells
        public int calcVisited()
        {
            int count = 0;
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (grid[r, c].visited)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void revealAll()
        {
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (grid[r, c].live)
                    {
                        grid[r, c].buttonState = 9; //bomb
                    }
                    else 
                    {
                        int count = calculateLiveNeighbors(r, c);
                        grid[r, c].buttonState = count;
                    }
                }
            }
        }
    }
}
