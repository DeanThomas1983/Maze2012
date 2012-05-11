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

        int i;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataModel = new DataModel();

            dataModel.MazeStructure.generationProgressChanged += 
                new MazeStructure.generationProgressChangedEventHandler(MazeStructure_generationProgressChanged);
            dataModel.MazeStructure.generationCompleted += new MazeStructure.generationCompletedEventHandler(MazeStructure_generationCompleted);
            
            g = panel1.CreateGraphics();
        }

        void MazeStructure_generationCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            g.Clear(Color.Black);

            g.DrawImage(dataModel.MazeStructure.TwoDimensionalMap, new Point(0, 0));
        }

        void MazeStructure_generationProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }


        
        private void button1_Click(object sender, EventArgs e)
        {
            dataModel.MazeStructure.generateMaze();
            
            
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
