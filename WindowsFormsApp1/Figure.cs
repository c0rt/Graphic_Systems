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

        public Figure(PictureBox newPicBox)
        {
            try
            {
                pb = newPicBox;

                defTranslationX = pb.Width / 2;
                defTranslationY = pb.Height / 2;

                reader = new StreamReader(@"Икосаэдр.txt");
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
                scale = 100;
                resultTransformMatrix = new Matrix3D();
                points.CopyTo(pointsToDraw, 0);
                Scale(0);

                zbuffer = new double[pb.Height, pb.Width];
                for (i = 0; i < pb.Height; i++)
                    for (int j = 0; j < pb.Width; j++)
                        zbuffer[i, j] = Double.NegativeInfinity;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void CheckFace(Face face)
        {
            Point3D t0 = pointsToDraw[face.p[0]], t1 = pointsToDraw[face.p[1]], t2 = pointsToDraw[face.p[2]];

            if (t0.Y == t1.Y && t0.Y == t2.Y) return; // отсеиваем дегенеративные треугольники
            if (t0.Y > t1.Y) Swap(ref t0, ref t1);
            if (t0.Y > t2.Y) Swap(ref t0, ref t2);
            if (t1.Y > t2.Y) Swap(ref t1, ref t2);
            
            int total_height = (int)(t2.Y - t0.Y);
            for (int i = 0; i < total_height; i++)
            {
                bool second_half = i > t1.Y - t0.Y || t1.Y == t0.Y;
                int segment_height = second_half ? (int)(t2.Y - t1.Y) : (int)(t1.Y - t0.Y);
                float alpha = (float)i / total_height;
                float beta = (float)(i - (second_half ? t1.Y - t0.Y : 0)) / segment_height; //тут может быть деление на 0

                Point3D A = t0 + (t2 - t0) * alpha;
                Point3D B = second_half ? t1 + (t2 - t1) * beta : t0 + (t1 - t0) * beta;

                if (A.X > B.X)
                    Swap(ref A, ref B);
                try
                {
                    for (int j = (int)A.X; j <= B.X; j++)
                    {
                        float phi = B.X == A.X ? 1 : (float)(j - A.X) / (float)(B.X - A.X);
                        Point3D P = A + (B - A) * phi;

                        if (zbuffer[(int)(P.X + defTranslationX), (int)(P.Y + defTranslationY)] < P.Z)
                        {
                            zbuffer[(int)(P.X + defTranslationX), (int)(P.Y + defTranslationY)] = P.Z;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        static void Swap<T>(ref T lhs, ref  T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        /*private void PutTriangle(Face face)
        {
            int ymax, ymin, ysc, e1, e, i;
            int[] x = new int [3], y = new int[3];
            //Заносим x,y из face в массивы для последующей работы с ними
            for (i = 0; i < 3; i++)
            {
                x[i] = (int)pointsToDraw[face.p[i]].X;
                y[i] = (int)pointsToDraw[face.p[i]].Y;
            }
               
            
            //Определяем максимальный и минимальный y
            ymax = ymin = y[0];

            if (ymax < y[1])
                ymax = y[1];
            else if (ymin > y[1])
                ymin = y[1];

            if (ymax < y[2])
                ymax = y[2];
            else if (ymin > y[2])
                ymin = y[2];

            ymin = (ymin < 0) ? 0 : ymin;
            ymax = (ymax < pb.Height) ? ymax : pb.Height;
            bool ne;
            int x1, x2, xsc1, xsc2;
            double z1, z2, tc, z;
            //Следующий участок кода перебирает все строки сцены
            //и определяет глубину каждого пикселя
            //для соответствующего треугольника
            for (ysc = ymin; ysc < ymax; ysc++)
            {
                ne = false;
                for (e = 0; e < 3; e++)
                {
                    e1 = e + 1;

                    if (e1 == 3)
                        e1 = 0;

                    if (y[e] < y[e1])
                    {
                        if (y[e1] <= ysc || ysc < y[e]) continue;
                    }
                    else if (y[e] > y[e1])
                    {
                        if (y[e1] > ysc || ysc >= y[e]) continue;
                    }
                    else continue;

                    tc = (double)(y[e] - ysc) / (y[e] - y[e1]);
                    if (ne)
                    {
                        x2 = x[e] + (int)(tc * (x[e1] - x[e]));
                        z2 = pointsToDraw[face.p[e]].Z + tc * (pointsToDraw[face.p[e1]].Z - pointsToDraw[face.p[e]].Z);
                    }

                    else
                    {
                        x1 = x[e] + (int)(tc * (x[e1] - x[e]));
                        z1 = pointsToDraw[face.p[e]].Z + tc * (pointsToDraw[face.p[e1]].Z - pointsToDraw[face.p[e]].Z);
                        ne = true;
                    }
                }
                if (x2 < x1) {
                    e = x1;
                    x1 = x2;
                    x2 = e;
                    tc = z1;
                    z1 = z2;
                    z2 = tc;
                }
                xsc1 = (x1 < 0) ? 0 : x1;
                xsc2 = (x2 < sX) ? x2 : sX;
                for (int xsc = xsc1; xsc < xsc2; xsc++)
                {
                    tc = double(x1 - xsc) / (x1 - x2);
                    z = z1 + tc * (z2 - z1);
                    //Если полученная глубина пиксела меньше той,
                    //что находится в Z-Буфере - заменяем храняшуюся на новую.
                    if (z < (*(buff[ysc] + xsc)).z)
                        (*(buff[ysc] + xsc)).color = face.color,
				(*(buff[ysc] + xsc)).z = z;
                }
            }
        }*/
        private void output()
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
            for (int t = 0; t < pb.Height; t++)
                for (int j = 0; j < pb.Width; j++)
                    zbuffer[t, j] = Double.NegativeInfinity;
            foreach (Face face in faces)
            {
                CheckFace(face);
            }
            //output();

            // Рисуем ребра
            for (int i = 0; i < countEdges; ++i)
                gr.DrawLine(new Pen(Color.Red, 1),
                    projPoints[edges[i].start].X, projPoints[edges[i].start].Y,
                    projPoints[edges[i].end].X, projPoints[edges[i].end].Y);

            for (int y = 0; y < pb.Height; y++)
            {
                for (int z = 0; z < pb.Width; z++)
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
            // Рисуем ребра
            for (int i = 0; i < countEdges; ++i)
                gr.DrawLine(new Pen(Color.Red, 1),
                            (float)pointsToDraw[edges[i].start].X + (float)defTranslationX,
                            (float)pointsToDraw[edges[i].start].Y + (float)defTranslationY,
                            (float)pointsToDraw[edges[i].end].X + (float)defTranslationX,
                            (float)pointsToDraw[edges[i].end].Y + (float)defTranslationY);

            for (int t = 0; t < pb.Height; t++)
                for (int j = 0; j < pb.Width; j++)
                    zbuffer[t, j] = Double.NegativeInfinity;
            /*foreach (Face face in faces)
            {
                CheckFace(face);
            }*/
            CheckFace(faces[0]);
            //output();
            for (int y = 0; y < pb.Height; y++)
            {
                for (int z = 0; z < pb.Width; z++)
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
        /*public void DrawFigurePerspective()
        {
            Bitmap bmp = new Bitmap(pb.Width, pb.Height);
            Graphics gr = Graphics.FromImage(bmp);
            // Получаем проекцию точек
            for (int i = 0; i < countPoints; i++)
            {
                // Получение координат проекции
                double xProj = focus / (focus + points[i].X) * points[i].Y;
                double yProj = focus / (focus + points[i].X) * points[i].Z;

                // Применение масштаба и смещения
                xProj = xProj + defTranslationX;
                yProj = yProj + defTranslationY;

                // Запись в массив проекции
                projPoints[i].X = (float)xProj;
                projPoints[i].Y = (float)yProj;
            }
            // Рисуем ребра
            for (int i = 0; i < countEdges; ++i)
                gr.DrawLine(new Pen(Color.Red, 1), projPoints[edges[i].start].X, projPoints[edges[i].start].Y,
                    projPoints[edges[i].end].X, projPoints[edges[i].end].Y);
            pb.Image = bmp;
            gr.Dispose(); //освобождение памяти
        }*/
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
                reader = new StreamReader(nameFigure);
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
                countFaces = int.Parse(reader.ReadLine());
                faces = new Face[countFaces];
                while (i != countFaces)
                {
                    var line = reader.ReadLine();
                    var tempVals = line.Split().Select(Convert.ToDouble).ToList();
                    faces[i].p[0] = (int)tempVals[0];
                    faces[i].p[1] = (int)tempVals[1];
                    faces[i].p[2] = (int)tempVals[2];
                    i++;
                }
                reader.Close();
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
        /*public void ChangeScale(int approximation)
        {
            if (approximation > 0)
                scale += 10;
            else
                if (scale > minScale)
                scale -= 10;
            DrawFigure(points);
        }*/

        /*public void RotateFigureX(double value)
        {
            deltaAngleX = value - currentAngleX;
            deltaAngleX *= Math.PI / 180;
            for (int i = 0; i < countPoints; i++)
            {
                double x = points[i].x * 1 + points[i].y * 0 + points[i].z * 0;
                double y = points[i].x * 0 + points[i].y * Math.Cos(deltaAngleX) + points[i].z * -Math.Sin(deltaAngleX);
                double z = points[i].x * 0 + points[i].y * Math.Sin(deltaAngleX) + points[i].z * Math.Cos(deltaAngleX);
                points[i].x = x;
                points[i].y = y;
                points[i].z = z;
            }
            DrawFigure();
            currentAngleX = value;

        }*/
        /*public void RotateFigureY(int value)
        {
            deltaAngleY = value - currentAngleY;
            deltaAngleY *= Math.PI / 180;
            for (int i = 0; i < countPoints; i++)
            {
                double x = points[i].x * Math.Cos(deltaAngleY) + points[i].y * 0 + points[i].z * Math.Sin(deltaAngleY);
                double y = points[i].x * 0 + points[i].y * 1 + points[i].z * 0;
                double z = points[i].x * -Math.Sin(deltaAngleY) + points[i].y * 0 + points[i].z * Math.Cos(deltaAngleY);
                points[i].x = x;
                points[i].y = y;
                points[i].z = z;
            }
            DrawFigure();
            currentAngleY = value;
        }*/
        /*public void RotateFigureZ(int value)
        {
            deltaAngleZ = value - currentAngleZ;
            deltaAngleZ *= Math.PI / 180;
            for (int i = 0; i < countPoints; i++)
            {
                double x = points[i].x * Math.Cos(deltaAngleZ) + points[i].y * -Math.Sin(deltaAngleZ) + points[i].z * 0;
                double y = points[i].x * Math.Sin(deltaAngleZ) + points[i].y * Math.Cos(deltaAngleZ) + points[i].z * 0;
                double z = points[i].x * 0 + points[i].y * 0 + points[i].z * 1;
                points[i].x = x;
                points[i].y = y;
                points[i].z = z;
            }
            DrawFigure();
            currentAngleZ = value;
        }*/
    }
}
