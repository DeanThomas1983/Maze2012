using System.Drawing;

namespace Maze2012
{
    class DataModel
    {
        MazeStructure mazeStructure;
        SolverAgentList solverAgentList;

        public Bitmap TwoDimensionalMap { get { return drawTwoDimensionalMap(); } }

        private Bitmap drawTwoDimensionalMap()
        {
            Bitmap twoDimensionalMap = mazeStructure.TwoDimensionalMap;

            Graphics g = Graphics.FromImage(twoDimensionalMap);
            Pen p = new Pen(Color.Blue);

            foreach (SolverAgent agent in solverAgentList)
            {
                g.DrawEllipse(p, mazeStructure.getBoundingRectangle(agent.CurrentCell));
            }

            g.Dispose();

            return twoDimensionalMap;
        }

        public MazeStructure MazeStructure { get { return mazeStructure; } }
        public SolverAgentList SolverAgentList { get { return solverAgentList; } }

        public DataModel()
        {
            mazeStructure = new MazeStructure();
            solverAgentList = new SolverAgentList();
        }
    }
}
