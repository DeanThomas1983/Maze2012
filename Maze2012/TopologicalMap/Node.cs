using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Maze2012
{
    class Node : List<Node>
    {
        private Node parentNode;
        private Int16 parentNodeDistance;
        private String nodeName;

        public Node(String nodeName, 
            Node parentNode = null, 
            Int16 parentNodeDistance = 0)
        {
            this.nodeName = nodeName;
            this.parentNode = parentNode;
            this.parentNodeDistance = parentNodeDistance;
        }

        public Bitmap draw()
        {
            Bitmap result = new Bitmap(32, 32);
            Graphics g = Graphics.FromImage(result);
            Pen pen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0,0,result.Width,result.Height);

            g.DrawEllipse(pen, rect);
            g.DrawString(this.nodeName, new Font("Arial", 8), new SolidBrush(Color.Black), (RectangleF)rect);


            pen.Dispose();
            g.Dispose();

            return result;
        }
    }


}
