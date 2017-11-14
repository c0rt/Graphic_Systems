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

        private StreamReader reader;
        private int countPoints, countEdges;
        private Point3D[] points, pointsToDraw;
        private PointF[] projPoints;
        private Edge[] edges;

        private const double focus = 2000;
        private const double minScale = 0.1;
        private double scale, defTranslationX, defTranslationY;
        private PictureBox pb;

        

        private Matrix3D resultTransformMatrix;

        public Figure(PictureBox newPicBox)
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
            reader.Close();
            scale = 100;
            resultTransformMatrix = new Matrix3D();
            points.CopyTo(pointsToDraw, 0);
            ScaleFigure(0);
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
                double xProj = focus / (focus + pointsToDraw[i].X) * pointsToDraw[i].Y;
                double yProj = focus / (focus + pointsToDraw[i].X) * pointsToDraw[i].Z;

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
                            (float)pointsToDraw[edges[i].start].Y + (float)defTranslationY,
                            (float)pointsToDraw[edges[i].start].Z + (float)defTranslationX,
                            (float)pointsToDraw[edges[i].end].Y + (float)defTranslationY,
                            (float)pointsToDraw[edges[i].end].Z + (float)defTranslationX);
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
        public void ScaleFigure(int approximation)
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
            reader.Close();
            scale = 100;
            resultTransformMatrix = new Matrix3D();
            points.CopyTo(pointsToDraw, 0);
            ScaleFigure(0);
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
