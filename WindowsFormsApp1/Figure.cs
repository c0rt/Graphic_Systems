using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace WindowsFormsApp1
{
    public class Figure
    {
        private struct Edge
        {
            public int start, end;
        };

        private struct Face
        {
            public int[] p;
        };

        private Vector3D light_dir = new Vector3D(0, 0, 1);

        private StreamReader reader;
        private int countPoints, countEdges, countFaces;
        private Point3D[] points, pointsToDraw;
        private PointF[] projPoints;
        private Edge[] edges;
        private Face[] faces;

        private const double focus = 2000;
        private const double minScale = 0.1;
        public double scale, defTranslationX, defTranslationY;
        private PictureBox pb;

        private double[,] zbuffer;

        private Matrix3D resultTransformMatrix;

        //private static double min = Double.PositiveInfinity, max = Double.NegativeInfinity;

        public Figure(PictureBox newPicBox)
        {
            try
            {
                pb = newPicBox;

                defTranslationX = pb.Width / 2;
                defTranslationY = pb.Height / 2;

                ReadFromFile(@"Икосаэдр.txt");

                scale = 100;
                resultTransformMatrix = new Matrix3D();
                points.CopyTo(pointsToDraw, 0);
                Scale(0);

                zbuffer = new double[pb.Width, pb.Height];
                for (int i = 0; i < pb.Width; i++)
                    for (int j = 0; j < pb.Height; j++)
                        zbuffer[i, j] = Double.NegativeInfinity;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void ReadFromFile(String figure)
        {
            try
            {
                reader = new StreamReader(figure);
                countPoints = int.Parse(reader.ReadLine());
                points = new Point3D[countPoints];
                pointsToDraw = new Point3D[countPoints];
                projPoints = new PointF[countPoints];

                int i = 0;
                while (i != countPoints)
                {
                    var line = reader.ReadLine();
                    var tempVals = line.Split().Select(Convert.ToDouble).ToList();
                    points[i].X = tempVals[0];
                    points[i].Y = tempVals[1];
                    points[i].Z = tempVals[2];
                    i++;
                }
                i = 0;
                countEdges = int.Parse(reader.ReadLine());
                edges = new Edge[countEdges];
                while (i != countEdges)
                {
                    var line = reader.ReadLine();
                    var tempVals = line.Split().Select(Convert.ToDouble).ToList();
                    edges[i].start = (int)tempVals[0];
                    edges[i].end = (int)tempVals[1];
                    i++;
                }
                i = 0;
                countFaces = int.Parse(reader.ReadLine());
                faces = new Face[countFaces];
                while (i != countFaces)
                {
                    var line = reader.ReadLine();
                    var tempVals = line.Split().Select(Convert.ToDouble).ToList();
                    faces[i].p = new int[3];
                    faces[i].p[0] = (int)tempVals[0];
                    faces[i].p[1] = (int)tempVals[1];
                    faces[i].p[2] = (int)tempVals[2];
                    i++;
                }
                reader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void DrawFace(Face face, ref Bitmap bmp, Color color)
        {
            Point3D[] t = new Point3D[face.p.Length];
            for (int i = 0; i < face.p.Length; i++)
            {
                t[i] = pointsToDraw[face.p[i]];
            }

            if (t[0].Y == t[1].Y && t[0].Y == t[2].Y) return; // отсеиваем дегенеративные треугольники
            if (t[0].Y > t[1].Y) Swap(ref t[0], ref t[1]); //сортировка вершин треугольника
            if (t[0].Y > t[2].Y) Swap(ref t[0], ref t[2]);
            if (t[1].Y > t[2].Y) Swap(ref t[1], ref t[2]);

            int total_height = (int)(t[2].Y - t[0].Y);
            for (int i = 0; i < total_height; i++) //построчное закрашивание треугольника
            {
                bool second_half = i > t[1].Y - t[0].Y || t[1].Y == t[0].Y;
                int segment_height = second_half ? (int)Math.Round(t[2].Y - t[1].Y) : (int)Math.Round(t[1].Y - t[0].Y);
                float alpha = (float)i / total_height;
                float beta = (float)(i - (second_half ? t[1].Y - t[0].Y : 0)) / segment_height; //тут может быть деление на 0

                Point3D A = t[0] + (t[2] - t[0]) * alpha;
                Point3D B = second_half ? t[1] + (t[2] - t[1]) * beta : t[0] + (t[1] - t[0]) * beta;

                if (A.X > B.X)
                    Swap(ref A, ref B);
                try
                {
                    for (int j = (int)Math.Round(A.X); j <= Math.Round(B.X); j++)
                    {
                        float phi = B.X == A.X ? 1 : (float)(j - A.X) / (float)(B.X - A.X);
                        Point3D P = A + (B - A) * phi;

                        int newCoordX = (int)Math.Round(P.X + defTranslationX);
                        int newCoordY = (int)Math.Round(P.Y + defTranslationY);

                        if (0 <= newCoordX && newCoordX < pb.Width &&
                            0 <= newCoordY && newCoordY < pb.Height)
                            if (zbuffer[newCoordX, newCoordY] < P.Z)
                            {
                                zbuffer[newCoordX, newCoordY] = P.Z;
                                bmp.SetPixel(newCoordX, newCoordY, color);
                            }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Проблема в заполнении zbuffer:\n" + e.Message);
                }
            }
        }
        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        /*private void output()
        {
            String str;
            using (StreamWriter outputFile = new StreamWriter(@"WriteLines.txt"))
            {
                for (int y = 0; y < pb.Height; y++)
                {
                    for (int z = 0; z < pb.Width; z++)
                    {
                        str = ((int)zbuffer[y, z]).ToString() + " ";
                        outputFile.Write(str);
                    }
                    outputFile.WriteLine();
                }
                outputFile.Close();
            }
        }*/
        public void ResizeBuffer()
        {
            zbuffer = (double[,])ResizeArray(zbuffer, new int[] { pb.Width, pb.Height });

        }
        private static Array ResizeArray(Array arr, int[] newSizes)
        {
            if (newSizes.Length != arr.Rank)
                throw new ArgumentException("arr must have the same number of dimensions " +
                                            "as there are elements in newSizes", "newSizes");

            var temp = Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
            int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
            Array.ConstrainedCopy(arr, 0, temp, 0, length);
            return temp;
        }
        public void DrawPerspective()
        {

            resultTransformMatrix.Transform(pointsToDraw);
            Bitmap bmp = new Bitmap(pb.Width, pb.Height);

            Graphics gr = Graphics.FromImage(bmp);
            // Получаем проекцию точек
            for (int i = 0; i < countPoints; i++)
            {
                // Получение координат проекции
                double xProj = focus / (focus + pointsToDraw[i].Z) * pointsToDraw[i].X;
                double yProj = focus / (focus + pointsToDraw[i].Z) * pointsToDraw[i].Y;

                // Применение смещения
                xProj += defTranslationX;
                yProj += defTranslationY;

                // Запись в массив проекции
                projPoints[i].X = (float)xProj;
                projPoints[i].Y = (float)yProj;
            }
            for (int t = 0; t < pb.Width; t++)
                for (int j = 0; j < pb.Height; j++)
                    zbuffer[t, j] = Double.NegativeInfinity;
            /*foreach (Face face in faces)
            {
                CheckFace(face);
            }*/
            //output();

            // Рисуем ребра
            for (int i = 0; i < countEdges; ++i)
                gr.DrawLine(new Pen(Color.Red, 1),
                    projPoints[edges[i].start].X, projPoints[edges[i].start].Y,
                    projPoints[edges[i].end].X, projPoints[edges[i].end].Y);

            for (int y = 0; y < pb.Width; y++)
            {
                for (int z = 0; z < pb.Height; z++)
                {
                    if (zbuffer[y, z] != Double.NegativeInfinity)
                        gr.DrawRectangle(new Pen(Color.Red, 1), y, z, 1, 1);
                }
            }
            pb.Image = bmp;
            gr.Dispose(); //освобождение памяти

            points.CopyTo(pointsToDraw, 0);
            resultTransformMatrix = new Matrix3D();
        }
        public void DrawParallel()
        {
            resultTransformMatrix.Transform(pointsToDraw);
            Bitmap bmp = new Bitmap(pb.Width, pb.Height);
            Graphics gr = Graphics.FromImage(bmp);


            for (int t = 0; t < pb.Width; t++)
                for (int j = 0; j < pb.Height; j++)
                    zbuffer[t, j] = Double.NegativeInfinity;

            foreach (Face face in faces)
            {
                Point3D[] t = new Point3D[face.p.Length];
                for (int i = 0; i < face.p.Length; i++)
                {
                    t[i] = pointsToDraw[face.p[i]];
                }
                Vector3D n = Vector3D.CrossProduct((t[2] - t[0]), (t[1] - t[0]));
                n.Normalize();
                double intensity = Vector3D.DotProduct(n, light_dir);

                if (intensity > 0)
                {
                    DrawFace(face, ref bmp, Color.FromArgb(255, (int)(intensity * 255), (int)(intensity * 255), (int)(intensity * 255)));
                }
            }

            // Рисуем ребра
            /*for (int i = 0; i < countEdges; ++i)
                gr.DrawLine(new Pen(Color.Red, 1),
                            (float)pointsToDraw[edges[i].start].X + (float)defTranslationX,
                            (float)pointsToDraw[edges[i].start].Y + (float)defTranslationY,
                            (float)pointsToDraw[edges[i].end].X + (float)defTranslationX,
                            (float)pointsToDraw[edges[i].end].Y + (float)defTranslationY);*/
            pb.Image = bmp;
            gr.Dispose(); //освобождение памяти

            points.CopyTo(pointsToDraw, 0);
            resultTransformMatrix = new Matrix3D();
        }
        public void RotateFigure(double angleX, double angleY, double angleZ)
        {
            angleX *= Math.PI / 180;
            angleY *= Math.PI / 180;
            angleZ *= Math.PI / 180;
            Matrix3D RotateXMatrix = new Matrix3D(1, 0, 0, 0,
                                                  0, Math.Cos(angleX), -Math.Sin(angleX), 0,
                                                  0, Math.Sin(angleX), Math.Cos(angleX), 0,
                                                  0, 0, 0, 1);
            Matrix3D RotateYMatrix = new Matrix3D(Math.Cos(angleY), 0, Math.Sin(angleY), 0,
                                                  0, 1, 0, 0,
                                                  -Math.Sin(angleY), 0, Math.Cos(angleY), 0,
                                                  0, 0, 0, 1);
            Matrix3D RotateZMatrix = new Matrix3D(Math.Cos(angleZ), -Math.Sin(angleZ), 0, 0,
                                                  Math.Sin(angleZ), Math.Cos(angleZ), 0, 0,
                                                  0, 0, 1, 0,
                                                  0, 0, 0, 1);
            resultTransformMatrix = Matrix3D.Multiply(resultTransformMatrix, RotateXMatrix);
            resultTransformMatrix = Matrix3D.Multiply(resultTransformMatrix, RotateYMatrix);
            resultTransformMatrix = Matrix3D.Multiply(resultTransformMatrix, RotateZMatrix);
        }
        public void Scale(int approximation)
        {
            //Применяем масштаб
            if (approximation != 0)
            {
                if (approximation > 0)
                    scale *= 1.111111;
                else
                if (scale > minScale)
                    scale /= 1.111111;
            }
            Matrix3D ScaleMatrix3D = new Matrix3D(scale, 0, 0, 0,
                                                  0, scale, 0, 0,
                                                  0, 0, scale, 0,
                                                  0, 0, 0, 1);
            resultTransformMatrix = Matrix3D.Multiply(resultTransformMatrix, ScaleMatrix3D);
        }
        public void MoveFigure(double translationX, double translationY, double translationZ)
        {
            Matrix3D TranslateMatrix3D = new Matrix3D(1, 0, 0, 0,
                                                      0, 1, 0, 0,
                                                      0, 0, 1, 0,
                                                      translationX, translationY, translationZ, 1);
            resultTransformMatrix = Matrix3D.Multiply(resultTransformMatrix, TranslateMatrix3D);
        }
        public void ChangeFigure(String nameFigure)
        {
            try
            {
                nameFigure += ".txt";
                ReadFromFile(nameFigure);
                scale = 100;
                resultTransformMatrix = new Matrix3D();
                points.CopyTo(pointsToDraw, 0);
                Scale(0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Scale(0);
            }
        }
    }
}
