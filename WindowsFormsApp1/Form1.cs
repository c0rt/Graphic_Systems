using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Figure figure;                                                        // Объект Figure
        private int trackBarValueXCentre, trackBarValueYCentre, trackBarValueZCentre; // Значения трекбаров смещения
        private int trackBarValueXAxis, trackBarValueYAxis, trackBarValueZAxis;       // Значения трекбаров вращения
        public Form1() // Инициализация компонентов
        {
            InitializeComponent();
            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
        }
        private void Form1_Load(object sender, EventArgs e) // Загрузка формы и инициализация Picture Box
        {
            figure = new Figure(pictureBox1);
            figure.Draw(comboBox1.Text == "Центральная");   // Вызов отрисовки с проверкой проекции
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e) // Отрисовка с применением масштаба
        {
            // Преобразования точек фигуры происходит в порядке Вращение->Смещение в случае вращения относительно центра фигуры
            // и в порядке Смещение->Вращение в случае вращения относительно центра координат

            // Такой же код присутствует в каждом Event'е формы, связанном с преобразованием фигуры
            if (!checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.Scale(e.Delta);
            figure.MoveFigure(trackBarMoveX.Value, trackBarMoveY.Value, trackBarMoveZ.Value);
            if (checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.Draw(comboBox1.Text == "Центральная");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // Обнуление значений трекбаров и масштаба при смене фигуры
        {
            trackBarMoveX.Value = 0;
            trackBarMoveY.Value = 0;
            trackBarMoveZ.Value = 0;
            trackBarAngleX.Value = 0;
            trackBarAngleY.Value = 0;
            trackBarAngleZ.Value = 0;
            figure.Scale(0);
            figure.Draw(comboBox1.Text == "Центральная");
        }

        private void Form1_Resize(object sender, EventArgs e) // Перерисовка в соответствии с ресайзом формы
        {
            figure.ResizeBuffer();
            if (!checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.Scale(0);
            figure.MoveFigure(trackBarMoveX.Value, trackBarMoveY.Value, trackBarMoveZ.Value);
            if (checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.Draw(comboBox1.Text == "Центральная");
        }

        private void trackBarRotate_ValueChanged(object sender, EventArgs e) // Перерисовка в соответствии с изменениями трекбаров вращения
        {
            if (!checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.Scale(0);
            figure.MoveFigure(trackBarMoveX.Value, trackBarMoveY.Value, trackBarMoveZ.Value);
            if (checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.Draw(comboBox1.Text == "Центральная");
        }
        private void trackBarTransfer_ValueChanged(object sender, EventArgs e) // Перерисовка в соответствии с изменениями трекбаров смещения
        {
            if (!checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.Scale(0);
            figure.MoveFigure(trackBarMoveX.Value, trackBarMoveY.Value, trackBarMoveZ.Value);
            if (checkBox1.Checked)
                figure.RotateFigure(trackBarAngleX.Value, trackBarAngleY.Value, trackBarAngleZ.Value);
            figure.Draw(comboBox1.Text == "Центральная");
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) // Обнуление значений трекбаров и масштаба при смене осей вращения
        {
            trackBarMoveX.Value = 0;
            trackBarMoveY.Value = 0;
            trackBarMoveZ.Value = 0;
            trackBarAngleX.Value = 0;
            trackBarAngleY.Value = 0;
            trackBarAngleZ.Value = 0;
            figure.ChangeFigure(comboBox2.Text);
            figure.Draw(comboBox1.Text == "Центральная");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // Смена значений трекбаров на предудыщие при смене осей вращения
        {
            if (checkBox1.Checked)
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
        }
    }
}
