using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze2012
{
    class SimpleSolverAgent : SolverAgent
    {
        protected override Cell calculateNextPosition()
        {
            return this.CurrentCell.CellToEast;
        }
    }
}
