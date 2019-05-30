using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gravity
{
    public struct Vector
    {
        public static Vector Zero = new Vector(0, 0);
        public double X;
        public double Y;

        public double Length
        {
            get => Math.Sqrt(X * X + Y * Y);
        }
        public Vector Normalize
        {
            get
            {
                double inv_length = (1 / Length);
                if (double.IsInfinity(inv_length))
                    return Vector.Zero;
                return new Vector(X * inv_length, Y * inv_length);
            }
        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector operator *(Vector v1, double a)
        {
            return new Vector(v1.X * a, v1.Y * a);
        }

        public static Vector operator /(Vector v1, double a)
        {
            return new Vector(v1.X / a, v1.Y / a);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y);
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return (v1.X != v2.X) || (v1.Y != v2.Y);
        }


    }
}
