using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Maze2012
{
    abstract class SolverAgent
    {
        private Cell currentCell;

        protected abstract Cell calculateNextPosition();

        internal Cell CurrentCell
        {
            get { return currentCell; }
        }

        public SolverAgent()
        {

        }

        public void setStartingCell(Cell startingCell)
        {
            this.currentCell = startingCell;
        }

        public void move()
        {
            if (currentCell != null)
                this.currentCell = this.calculateNextPosition();
            else
                Debug.WriteLine("Cannot move agent as it has no position assigned");
        }

    }
}
