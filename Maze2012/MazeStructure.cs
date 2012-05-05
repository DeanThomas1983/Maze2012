using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze2012
{
    class MazeStructure
    {
        List<Cell> cells = new List<Cell>();

        public MazeStructure()
        {
            if (cells != null)
            {
                cells.Clear();
            }


        }
    }
}
