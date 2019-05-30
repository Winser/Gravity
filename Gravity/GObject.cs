using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gravity
{
    internal class GObject
    {
        public Vector Position;
        public Vector Speed;
        public Vector Axel;
        public double Mass;


        public GObject(Vector position, Vector speed, double mass)
        {
            Position = position;
            Speed = speed;
            Mass = mass;
        }
        public GObject()
        {
            Position = new Vector(0, 0);
            Speed = new Vector(0, 0);
            Mass = 0;
        }
        public void UpdateAxel(GObject gObject)
        {
            Vector toTarget = gObject.Position - this.Position;
            double sqrDistance = toTarget.X * toTarget.X + toTarget.Y * toTarget.Y;
            double axel = Space.G * (gObject.Mass / sqrDistance);

            Axel += toTarget.Normalize * axel;
        }
        public void UpdateSpeed()
        {
            Speed += Axel * Space.Speed;
        }
        public void UpdatePosition()
        {
            Position += Speed * Space.Speed;
        }
        public void Draw(Canvas canvas, double scale, DrawParams drawParams, Vector offset)
        {
            Vector drawPos = (Position + offset) * scale;

            double r = 5;
            Ellipse ellipse = new Ellipse()
            {
                Width = r,
                Height = r,
                Fill = Brushes.White
            };

            ellipse.SetValue(Canvas.LeftProperty, drawPos.X - r / 2);
            ellipse.SetValue(Canvas.TopProperty, drawPos.Y - r / 2);
            canvas.Children.Add(ellipse);

            //Скорость
            if (drawParams.DrawSpeed)
            {
                Line line = new Line()
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 2,
                    X1 = drawPos.X,
                    Y1 = drawPos.Y,
                    X2 = drawPos.X + Speed.Normalize.X * 10,
                    Y2 = drawPos.Y + Speed.Normalize.Y * 10
                };

                TextBlock text = new TextBlock()
                {
                    Background = Brushes.Black,
                    Foreground = Brushes.White,
                    Text = Speed.Length.ToString("N4")
                };

                text.SetValue(Canvas.LeftProperty, drawPos.X + r);
                text.SetValue(Canvas.TopProperty, drawPos.Y + r);

                canvas.Children.Add(line);
                canvas.Children.Add(text);
            }

            //Ускорение
            if (drawParams.DrawAxel)
            {
                Line line = new Line()
                {
                    Stroke = Brushes.Orange,
                    StrokeThickness = 2,
                    X1 = drawPos.X,
                    Y1 = drawPos.Y,
                    X2 = drawPos.X + Axel.Normalize.X * 10,
                    Y2 = drawPos.Y + Axel.Normalize.Y * 10
                };
                canvas.Children.Add(line);
            }

            //Масса
            {
                TextBlock text = new TextBlock()
                {
                    Background = Brushes.Black,
                    Foreground = Brushes.White,
                    Text = Mass.ToString()
                };

                text.SetValue(Canvas.LeftProperty, drawPos.X + r);
                text.SetValue(Canvas.TopProperty, drawPos.Y - 20);
                canvas.Children.Add(text);
            }
        }

    }
}