using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maze2012
{
    public partial class Form1 : Form
    {
        Graphics g;
        DataModel dataModel;
        int selected = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataModel = new DataModel();

            g = panel1.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataModel.MazeStructure.generateMaze();
            
            g.Clear(Color.Black);

            g.DrawImage(dataModel.MazeStructure.TwoDimensionalMap,new Point(0,0));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selected++;

            dataModel.MazeStructure.SelectedCell = selected;

            g.Clear(Color.Black);

            g.DrawImage(dataModel.MazeStructure.TwoDimensionalMap, new Point(0, 0));
        }
    }
}
