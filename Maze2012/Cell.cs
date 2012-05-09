using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Maze2012
{
    class Cell
    {
        #region CONSTANTS
        //  Index of cells in connection list
        int NORTH = 0;
        int EAST = 1;
        int SOUTH = 2;
        int WEST = 3;
        #endregion
        #region PRIVATE_VARIABLES
        //  Lookup table for connected cells
        private Cell[] connectedCells;
        //  Walls
        private bool[] walls;
        #endregion
        #region PROPERTIES
        //  Connected cells
        public Cell CellToNorth { get { return connectedCells[NORTH]; } set { connectedCells[NORTH] = value; } }
        public Cell CellToSouth { get { return connectedCells[SOUTH]; } set { connectedCells[SOUTH] = value; } }
        public Cell CellToEast { get { return connectedCells[EAST]; } set { connectedCells[EAST] = value; } }
        public Cell CellToWest { get { return connectedCells[WEST]; } set { connectedCells[WEST] = value; } }
        //  Walls
        public Boolean NorthWall { get { return walls[NORTH]; } set { walls[NORTH] = value; } }
        public Boolean SouthWall { get { return walls[SOUTH]; } set { walls[SOUTH] = value; } }
        public Boolean EastWall { get { return walls[EAST]; } set { walls[EAST] = value; } }
        public Boolean WestWall { get { return walls[WEST]; } set { walls[WEST] = value; } }
        public int NumberOfWalls { get { return countWalls(); } }
        #endregion
        private Boolean demolishNorthWall()
        {
            if (connectedCells[NORTH] != null)
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

        private Boolean demolishSouthWall()
        {
            if (connectedCells[SOUTH] != null)
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

        private Boolean demolishEastWall()
        {
            if (connectedCells[EAST] != null)
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

        private Boolean demolishWestWall()
        {
            if (connectedCells[WEST] != null)
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

        public Cell()
        {
            //  List of connected cells
            connectedCells = new Cell[4];

            //  Cell Walls
            walls = new bool[4] { true, true, true, true };

        }
    }
}
