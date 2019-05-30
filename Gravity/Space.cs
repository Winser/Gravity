using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gravity
{
    public class Space
    {
        public static double G = 6.67408E-11;

        public Vector Offset { get; set; }

        public double Scale { get; set; }
        public double DefaultSpeed { get; set; }
        public GObject Target { get; private set; }

        private List<GObject> GObjects;

        public Space()
        {
            GObjects = new List<GObject>();
            InitializeRealTestObjects();
        }

        private void InitializeRealTestObjects()
        {
            Scale = 0.0000005;
            DefaultSpeed = 100000;

            GObjects.Add(new GObject()
            {
                Position = new Vector(0, 0),
                Speed = new Vector(0, 0),
                Mass = 5.972 * Math.Pow(10,24)
            });
            GObjects.Add(new GObject()
            {
                Position = new Vector(362600000, 0),
                Speed = new Vector(0, 1080),
                Mass = 7.36 * Math.Pow(10, 22)
            });

            GObjects.Add(new GObject()
            {
                Position = new Vector(-362600000, 0),
                Speed = new Vector(0, -1080),
                Mass = 7.36 * Math.Pow(10, 22)
            });
        }

        public void Update(double dTime)
        {
            dTime = dTime / 1000.0 * DefaultSpeed;
            foreach (GObject gObject1 in GObjects)
            {
                gObject1.Axel = Vector.Zero;
                foreach (GObject gObject2 in GObjects)
                {
                    if (gObject1 != gObject2)
                        gObject1.UpdateAxel(gObject2);
                }
            }

            foreach (GObject gObject1 in GObjects)
            {
                gObject1.UpdateSpeed(dTime);
            }

            foreach (GObject gObject in GObjects)
            {
                gObject.UpdatePosition(dTime);
            }
        }

        public void SetTarget(Vector vector)
        {
            GObject gObject = GetObjectOnPos(vector);
            if (gObject != null)
            {
                Target = gObject;
            }
        }

        public void MouseMove(Vector pos)
        {
            pos += Offset;
            GObject target = GetObjectOnPos(pos);
            if (target != null)
            {

            }
        }

        private GObject GetObjectOnPos(Vector pos)
        {
            pos /= Scale;
            pos -= Offset;
            double dist = 5 / Scale;
            foreach (GObject gObject in GObjects)
            {
                if ((gObject.Position - pos).Length < dist)
                    return gObject;
            }

            return null;
        }

        public void Draw(Canvas canvas, DrawParams drawParams)
        {
            Offset = new Vector()
            {
                X = (canvas.ActualWidth / 2) / Scale,
                Y = (canvas.ActualHeight / 2) / Scale
            };

            foreach (GObject gObject in GObjects)
            {
                gObject.Draw(canvas, Scale, drawParams, Offset);
            }

            //Центр масс
            Vector cm = GetCenterMass();
            cm = (cm + Offset) * Scale;
            DrawEllipce(canvas, cm, 1, Brushes.Green, null);

            //Цель
            if (Target != null)
                DrawEllipce(canvas, (Target.Position + Offset) * Scale, 15, null, Brushes.Red);
        }

        private Vector GetCenterMass()
        {
            Vector cm = Vector.Zero;
            double summ_mass = 0;
            foreach (GObject gObject in GObjects)
            {
                cm += gObject.Position * gObject.Mass;
                summ_mass += gObject.Mass;
            }
            cm /= summ_mass;
            return cm;
        }

        private static void DrawEllipce(Canvas canvas, Vector pos, double r, Brush fill, Brush stroke)
        {
            Ellipse point = new Ellipse()
            {
                Width = r,
                Height = r,
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = 2
            };
            point.SetValue(Canvas.LeftProperty, pos.X - r / 2);
            point.SetValue(Canvas.TopProperty, pos.Y - r / 2);

            canvas.Children.Add(point);
        }
    }
}
