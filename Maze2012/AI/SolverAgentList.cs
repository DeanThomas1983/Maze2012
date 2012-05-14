/**
 *  @file SolverAgentList.cs
 *  @author Dean Thomas
 *  @version 0.1
 *  
 *  @section LICENSE
 *  
 *  @section DESCRIPTION
 *  
 *  The SolverAgentList class is used to hold a list
 *  of solver agents.  It provides additional functionality
 *  to set the start position and to move as a group.
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Maze2012
{
    class SolverAgentList : List<SolverAgent>
    {
        #region CONSTRUCTOR_METHODS

        /**
         *  Default constructor
         *  
         *  Create a new agent list object
         */
        public SolverAgentList()
        {

        }

        #endregion
        #region PUBLIC_METHODS

        /**
         *  Set the starting position of each of the agents
         *  
         *  Traverse through the list of solver agents and
         *  set each of them to the same starting position.
         *  
         *  @param the cell to start from (probably the maze
         *  origin cell)
         */
        public void setAgentStartingCells(Cell startingCell)
        {
            foreach (SolverAgent solverAgent in this)
            {
                solverAgent.setStartingCell(startingCell);
            }

            outputAgentPositions();
        }

        /**
         *  Move each of the agents forward one step
         *  
         *  Traverse through the list of agents and send
         *  a request to each of them to move forward one
         *  step.
         */
        public void move()
        {
            foreach (SolverAgent solverAgent in this)
            {
                solverAgent.move();
            }

            outputAgentPositions();
        }
        
        #endregion
        #region PRIVATE_METHODS

        /**
         *  Print current positions of agents
         *  
         *  Output the current position of all existing agents in
         *  the list to the debug console.
         */
        private void outputAgentPositions()
        {
            foreach (SolverAgent solverAgent in this)
            {
                Debug.WriteLine("Solver at position : " +
                    solverAgent.CurrentCell.Coordinates.ToString() +
                    " is heading " +
                    solverAgent.DirectionOfTravel.ToString());
            }
        } 

        #endregion
    }
}
