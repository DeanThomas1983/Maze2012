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
