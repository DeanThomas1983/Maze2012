using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Maze2012
{
    class Node : List<Node>
    {
        private Int32 layerID;

        public Int32 LayerID
        {
            get { return layerID; }
        }
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

            if (parentNode == null)
                this.layerID = 0;
            else
                this.layerID = parentNode.LayerID + 1;
        }

        public Bitmap draw()
        {
            const int NODE_RADIUS = 32;

            Bitmap result = new Bitmap(NODE_RADIUS, NODE_RADIUS);
            Graphics g = Graphics.FromImage(result);
            Pen pen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0,0,NODE_RADIUS,NODE_RADIUS);
            SizeF textRect;
            Font font = new Font("Arial",8);

            g.DrawEllipse(pen, rect);

            //  Measure the size of the string so that we can
            //  centre the name in the node
            textRect = g.MeasureString(this.nodeName, font);
            g.DrawString(this.nodeName,
                font,
                new SolidBrush(Color.Black),
                new PointF(
                    (NODE_RADIUS - textRect.Width) / 2,
                    (NODE_RADIUS - textRect.Height) / 2));
                
            
            font.Dispose();
            pen.Dispose();
            g.Dispose();

            return result;
        }
    }


}
