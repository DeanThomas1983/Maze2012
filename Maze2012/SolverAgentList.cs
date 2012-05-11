using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Maze2012
{
    class SolverAgentList : List<SolverAgent>
    {
        public SolverAgentList()
        {

        }

        public void setAgentStartingCells(Cell startingCell)
        {
            foreach (SolverAgent solverAgent in this)
            {
                solverAgent.setStartingCell(startingCell);
            }

            outputAgentPositions();
        }

        public void move()
        {
            foreach (SolverAgent solverAgent in this)
            {
                solverAgent.move();
            }

            outputAgentPositions();
        }

        private void outputAgentPositions()
        {
            foreach (SolverAgent solverAgent in this)
            {
                Debug.WriteLine("Solver at position : " + solverAgent.CurrentCell.Coordinates.ToString());
            }
        }
    }
}
