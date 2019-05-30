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
        public static double G = 1;
        public static double Speed = 1;
       
        public double Scale { get; set; } = 1;
        private List<GObject> GObjects;

        public Space()
        {
            GObjects = new List<GObject>();
            InitializeRealTestObjects();
        }

        private void InitializeRealTestObjects()
        {
            G = 6.67408 * Math.Pow(10, -11);
            Scale = 0.0000005;
            Speed = 10000;

            GObjects.Add(new GObject()
            {
                Position = new Vector(0, 0),
                Speed = new Vector(0, 0),
                Mass = 5.972 * Math.Pow(10,24)
            });
            GObjects.Add(new GObject()
            {
                Position = new Vector(374400000, 0),
                Speed = new Vector(0, 1080),
                Mass = 7.36 * Math.Pow(10, 22)
            });

            GObjects.Add(new GObject()
            {
                Position = new Vector(-374400000, 0),
                Speed = new Vector(0, -1080),
                Mass = 7.36 * Math.Pow(10, 22)
            });
        }

        public void Update()
        {
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
                gObject1.UpdateSpeed();
            }

            foreach (GObject gObject in GObjects)
            {
                gObject.UpdatePosition();
            }
        }
        public void Draw(Canvas canvas, DrawParams drawParams)
        {

            Vector offset = new Vector()
            {
                X = (canvas.ActualWidth / 2) / Scale,
                Y = (canvas.ActualHeight / 2) / Scale
            };

            foreach (GObject gObject in GObjects)
            {
                gObject.Draw(canvas, Scale, drawParams, offset);
            }

            //Центр масс
            Vector cm = Vector.Zero;
            double summ_mass = 0;
            foreach (GObject gObject in GObjects)
            {
                cm += gObject.Position * gObject.Mass;
                summ_mass += gObject.Mass;
            }

            cm /= summ_mass;

            cm += offset;

            Ellipse point = new Ellipse()
            {
                Width = 5,
                Height = 5,
                Fill = Brushes.Green
            };

            point.SetValue(Canvas.LeftProperty, cm.X * Scale - 5 / 2);
            point.SetValue(Canvas.TopProperty, cm.Y * Scale - 5 / 2);

            canvas.Children.Add(point);
        }

    }
}
