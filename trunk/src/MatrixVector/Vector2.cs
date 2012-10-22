using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixVector
{
    public class Vector2{
        public float X;
        public float Y;
        public Vector2()
        {
            X = 0.0f;
            Y = 0.0f;
        }
        public Vector2(float xyz)
        {
            this.X = xyz;
            this.Y = xyz;
        }
        public Vector2(float X, float Y){
            this.X=X;
            this.Y=Y;
        }
        public Vector2(Vector2 v)
        {
            this.X = v.X;
            this.Y = v.Y;
        }
        public static double signedAngle(Vector2 v1, Vector2 v2)
        {
           /* float perpDot = v1.X * v3.Y - v1.Y * v3.X;
            return (float)Math.Atan2(perpDot, v1.Dot(v3));*/
            double a = v1.X * v1.X + v1.Y * v1.Y;
            double b = v2.X * v2.X + v2.Y * v2.Y;
            if (a == 0) a = 1;
            if (b == 0) b = 1;
            double cos = (v1.X * v2.X + v1.Y * v2.Y) / (Math.Sqrt(a) * Math.Sqrt(b));
            double angle = Math.Acos(cos) * 180 / Math.PI;
            double crossProduct = v1.X * v2.Y - v1.Y * v2.X;
            if (crossProduct < 0)
            {
                angle = -angle;
            }
            if (double.IsNaN(angle))
            {
                angle = v1.X * v2.X + v1.Y * v2.Y;
            }
            return angle;

        }
        public float Dot(Vector2 other)
        {
            return X * other.X + Y * other.Y;
        }
         public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.Y, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        public static Vector2 operator *(Vector2 v, float scalar)
        {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }

        public static Vector2 operator /(Vector2 v, float scalar)
        {
            return new Vector2(v.X / scalar, v.Y / scalar);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y;
        }
        public Vector2 Clone()
        {
            return new Vector2(this);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
    

    

    

    
}
