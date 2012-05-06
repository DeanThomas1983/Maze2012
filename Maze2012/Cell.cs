using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Maze2012
{
    class Cell
    {
#region PRIVATE_VARIABLES
        //  Lookup table for connected cells
        private Cell cellToNorth;
        private Cell cellToSouth;
        private Cell cellToEast;
        private Cell cellToWest;
        //  Walls between cells
        private Boolean northWall;
        private Boolean southWall;
        private Boolean eastWall;
        private Boolean westWall;
#endregion
        public Cell CellToNorth { get { return cellToNorth; } set { cellToNorth = value; } }
        public Cell CellToSouth { get { return cellToSouth; } set { cellToSouth = value; } }
        public Cell CellToEast { get { return cellToEast; } set { cellToEast = value; } }
        public Cell CellToWest { get { return cellToWest; } set { cellToWest = value; } }
        public int NumberOfWalls { get { return countWalls(); } }

        public Cell demolishRandomWall()
        {
            //  get a list of connected cells from the current cell
            List<Cell> connectedCells;


            Cell result;

            Random random = new Random();

            

            return result;
        }

        private Cell demolishNorthWall()
        {
            this.northWall = false;
            this.cellToNorth.southWall = false;

            return cellToNorth;
        }

        private Cell demolishSouthWall()
        {
            this.southWall = false;
            this.cellToSouth.northWall = false;

            return cellToSouth;
        }

        private Cell demolishEastWall()
        {
            this.eastWall = false;
            this.cellToEast.westWall = false;

            return cellToEast;
        }

        private Cell demolishWestWall()
        {
            this.westWall = false;
            this.cellToWest.eastWall = false;

            return cellToWest;
        }

        private int countWalls()
        {
            int result = 0;

            if (northWall)
                result++;

            if (southWall)
                result++;

            if (westWall)
                result++;

            if (eastWall)
                result++;

            return result;
        }

        public Cell()
        {
            northWall = true;
            southWall = true;
            eastWall = true;
            westWall = true;
        }
    }
}
