/**
 *  @file SimpleSolverAgent.cs
 *  @author Dean Thomas
 *  @version 0.1
 *  
 *  @section LICENSE
 *  
 *  @section DESCRIPTION
 *  
 *  The SimpleSolverAgent class is used to implement a very
 *  simple solver which uses the method of always following
 *  the right hand wall to find the exit of the maze.
 */

namespace Maze2012
{
    class SimpleSolverAgent : SolverAgent
    {
        /**
         *  Calculate the next cell that the agent 
         *  should be in after moving one space.
         *  
         *  @return the new position of the agent after
         *  moving one space.
         */  
        protected override Cell calculateNextPosition()
        {
            //  Use the basic path finding routine from
            //  the parent class.
            return cellClockwise();
        }

        
    }
}
