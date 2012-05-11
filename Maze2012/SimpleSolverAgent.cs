using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Maze2012
{
    class SimpleSolverAgent : SolverAgent
    {
        protected override Cell calculateNextPosition()
        {
            //if (this.directionOfTravel())

            return cellClockwise();
        }

        private Cell cellClockwise()
        {
            switch (this.DirectionOfTravel)
            {
                case DirectionOfTravel.NORTH:
                    //  Check E->W->N->S
                    if (!CurrentCell.EastWall) //    1 represents east
                        return CurrentCell.CellToEast;
                    else
                        if (!currentCell.WestWall) //    2 represents south
                            return CurrentCell.CellToWest;
                        else
                            if (!currentCell.NorthWall) //    3 represents west
                                return CurrentCell.CellToNorth;
                            else
                                return CurrentCell.CellToSouth;
                case DirectionOfTravel.EAST:
                    //  Check S->N->E->W
                    if (!currentCell.SouthWall) //    2 represents south
                        return CurrentCell.CellToSouth;
                    else
                        if (!currentCell.NorthWall) //    3 represents west
                            return CurrentCell.CellToNorth;
                        else
                            if (!currentCell.EastWall) //    0 represents north
                                return CurrentCell.CellToEast;
                            else
                                return CurrentCell.CellToWest;
                case DirectionOfTravel.SOUTH:
                    //  Check W->E->S->N
                    if (!currentCell.WestWall) //    3 represents west
                        return CurrentCell.CellToWest;
                    else
                        if (!currentCell.EastWall) //    0 represents north
                            return CurrentCell.CellToEast;
                        else
                            if (!currentCell.SouthWall) //    1 represents east
                                return CurrentCell.CellToSouth;
                            else
                                return CurrentCell.CellToNorth;
                case DirectionOfTravel.WEST:
                    //  Check N->S->W->E
                    if (!currentCell.NorthWall) //    0 represents north
                        return CurrentCell.CellToNorth;
                    else
                        if (!currentCell.SouthWall) //    1 represents east
                            return CurrentCell.CellToSouth;
                        else
                            if (!currentCell.WestWall) //    2 represents south
                                return CurrentCell.CellToWest;
                            else
                                return CurrentCell.CellToEast;
                default:
                    Debug.WriteLine("Could not detect next cell clockwise");

                    return this.CurrentCell;
            }
        }
    }
}
