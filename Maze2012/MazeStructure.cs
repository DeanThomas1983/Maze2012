/**
 *  @file MazeStructure.cs
 *  @author Dean Thomas
 *  @version 0.1
 *  
 *  @section LICENSE
 *  
 *  @section DESCRIPTION
 *  
 *  The MazeStructure class is a class to hold an array of cells to form a maze.  Also includes
 *  methods to generate the maze using different algorithms.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;

namespace Maze2012
{
    class MazeStructure
    {
        #region REFERENCES

        //  Background worker:  
        //  http://msdn.microsoft.com/en-us/library/system.componentmodel.backgroundworker.aspx     [11-05-2012]
       
        #endregion
        #region PRIVATE_VARIABLES

        //  List of all the cells associated with the current maze
        List<Cell> cells = new List<Cell>();

        //  Random number generator
        private static Random random = new Random();

        //  Dimensions of the maze (number of cells)
        Size mazeDimensions;

        //  Cell sizes
        Size cellSize;

        //  Start and end of the maze
        Cell origin;
        Cell terminus;

        //  Cell index to highlight
        int selectedCellIndex = -1;

        //  Holder for 2D representation of the maze
        Bitmap twoDimensionalMap;

        //  Background worker for generation algorithms
        BackgroundWorker generationBackgroundWorker = new BackgroundWorker();

        #endregion
        #region PUBLIC_PROPERTIES

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
         *  Return the cell index currently marked as selected
         * 
         *  @return the index of the selected cell
         */
        public int SelectedCellIndex { get { return selectedCellIndex; } set { selectedCellIndex = value; } }

        /**
         *  Return the selected cell as an object
         * 
         *  Return the cell currently marked as selected
         * 
         *  @return the selected cell
         */
        public Cell SelectedCell { get { return cells[selectedCellIndex]; } }

        /**
         *  Return the coordinates of selected cell
         *  
         *  Return the coordinates of the cell current marked as selected
         *  
         *  @return the coordinates of the selected cell
         */
        public Point SelectedCellCoordinates { get { return indexToCoordinate(selectedCellIndex); } }

        #endregion
        #region DELEGATE_METHODS

        //  Handle progression event in maze generation
        public delegate void generationProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
        //  Handle completion event in maze generation
        public delegate void generationCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);

        #endregion
        #region EVENTS

        //  Raise an event to be handled by the parent object for completion of maze generation
        public event generationCompletedEventHandler generationCompleted;

        //  Raise an event to be handled by the parent object for progression of maze generation
        public event generationProgressChangedEventHandler generationProgressChanged;

        #endregion
        #region BACKGROUND_WORKER_METHODS

        /**
         *  Maze generation progressed
         *  
         *  The maze generator has progressed; pass this on to parent objects
         */
        void generationBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            generationProgressChangedEventHandler handler = generationProgressChanged;

            if (handler != null)
            {
                //  Invoke the delegate
                handler(this, e);
            }
        }

        /**
         *  Maze generation completed
         *  
         *  The generation background thread completed; maze has been completed
         */
        void generationBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            generationCompletedEventHandler handler = generationCompleted;

            if (handler != null)
            {
                //  Invoke the delegate
                handler(this, e);
            }
        }

        /**
         *  Generate a new maze
         *  
         *  Generation background work has been started; begin creating a new maze
         */
        void generationBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("Generating new maze");

            this.resetMaze();

            //  Later will add additional algorithms
            this.generateDepthFirst();
        }

        #endregion
        #region CONSTRUCTOR_METHODS

        /**
         *  Default constructor
         *  
         *  Construct a new maze using the default constructor
         */
        public MazeStructure() : this(new Size(16, 16), new Size(16, 16)) { }

        /**
         *  Overloaded constructor
         *  
         *  Construct a new maze using the specified parameters
         *  
         *  @param mazeDimensions number of cells in the x and y dimensions
         *  @param cellSize the size of each cell in pixels in the x and y dimensions
         */
        public MazeStructure(Size mazeDimensions, Size cellSize)
        {
            //  Indent console output
            Debug.Indent();

            //  Set class variables
            this.mazeDimensions = mazeDimensions;
            this.cellSize = cellSize;

            //  Set up the background worker for maze generation
            this.generationBackgroundWorker.WorkerReportsProgress = true;
            this.generationBackgroundWorker.DoWork += new DoWorkEventHandler(generationBackgroundWorker_DoWork);
            this.generationBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(generationBackgroundWorker_ProgressChanged);
            this.generationBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(generationBackgroundWorker_RunWorkerCompleted);


            //  Clear the maze and setup connections
            resetMaze();
        }

        #endregion
        #region PRIVATE_METHODS
        /**
         *  Reset the maze
         *  
         *  Clear any existing maze structure and rebuild all walls between cells
         */
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

        /**
         *  Create a two dimensional representation of the maze
         *  
         *  Create a two dimensional representation of the maze and store it
         *  internally within the maze object
         */
        private void createTwoDimensionalMap()
        {
            //  If a map currently exists dispose of it
            if (twoDimensionalMap != null)
                twoDimensionalMap.Dispose();

            //  Create a bitmap large enough to hold the map
            twoDimensionalMap = new Bitmap(this.mazeDimensions.Width * cellSize.Width,
                this.mazeDimensions.Height * cellSize.Height);

            //  Create graphics objects to draw the map
            Graphics g = Graphics.FromImage(twoDimensionalMap);
            Pen pen = new Pen(Color.Blue);

            //  Loop through all the cells in the array list
            for (int i = 0; i < this.cells.Count; i++)
            {
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
                        //  Highlight selected cell in yellow
                        if (i == selectedCellIndex)
                        {
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
                        new Point(cellSize.Width * indexToCoordinate(i).X, 
                            cellSize.Height * indexToCoordinate(i).Y),
                        new Point(cellSize.Width * (indexToCoordinate(i).X + 1), 
                            cellSize.Height * indexToCoordinate(i).Y));

                //  Draw the south wall (if present)
                if (cells[i].SouthWall)
                    g.DrawLine(pen,
                        new Point(cellSize.Width * indexToCoordinate(i).X, 
                            (cellSize.Height * (indexToCoordinate(i).Y + 1)) - 1),
                        new Point(cellSize.Width * (indexToCoordinate(i).X + 1), 
                            (cellSize.Height * (indexToCoordinate(i).Y + 1)) - 1));

                //  Draw the west wall (if present)
                if (cells[i].WestWall)
                    g.DrawLine(pen,
                        new Point(cellSize.Width * indexToCoordinate(i).X, 
                            cellSize.Height * indexToCoordinate(i).Y),
                        new Point(cellSize.Width * indexToCoordinate(i).X, 
                            cellSize.Height * (indexToCoordinate(i).Y + 1)));

                //  Draw the east wall (if present)
                if (cells[i].EastWall)
                    g.DrawLine(pen,
                        new Point((cellSize.Width * (indexToCoordinate(i).X + 1) - 1), 
                            cellSize.Height * indexToCoordinate(i).Y),
                        new Point((cellSize.Width * (indexToCoordinate(i).X + 1) - 1), 
                            cellSize.Height * (indexToCoordinate(i).Y + 1)));

            }

            //  Dispose of graphics objects
            pen.Dispose();
            g.Dispose();
        }

        /**
         *  Build connections between cells
         *  
         *  Allow the cells in the maze to be aware of their immediate neighbours
         */
        private void connectCells()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                //  Set up connections
                if (indexToCoordinate(i).Y > 0)
                    cells[i].CellToNorth = cells[coordinateToIndex(
                        new Point(indexToCoordinate(i).X, indexToCoordinate(i).Y - 1))];

                if (indexToCoordinate(i).Y < this.mazeDimensions.Height - 1)
                    cells[i].CellToSouth = cells[coordinateToIndex(
                        new Point(indexToCoordinate(i).X, indexToCoordinate(i).Y + 1))];

                if (indexToCoordinate(i).X > 0)
                    cells[i].CellToWest = cells[coordinateToIndex(
                        new Point(indexToCoordinate(i).X - 1, indexToCoordinate(i).Y))];

                if (indexToCoordinate(i).X < mazeDimensions.Width - 1)
                    cells[i].CellToEast = cells[coordinateToIndex(
                        new Point(indexToCoordinate(i).X + 1, indexToCoordinate(i).Y))];

            }
        }

        /**
         *  Convert between cell index and coordinates
         *  
         *  Take a one dimensional index and covert it to a two dimensional
         *  coordinate within the maze
         *  
         *  @param index the index of the cell within the array list
         *  @return Point the coordinates of the corresponding cell
         */
        private Point indexToCoordinate(int index)
        {
            Point result = new Point();

            //  Calculate X and Y coordinates
            result.X = index % this.mazeDimensions.Width;
            result.Y = index / this.mazeDimensions.Width;

            return result;
        }

        /**
         *  Convert between coordinates and cell index
         * 
         *  Take a two dimension cell coordinate and convert it to a 
         *  one dimensional index in the cell array
         *  
         *  @param Point the coordinates of the corresponding cell
         *  @return index the index of the cell within the array list
         */
        private int coordinateToIndex(Point coordinate)
        {
            if ((coordinate.Y >= 0) && (coordinate.Y < mazeDimensions.Height))
            {
                if ((coordinate.X >= 0) && (coordinate.X < mazeDimensions.Height))
                {
                    int result = 0;

                    result = coordinate.Y * mazeDimensions.Width;

                    result += coordinate.X;

                    return result;
                }
                else
                {
                    Debug.WriteLine("coordinate.X {0} out of range (maxium {1})", 
                        coordinate.X, mazeDimensions.Width - 1);

                    return -1;
                }
            }
            else
            {
                Debug.WriteLine("coordinate.Y {0} out of range (maximum {1})", 
                    coordinate.Y, mazeDimensions.Height - 1);

                return -1;
            }
        }

        /**
         *  Generate a depth first maze
         *  
         *  Use the depth first maze creation algorithm to build the
         *  connections between the cells
         */
        private void generateDepthFirst()
        {
            //  Keep tabs on the number of cells visited and the order in
            //  which they were visited
            Stack<Cell> cellStack = new Stack<Cell>();
            int visitedCells = 0;

            //  Start the maze at a random position
            Cell currentCell = cells[random.Next(cells.Count)];
            origin = currentCell;

            //  Push the origin onto the stack
            cellStack.Push(currentCell);
            visitedCells++;

            //  Output the current position and count to the console
            Debug.WriteLine("Current cell [{0},{1}]",
                indexToCoordinate(cells.IndexOf(currentCell)).X,
                indexToCoordinate(cells.IndexOf(currentCell)).Y);
            Debug.WriteLine("Visited cells is now {0}", visitedCells);

            //  Repeat until we have visited ever cell in the maze
            while (visitedCells < cells.Count)
            {
                //  Potential cell connections represents neighboring
                //  cells with four walls intact
                if (currentCell.PotentialCellConnections.Count > 0)
                {
                    //  Move in a random direction
                    currentCell = currentCell.demolishRandomWall();

                    //  Put the new cell on the stack
                    cellStack.Push(currentCell);

                    //  Output the current position and count to the console
                    Debug.WriteLine("Current cell [{0},{1}]",
                        indexToCoordinate(cells.IndexOf(currentCell)).X,
                        indexToCoordinate(cells.IndexOf(currentCell)).Y);
                    Debug.WriteLine("Visited cells is now {0}", visitedCells);

                    //  Mark that we have moved to another cell
                    visitedCells++;
                }
                else
                {
                    //  Go back down the path we previously followed
                    currentCell = cellStack.Pop();

                    //  Output the current position and count to the console
                    Debug.WriteLine("Current cell [{0},{1}]",
                        indexToCoordinate(cells.IndexOf(currentCell)).X,
                        indexToCoordinate(cells.IndexOf(currentCell)).Y);
                }

                //  Report the generation progress to the delegate method
                generationBackgroundWorker.ReportProgress(
                    (int)(100 * ((decimal)visitedCells / (decimal)cells.Count)));
            }

            //  Mark the exit of the maze
            this.terminus = currentCell;
        }
        #endregion
        #region PUBLIC_METHODS

        /**
         *  Generate a new maze
         *  
         *  Generate a new maze; handled on a background thread
         */
        public void generateMaze()
        {
            //  Start the generator thread
            generationBackgroundWorker.RunWorkerAsync();
        }

        #endregion
    }
}
