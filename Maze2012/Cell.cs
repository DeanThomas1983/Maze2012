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
        const int NORTH = 0;
        const int EAST = 1;
        const int SOUTH = 2;
        const int WEST = 3;
        //  Number of maximum connections
        const int MAXIMUM_CONNECTIONS = 4;
        #endregion
        #region PRIVATE_VARIABLES
        //  Lookup table for connected cells
        private Cell[] connectedCells;
        //  Walls
        private bool[] walls;

        private List<int> potentialCellConnections = new List<int>();
        //  Random number generator
        static private Random random = new Random();
        #endregion
        #region PROPERTIES
        //  Connected cells
        public Cell CellToNorth { get { return connectedCells[NORTH]; } set { connectedCells[NORTH] = value; this.buildListOfPotentialConnections(); } }
        public Cell CellToSouth { get { return connectedCells[SOUTH]; } set { connectedCells[SOUTH] = value; this.buildListOfPotentialConnections(); } }
        public Cell CellToEast { get { return connectedCells[EAST]; } set { connectedCells[EAST] = value; this.buildListOfPotentialConnections(); } }
        public Cell CellToWest { get { return connectedCells[WEST]; } set { connectedCells[WEST] = value; this.buildListOfPotentialConnections(); } }
        //  List of valid connections
        public List<int> PotentialCellConnections { get { return potentialCellConnections; } }
        //  Walls
        public Boolean NorthWall { get { return walls[NORTH]; } set { walls[NORTH] = value; } }
        public Boolean SouthWall { get { return walls[SOUTH]; } set { walls[SOUTH] = value; } }
        public Boolean EastWall { get { return walls[EAST]; } set { walls[EAST] = value; } }
        public Boolean WestWall { get { return walls[WEST]; } set { walls[WEST] = value; } }
        public int NumberOfWalls { get { return countWalls(); } }
        
        #endregion
        private void buildListOfPotentialConnections()
        {
            potentialCellConnections.Clear();

            //  Build a list of connections to cells with all their walls intact
            for (int i = 0; i < connectedCells.Length; i++)
            {
                if (connectedCells[i] != null)
                {
                    if (connectedCells[i].countWalls() == 4)
                    {
                        //  List of directions
                        potentialCellConnections.Add(i);
                    }
                }
            }
        }

        public Cell demolishRandomWall()
        {
            int r = random.Next(potentialCellConnections.Count);

            switch (potentialCellConnections[r])
            {
                case NORTH: demolishNorthWall(); return CellToNorth;
                case SOUTH: demolishSouthWall(); return CellToSouth;
                case EAST: demolishEastWall(); return CellToEast;
                case WEST: demolishWestWall(); return CellToWest;
                default: Debug.WriteLine("Invalid cell selected for demolishion"); return null;
            }
        }

        private Boolean demolishNorthWall()
        {
            if ((connectedCells[NORTH] != null) && (this.NorthWall))
            {
                this.walls[NORTH] = false;
                this.buildListOfPotentialConnections();
                this.CellToNorth.walls[SOUTH] = false;
                this.CellToNorth.buildListOfPotentialConnections();
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
            if ((connectedCells[SOUTH] != null) && (this.SouthWall))
            {
                this.walls[SOUTH] = false;
                this.buildListOfPotentialConnections();
                this.CellToSouth.walls[NORTH] = false;
                this.CellToSouth.buildListOfPotentialConnections();
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
            if ((connectedCells[EAST] != null) && (this.EastWall))
            {
                this.walls[EAST] = false;
                this.buildListOfPotentialConnections();
                this.CellToEast.walls[WEST] = false;
                this.CellToEast.buildListOfPotentialConnections();
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
            if ((connectedCells[WEST] != null) && (this.WestWall))
            {
                this.walls[WEST] = false;
                this.buildListOfPotentialConnections();
                this.CellToWest.walls[EAST] = false;
                this.CellToWest.buildListOfPotentialConnections();
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
