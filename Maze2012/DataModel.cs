using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze2012
{
    class DataModel
    {
        MazeStructure mazeStructure;
        SolverAgentList solverAgentList;

        public MazeStructure MazeStructure { get { return mazeStructure; } }
        public SolverAgentList SolverAgentList { get { return solverAgentList; } }

        public DataModel()
        {
            mazeStructure = new MazeStructure();
            solverAgentList = new SolverAgentList();
        }
    }
}
