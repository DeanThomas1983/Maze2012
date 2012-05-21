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
        SharpGLForm sharpGLForm = new SharpGLForm();

        int selected = 0;

        int i;

        public Form1()
        {
            InitializeComponent();

#if !NO_OPEN_GL
            sharpGLForm.Show();
#endif
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataModel = new DataModel();

            dataModel.MazeStructure.generationProgressChanged += 
                new MazeStructure.generationProgressChangedEventHandler(MazeStructure_generationProgressChanged);
            dataModel.MazeStructure.generationCompleted += new MazeStructure.generationCompletedEventHandler(MazeStructure_generationCompleted);
            
            dataModel.SolverAgentList.Add(new SimpleSolverAgent());

            g = panel1.CreateGraphics();

#if !NO_OPEN_GL
            sharpGLForm.DataModel = dataModel;
#endif
        }

        void MazeStructure_generationCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataModel.SolverAgentList.setAgentStartingCells(dataModel.MazeStructure.Origin);

            g.Clear(Color.Black);

            g.DrawImage(dataModel.TwoDimensionalMap, new Point(0, 0));

            
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
            dataModel.SolverAgentList.move();

            g.DrawImage(dataModel.TwoDimensionalMap, new Point(0, 0));

        }
    }
}
