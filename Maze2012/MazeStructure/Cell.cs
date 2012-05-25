/**
 *  @file Cell.cs
 *  @author Dean Thomas
 *  @version 0.1
 *  
 *  @section LICENSE
 *  
 *  @section DESCRIPTION
 *  
 *  The cell class holds information regarding individual cells within
 *  the maze structure.  Also contains functions to navigate between
 *  cells on a cell by cell basis.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Maze2012
{
    class Cell
    {
        #region CONSTANTS

        public static int DISTANCE_UNINITILAISED = -1;

        //  Index of cells in connection list
        const int NORTH = 0;
        const int EAST = 1;
        const int SOUTH = 2;
        const int WEST = 3;
        //  Number of maximum connections
        const int MAXIMUM_CONNECTIONS = 4;

        Size cellSize;

        #endregion
        #region PRIVATE_VARIABLES

        //  Lookup table for connected cells
        private Cell[] connectedCells;
        //  Walls
        private bool[] walls;
        //  Let the cell know if it is the origin or exit
        private bool isOrigin;
        private bool isExit;

        //  Random number generator
#if FIXED_MAZE_LAYOUT
        private static Random random = new Random(1);
#else
        private static Random random = new Random();
#endif
        //  Coordinates of the cell in the maze
        private Point coordinates;

        //  Distance from the origin
        private int distanceFromOrigin;

        #endregion
        #region PUBLIC_PROPERTIES
        //  Connected cells
        public Cell CellToNorth { get { return connectedCells[NORTH]; } set { connectedCells[NORTH] = value; } }
        public Cell CellToSouth { get { return connectedCells[SOUTH]; } set { connectedCells[SOUTH] = value; } }
        public Cell CellToEast { get { return connectedCells[EAST]; } set { connectedCells[EAST] = value; } }
        public Cell CellToWest { get { return connectedCells[WEST]; } set { connectedCells[WEST] = value; } }

        //  List of valid connections
        public List<int> PotentialCellConnections { get { return buildListOfNeighboursWithAllWallsIntact(); } }

        //  Walls
        public Boolean NorthWall { get { return walls[NORTH]; } set { walls[NORTH] = value; } }
        public Boolean SouthWall { get { return walls[SOUTH]; } set { walls[SOUTH] = value; } }
        public Boolean EastWall { get { return walls[EAST]; } set { walls[EAST] = value; } }
        public Boolean WestWall { get { return walls[WEST]; } set { walls[WEST] = value; } }
        public int NumberOfWalls { get { return countWalls(); } }

        //  Coordinates
        public Point Coordinates { get { return coordinates; } }

        //  Size of the cell in pixels
        public Size CellSize
        {
            get { return cellSize; }
            set { cellSize = value; }
        }

        public bool IsOrigin
        {
            get { return isOrigin; }
            set { isOrigin = value; }
        }

        public bool IsExit
        {
            get { return isExit; }
            set { isExit = value; }
        }

        /**
         *  Distance from the origin cell
         *  
         *  Return the distance from the origin cell using the shortest path
         *  
         *  @return the distance from the origin as an integer
         */
        public int DistanceFromOrigin
        {
            get { return distanceFromOrigin; }
            set { distanceFromOrigin = value; }
        }
         
        #endregion
        #region PRIVATE_METHODS

        /**
         *  Build a list of neighbouring cells with four walls
         *  
         *  Build a list of neighbouring cells with four walls.  Required
         *  as part of the depth first algorithm
         *  
         *  @return a list of the directions which have intact cells (if any)
         */
        private List<int> buildListOfNeighboursWithAllWallsIntact()
        {
            List<int> result = new List<int>();

            //  Build a list of connections to cells with all their walls intact
            for (int i = 0; i < connectedCells.Length; i++)
            {
                if (connectedCells[i] != null)
                {
                    if (connectedCells[i].countWalls() == 4)
                    {
                        //  List of directions
                        result.Add(i);
                    }
                }
            }

            return result;
        }

        /**
         *  Demolish the north wall
         * 
         *  Demolish the wall between this cell and the one to the north.
         *  Both this wall and the south wall of north cell are demolished.
         *  
         *  @return true if successful
         */ 
        private Boolean demolishNorthWall()
        {
            if ((connectedCells[NORTH] != null) && (this.NorthWall))
            {
                this.walls[NORTH] = false;
                this.CellToNorth.walls[SOUTH] = false;
                return true;
            }
            else
            {
                Debug.WriteLine("Cannot demolish north wall");
                return false;
            }
        }

        /**
         *  Demolish the south wall
         * 
         *  Demolish the wall between this cell and the one to the south.
         *  Both this wall and the north wall of south cell are demolished.
         *  
         *  @return true if successful
         */
        private Boolean demolishSouthWall()
        {
            if ((connectedCells[SOUTH] != null) && (this.SouthWall))
            {
                this.walls[SOUTH] = false;
                this.CellToSouth.walls[NORTH] = false;
                return true;
            }
            else
            {
                Debug.WriteLine("Cannot demolish south wall");
                return false;
            }
        }

        /**
         *  Demolish the east wall
         * 
         *  Demolish the wall between this cell and the one to the east.
         *  Both this wall and the west wall of east cell are demolished.
         *  
         *  @return true if successful
         */
        private Boolean demolishEastWall()
        {
            if ((connectedCells[EAST] != null) && (this.EastWall))
            {
                this.walls[EAST] = false;
                this.CellToEast.walls[WEST] = false;
                return true;
            }
            else
            {
                Debug.WriteLine("Cannot demolish east wall");
                return false;
            }
        }

        /**
         *  Demolish the west wall
         * 
         *  Demolish the wall between this cell and the one to the west.
         *  Both this wall and the east wall of west cell are demolished.
         *  
         *  @return true if successful
         */
        private Boolean demolishWestWall()
        {
            if ((connectedCells[WEST] != null) && (this.WestWall))
            {
                this.walls[WEST] = false;
                this.CellToWest.walls[EAST] = false;
                return true;
            }
            else
            {
                Debug.WriteLine("Cannot demolish west wall");
                return false;
            }
        }

        /**
         *  Count the number of intact walls
         *  
         *  Count how many walls the cell has intact.
         *  
         *  @param the number of walls as an integer.
         */
        private int countWalls()
        {
            int result = 0;

            for (int i = 0; i < walls.Length; i++)
            {
                if (this.walls[i])
                    result++;
            }
            return result;
        }

        #endregion
        #region PUBLIC_METHODS

        /**
         *  Pick a random wall to demolish
         * 
         *  First builds a list of cells with all their walls intact and
         *  then randomly picks one of these to demolish.
         *  
         *  @return the cell which is the neighbour that has been demolished
         *  or null in the case of an invalid selection.
         */
        public Cell demolishRandomWall()
        {
            List<int> potentialCellConnections = this.buildListOfNeighboursWithAllWallsIntact();

            int r = random.Next(potentialCellConnections.Count);

            switch (potentialCellConnections[r])
            {
                case NORTH: demolishNorthWall(); return CellToNorth;
                case SOUTH: demolishSouthWall(); return CellToSouth;
                case EAST: demolishEastWall(); return CellToEast;
                case WEST: demolishWestWall(); return CellToWest;
                default: Debug.WriteLine("Invalid cell selected for demolition"); return null;
            }
        }

        /**
         *  Knock down a wall
         *  
         *  Knock the wall down between two cells, the wall demolished
         *  is caluclated by assessing the orientation between the two
         *  cells.
         *  
         *  @param otherCell the second cell to knock the wall down
         */
        public void demolishWallBetweenCells(Cell otherCell)
        {
            //  Has a wall been knocked down?
            Boolean success = false;

            //  Demolish a wall on the Y axis
            if (this.coordinates.X == otherCell.coordinates.X)
            {
                if (this.coordinates.Y < otherCell.coordinates.Y)
                    success = demolishSouthWall();
                else
                    if (this.coordinates.Y > otherCell.coordinates.Y)
                        success = demolishNorthWall();
            }
            else
            {
                //  Demolish a wall on the X axis
                if (this.coordinates.Y == otherCell.coordinates.Y)
                {
                    if (this.coordinates.X < otherCell.coordinates.X)
                        success = demolishEastWall();
                    else
                        if (this.coordinates.X > otherCell.coordinates.X)
                            success = demolishWestWall();
                }
            }

            if (success)
                Debug.WriteLine("Wall successfully demolished");
            //return success;
        }

        /**
         *  Render the cell in 2D
         *  
         *  Draw the cell in 2D by representing walls as a line
         *  
         *  @return Bitmap the 2D render of the cell
         */
        public Bitmap draw2D()
        {
            //  Graphic objects
            Bitmap bitmap = new Bitmap(this.cellSize.Width, this.cellSize.Height);
            Graphics g = Graphics.FromImage(bitmap);
            Pen pen;

            //  Choose the colour of the pen
            if (this.isExit)
            {
                pen = new Pen(Color.Lime);
            }
            else
            {
                if (this.isOrigin)
                {
                    pen = new Pen(Color.Red);
                }
                else
                {
                    pen = new Pen(Color.Blue);
                }
            }

            //  Draw the north wall
            if (NorthWall)
                g.DrawLine(pen, 
                    new Point(0,0), 
                    new Point(cellSize.Width - 1, 0));

            //  Draw the south wall (if present)
            if (SouthWall)
                g.DrawLine(pen, 
                    new Point(0, cellSize.Height - 1),
                    new Point(cellSize.Width - 1, cellSize.Height - 1));

            //  Draw the west wall (if present)
            if (WestWall)
                g.DrawLine(pen,
                    new Point(0, 0),
                    new Point(0, cellSize.Height - 1));

            //  Draw the east wall (if present)
            if (EastWall)
                g.DrawLine(pen,
                    new Point(cellSize.Width - 1, 0),
                    new Point(cellSize.Width - 1, cellSize.Height - 1));

            //  Release the graphics object
            pen.Dispose(); 
            g.Dispose();

            return bitmap;
        }
        
        #endregion
        #region CONSTRUCTOR_METHODS

        /**
         *  Default constructor
         *  
         *  Create a new cell and set up lookup tables for connections
         *  and set all walls to intact
         */
        public Cell()
        {
            //  List of connected cells
            connectedCells = new Cell[4];

            //  Cell Walls
            walls = new bool[4] { true, true, true, true };

            //  Distance from origin
            distanceFromOrigin = DISTANCE_UNINITILAISED;

            //  By default neither of these are true
            isOrigin = false;
            isExit = false;
        }

        /**
         *  Override constructor
         *  
         *  Call the default constructor and then store the coordinates
         *  of where the cell is in the maze object.
         *  
         *  @param coordinates a 2 dimensional point in space relating to the distance in
         *  the parent maze object
         *  @param cellSize the size in pixels of the new cell
         */
        public Cell(Point coordinates, Size cellSize)
            : this()
        {
            this.coordinates = coordinates;
            this.cellSize = cellSize;
        }

        #endregion
    }
}
