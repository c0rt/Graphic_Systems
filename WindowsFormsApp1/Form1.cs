using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Figure figure;
        private int trackBarValueXCentre, trackBarValueYCentre, trackBarValueZCentre;
        private int trackBarValueXAxis, trackBarValueYAxis, trackBarValueZAxis;
        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            figure = new Figure(pictureBox1);
            if (comboBox1.Text == "Параллельная")
                figure.DrawParallel();
            else
                figure.DrawPerspective();
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.ScaleFigure(e.Delta);
            figure.MoveFigure(trackBarMoveX.Value, trackBarMoveY.Value, trackBarMoveZ.Value);
            if (checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            if (comboBox1.Text == "Параллельная")
                figure.DrawParallel();
            else
                figure.DrawPerspective();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackBarMoveX.Value = 0;
            trackBarMoveY.Value = 0;
            trackBarMoveZ.Value = 0;
            trackBarAngleX.Value = 0;
            trackBarAngleY.Value = 0;
            trackBarAngleZ.Value = 0;
            figure.ScaleFigure(0);
            if (comboBox1.Text == "Параллельная")
                figure.DrawParallel();
            else
                figure.DrawPerspective();
        }

        private void trackBarRotate_ValueChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.ScaleFigure(0);
            figure.MoveFigure(trackBarMoveX.Value, trackBarMoveY.Value, trackBarMoveZ.Value);
            if (checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            if (comboBox1.Text == "Параллельная")
                figure.DrawParallel();
            else
                figure.DrawPerspective();
            
        }
        private void trackBarTransfer_ValueChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.ScaleFigure(0);
            figure.MoveFigure(trackBarMoveX.Value, trackBarMoveY.Value, trackBarMoveZ.Value);
            if (checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            if (comboBox1.Text == "Параллельная")
                figure.DrawParallel();
            else
                figure.DrawPerspective();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackBarMoveX.Value = 0;
            trackBarMoveY.Value = 0;
            trackBarMoveZ.Value = 0;
            trackBarAngleX.Value = 0;
            trackBarAngleY.Value = 0;
            trackBarAngleZ.Value = 0;
            figure.ChangeFigure(comboBox2.Text);
            if (comboBox1.Text == "Параллельная")
                figure.DrawParallel();
            else
                figure.DrawPerspective();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                trackBarValueXCentre = trackBarAngleX.Value;
                trackBarValueYCentre = trackBarAngleY.Value;
                trackBarValueZCentre = trackBarAngleZ.Value;
                trackBarAngleX.Value = trackBarValueXAxis;
                trackBarAngleY.Value = trackBarValueYAxis;
                trackBarAngleZ.Value = trackBarValueZAxis;
            }
            else
            {
                trackBarValueXAxis = trackBarAngleX.Value;
                trackBarValueYAxis = trackBarAngleY.Value;
                trackBarValueZAxis = trackBarAngleZ.Value;
                trackBarAngleX.Value = trackBarValueXCentre;
                trackBarAngleY.Value = trackBarValueYCentre;
                trackBarAngleZ.Value = trackBarValueZCentre;
            }
            /*trackBarMoveX.Value = 0;
            trackBarMoveY.Value = 0;
            trackBarMoveZ.Value = 0;*/
            /*trackBarAngleX.Value = 0;
            trackBarAngleY.Value = 0;
            trackBarAngleZ.Value = 0;*/
            figure.ScaleFigure(0);
            //figure.DrawFigurePerspective();
        }
    }
}
