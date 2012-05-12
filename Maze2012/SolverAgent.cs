using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Maze2012
{
    abstract class SolverAgent
    {
        //  Keep track of our position
        protected Cell previousCell;
        protected Cell currentCell;

        private DirectionOfTravel directionOfTravel;

        internal DirectionOfTravel DirectionOfTravel
        {
            get { return directionOfTravel; }
        }

        protected abstract Cell calculateNextPosition();



        internal Cell CurrentCell
        {
            get { return currentCell; }
        }

        public SolverAgent()
        {

        }

        private void calculateDirectionOfTravel()
        {
            DirectionOfTravel result;

            if (previousCell == null)
            {
                //  By default report that the agent is heading north
                result = DirectionOfTravel.NORTH;
            }
            else
            {
                if (currentCell.Coordinates.Y < previousCell.Coordinates.Y)
                    result = DirectionOfTravel.NORTH;
                else
                    if (currentCell.Equals(previousCell.CellToSouth))
                        result = DirectionOfTravel.SOUTH;
                    else
                        if (currentCell.Equals(previousCell.CellToEast))
                            result = DirectionOfTravel.EAST;
                        else
                            if (currentCell.Equals(previousCell.CellToWest))
                                result = DirectionOfTravel.WEST;
                            else
                            {
                                Debug.WriteLine("Invalid heading reached whilst evaluating direction of travel");

                                result = DirectionOfTravel.NORTH;
                            }
            }

            directionOfTravel = result;
        }

        /**
         *  Find the next cell clockwise
         *  
         *  Using the current heading of the agent find the nearest
         *  accessible cell in a clockwise direction
         *  
         *  @return the cell to move into
         */
        protected Cell cellClockwise()
        {
            switch (this.DirectionOfTravel)
            {
                case DirectionOfTravel.NORTH:
                    //  Check E->N>W->S
                    if (!CurrentCell.EastWall)
                        return CurrentCell.CellToEast;
                    else
                        if (!currentCell.NorthWall)
                            return CurrentCell.CellToNorth;
                        else
                            if (!currentCell.WestWall)
                                return CurrentCell.CellToWest;
                            else
                                return CurrentCell.CellToSouth;
                case DirectionOfTravel.EAST:
                    //  Check S->E->N->W
                    if (!currentCell.SouthWall)
                        return CurrentCell.CellToSouth;
                    else
                        if (!currentCell.EastWall)
                            return CurrentCell.CellToEast;
                        else
                            if (!currentCell.NorthWall)
                                return CurrentCell.CellToNorth;
                            else
                                return CurrentCell.CellToWest;
                case DirectionOfTravel.SOUTH:
                    //  Check W->S->E->N
                    if (!currentCell.WestWall)
                        return CurrentCell.CellToWest;
                    else
                        if (!currentCell.SouthWall)
                            return CurrentCell.CellToSouth;
                        else
                            if (!currentCell.EastWall)
                                return CurrentCell.CellToEast;
                            else
                                return CurrentCell.CellToNorth;
                case DirectionOfTravel.WEST:
                    //  Check N->W->S->E
                    if (!currentCell.NorthWall)
                        return CurrentCell.CellToNorth;
                    else
                        if (!currentCell.WestWall)
                            return CurrentCell.CellToWest;
                        else
                            if (!currentCell.SouthWall)
                                return CurrentCell.CellToSouth;
                            else
                                return CurrentCell.CellToEast;
                default:
                    Debug.WriteLine("Could not detect next cell clockwise");

                    return this.CurrentCell;
            }
        }

        public void setStartingCell(Cell startingCell)
        {
            this.previousCell = null;
            this.currentCell = startingCell;

            calculateDirectionOfTravel();
        }

        public void move()
        {
            previousCell = currentCell;

            if (previousCell != null)
                this.currentCell = this.calculateNextPosition();
            else
                Debug.WriteLine("Cannot move agent as it has no position assigned");

            calculateDirectionOfTravel();


            Debug.WriteLine("Previous position: " + previousCell.Coordinates.ToString()
                + "; Current position: " + currentCell.Coordinates.ToString());
        }

    }
}
