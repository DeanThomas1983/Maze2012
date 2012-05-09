using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
        #region CONSTRUCTOR_METHODS
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
        #endregion

        public Bitmap get2DMap(Size cellSize)
        {
            Bitmap result = new Bitmap(this.width * cellSize.Width, this.height * cellSize.Height);
            Graphics g = Graphics.FromImage(result);

            Pen pen = new Pen(Color.Blue);

            int row = 0;
            int col = 0;

            for (int i = 0; i < this.cells.Count; i++)
            {
                if (col == width)
                {
                    row++;
                    col = 0;
                }

                //  Draw the north wall (if present)
                if (cells[i].NorthWall)
                    g.DrawLine(pen,
                        new Point(cellSize.Width * col, cellSize.Height * row),
                        new Point(cellSize.Width * (col + 1), cellSize.Height * row));

                //  Draw the south wall (if present)
                if (cells[i].SouthWall)
                    g.DrawLine(pen,
                        new Point(cellSize.Width * col, (cellSize.Height * (row + 1)) - 1),
                        new Point(cellSize.Width * (col + 1), (cellSize.Height * (row + 1)) - 1));

                //  Draw the west wall (if present)
                if (cells[i].WestWall)
                    g.DrawLine(pen,
                        new Point(cellSize.Width * col, cellSize.Height * row),
                        new Point(cellSize.Width * col, cellSize.Height * (row + 1)));

                //  Draw the east wall (if present)
                if (cells[i].EastWall)
                    g.DrawLine(pen,
                        new Point((cellSize.Width * (col + 1) - 1), cellSize.Height * row),
                        new Point((cellSize.Width * (col + 1) - 1), cellSize.Height * (row + 1)));

                col++;
            }

            return result;
        }

        public void generateMaze()
        {
            //  Later will add additional algorithms
            this.generateDepthFirst();
        }

        private void connectCells()
        {
            int row = 0;
            int col = 0;

            for (int i = 0; i < cells.Count; i++)
            {
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

                col++;
                
            }
        }

        private void generateDepthFirst()
        {
            Stack<Cell> cellStack = new Stack<Cell>();
            Random random = new Random();

            int visitedCells = 0;

            //  Start the maze at a random position
            Cell currentCell = cells[random.Next(cells.Count)];

            //  Push the origin onto the stack
            cellStack.Push(currentCell);
            visitedCells++;

            //  Repeat until we have visited ever cell in the maze
            while (visitedCells < cells.Count)
            {
                if (currentCell.NumberOfWalls > 1)
                {
                    currentCell = currentCell.demolishRandomWall();

                    cellStack.Push(currentCell);

                    visitedCells++;
                }
                else
                {
                    currentCell = cellStack.Pop();
                }
            }
        }
    }
}
