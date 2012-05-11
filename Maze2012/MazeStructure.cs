using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace Maze2012
{
    class MazeStructure
    {
        #region PRIVATE_VARIABLES
        //  List of all the cells associated with the current maze
        List<Cell> cells = new List<Cell>();

        //  Random number generator
        Random r = new Random();

        //  Dimensions of the maze (number of cells)
        Size mazeDimensions;

        //  Cell sizes
        Size cellSize;

        //  Start and end of the maze
        Cell origin;
        Cell terminus;

        //  Cell index to highlight
        int selectedCell = -1;

        //  Holder for 2D representation of the maze
        Bitmap twoDimensionalMap;
        #endregion
        /**
         *  Return the maze in 2D
         * 
         *  Return a map of the maze in 2 dimensions
         *  
         *  @return the map as a bitmap
         */
        public Bitmap TwoDimensionalMap { get { createTwoDimensionalMap(); return twoDimensionalMap; } }
        /**
         *  Return the index of the selected cell
         * 
         *  Return the cell currently marked as selected
         * 
         *  @return the index of the selected cell
         */  
        public int SelectedCell { get { return selectedCell; } set { selectedCell = value; } }


        #region CONSTRUCTOR_METHODS
        public MazeStructure() : this(new Size(8, 8), new Size(32,32)) { }
        
        public MazeStructure(Size mazeDimensions, Size cellSize)
        {
            Debug.Indent();

            //  Set class variables
            this.mazeDimensions = mazeDimensions;
            
            this.cellSize = cellSize;

            resetMaze();
        }
        #endregion
        
        private void resetMaze()
        {
            //  Clear any previous structures
            if (cells != null)
            {
                cells.Clear();
            }

            //  Create the array of cells (as a 1D list)
            for (int i = 0; i < mazeDimensions.Width * mazeDimensions.Height; i++)
            {
                Cell newCell = new Cell();

                cells.Add(newCell);
            }

            //  Connect the cells
            connectCells();
        }
        
        private void createTwoDimensionalMap()
        {
            if (twoDimensionalMap != null)
                twoDimensionalMap.Dispose();

            twoDimensionalMap = new Bitmap(this.mazeDimensions.Width * cellSize.Width,
                this.mazeDimensions.Height * cellSize.Height);
            
            Graphics g = Graphics.FromImage(twoDimensionalMap);

            Pen pen = new Pen(Color.Blue);

            int row = 0;
            int col = 0;

            for (int i = 0; i < this.cells.Count; i++)
            {
                if (col == this.mazeDimensions.Width)
                {
                    row++;
                    col = 0;
                }

                //  Highlight origin in light blue
                if (cells[i] == origin)
                {
                    pen.Color = Color.LightBlue;
                }
                else
                {
                    //  Mark the terminus in orange
                    if (cells[i] == terminus)
                    {
                        pen.Color = Color.Orange;
                    }
                    else
                    {
                        if (i == selectedCell)
                        {
                            //  Highlight selected cell in yellow
                            pen.Color = Color.Yellow;
                        }
                        else
                        {
                            /*
                            if ((selectedCell == coordinateToIndex(row - 1, col)) || (selectedCell == coordinateToIndex(row + 1, col))
                                || (selectedCell == coordinateToIndex(row, col - 1)) || (selectedCell == coordinateToIndex(row, col + 1)))
                            {
                                pen.Color = Color.LightGreen;
                            }
                            else
                             */
                            {
                                pen.Color = Color.Blue;
                            }
                        }
                    }
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

            pen.Dispose();

            g.Dispose();
        }

        public void generateMaze()
        {
            Debug.WriteLine("Generating new maze");

            this.resetMaze();

            //  Later will add additional algorithms
            this.generateDepthFirst();
        }

        private void connectCells()
        {
            int row = 0;
            int col = 0;

            for (int i = 0; i < cells.Count; i++)
            {
                //  Take the 1D number and convert to a 2D position
                if (col == this.mazeDimensions.Width)
                {
                    row++;
                    col = 0;
                }

                //  Set up connections
                if (row > 0)
                    cells[i].CellToNorth = cells[coordinateToIndex(row - 1, col)];

                if (row < this.mazeDimensions.Height - 1)
                    cells[i].CellToSouth = cells[coordinateToIndex(row + 1, col)];

                if (col > 0)
                    cells[i].CellToWest = cells[coordinateToIndex(row, col - 1)];

                if (col < mazeDimensions.Width - 1)
                    cells[i].CellToEast = cells[coordinateToIndex(row, col + 1)];

                col++;
                
            }
        }

        private Point indexToCoordinate(int index)
        {
            Point result = new Point();

            result.X = index % this.mazeDimensions.Width;
            result.Y = index / this.mazeDimensions.Width;
            
            return result;
        }

        private int coordinateToIndex(int row, int col)
        {
            if ((row >= 0) && (row < mazeDimensions.Height))
            {
                if ((col >= 0) && (col < mazeDimensions.Height))
                {
                    int result = 0;

                    result = row * mazeDimensions.Width;

                    result += col;

                    return result;
                }
                else
                {
                    Debug.WriteLine("Col {0} out of range (maxium {1})", col, mazeDimensions.Width - 1);

                    return -1;
                }
            }
            else
            {
                Debug.WriteLine("Row {0} out of range (maximum {1})", row, mazeDimensions.Height - 1);

                return -1;
            }
        }

        private void generateDepthFirst()
        {
            Stack<Cell> cellStack = new Stack<Cell>();
            Random random = new Random();

            int visitedCells = 0;

            //  Start the maze at a random position
            Cell currentCell = cells[random.Next(cells.Count)];
            origin = currentCell;

            //  Push the origin onto the stack
            cellStack.Push(currentCell);
            visitedCells++;

            Debug.WriteLine("Current cell [{0},{1}]",
                indexToCoordinate(cells.IndexOf(currentCell)).X, 
                indexToCoordinate(cells.IndexOf(currentCell)).Y);
            Debug.WriteLine("Visited cells is now {0}", visitedCells);

            //  Repeat until we have visited ever cell in the maze
            while (visitedCells < cells.Count)
            {
                if (currentCell.PotentialCellConnections.Count > 0)
                {
                    currentCell = currentCell.demolishRandomWall();

                    cellStack.Push(currentCell);

                    Debug.WriteLine("Current cell [{0},{1}]",
                        indexToCoordinate(cells.IndexOf(currentCell)).X,
                        indexToCoordinate(cells.IndexOf(currentCell)).Y);
                    Debug.WriteLine("Visited cells is now {0}", visitedCells);

                    visitedCells++;
                }
                else
                {
                    currentCell = cellStack.Pop();

                    Debug.WriteLine("Current cell [{0},{1}]",
                        indexToCoordinate(cells.IndexOf(currentCell)).X,
                        indexToCoordinate(cells.IndexOf(currentCell)).Y);
                }
            }

            //  Mark the exit of the maze
            this.terminus = currentCell;
        }
    }
}
