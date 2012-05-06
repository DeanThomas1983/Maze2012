using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze2012
{
    class MazeStructure
    {
#region PRIVATE_VARIABLES
        List<Cell> cells = new List<Cell>();

        //  Dimensions in cells
        int width;
        int height;
        //  Cell sizes
        int cellWidth;
        int cellHeight;
#endregion

        public MazeStructure() : this(8, 8) { }
        
        public MazeStructure(int width, int height)
        {
            //  Clear any previous structures
            if (cells != null)
            {
                cells.Clear();
            }

            //  Create the array of cells (as a 1D list)
            for (int i = 0; i < width * height; i++)
            {
                Cell newCell = new Cell();

                cells.Add(newCell);
            }

            //  Connect the cells
            connectCells();

            //  Set class variables
            this.width = width;
            this.height = height;
        }

        public void generateMaze()
        {
            //  Later will add additional algorithms
            this.generateDepthFirst();
        }

        private void connectCells()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                int row = 0;
                int col = 0;

                //  Take the 1D number and conver to a 2D position
                if (col == width)
                {
                    row++;
                    col = 0;
                }

                //  Set up connections
                if (row > 0)
                    cells[i].CellToNorth = cells[((col - 1) * width) + row];

                if (col > 0)
                    cells[i].CellToWest = cells[i - 1];

                if (row < height - 1)
                    cells[i].CellToSouth = cells[((col + 1) * width) + row];

                if (col < width - 1)
                    cells[i].CellToEast = cells[i + 1];
            }
        }

        private void generateDepthFirst()
        {
            Stack<Cell> cellStack = new Stack<Cell>();
            Random random = new Random();

            int visitedCells = 0;

            //  Start the maze at a random position
            Cell initialCell = cells[random.Next(cells.Count)];

            //  Push the origin onto the stack
            cellStack.Push(initialCell);
            visitedCells++;

            //  Repeat until we have visited ever cell in the maze
            while (visitedCells < cells.Count)
            {
                
                if (cellStack.Peek().NumberOfWalls == 4)
                {
                    cellStack.Peek().demolishRandomWall();
                }
            }
        }
    }
}
