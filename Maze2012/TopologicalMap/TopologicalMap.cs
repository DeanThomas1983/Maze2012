using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Maze2012
{
    class TopologicalMap
    {
        private Node rootNode;

        public TopologicalMap(String rootNodeName)
        {
            this.rootNode = new Node(rootNodeName);
        }

        public Bitmap draw()
        {
            Bitmap result = new Bitmap(640, 480);
            Graphics g = Graphics.FromImage(result);

            g.DrawImage(rootNode.draw(), 0, 0);

            g.Dispose();

            return result;
        }
    }
}
