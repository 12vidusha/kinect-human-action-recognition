using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixVector
{

    public class Vector3
    {
        public static Vector3 Zero = NewZero();
        public static Vector3 One = NewOne();

        public float X;
        public float Y;
        public float Z;

        public Vector3()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(float xyz)
        {
            this.X = xyz;
            this.Y = xyz;
            this.Z = xyz;
        }

        public Vector3(Vector3 v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public static Vector3 NewZero()
        {
            return new Vector3(0.0f);
        }

        public static Vector3 NewOne()
        {
            return new Vector3(1.0f);
        }

        public float DotProduct(Vector3 other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator *(Vector3 v, float scalar)
        {
            return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector3 operator /(Vector3 v, float scalar)
        {
            return new Vector3(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
        }

        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public Vector3 Add(Vector3 v)
        {
            X += v.X;
            Y += v.Y;
            Z += v.Z;
            return this;
        }

        public float DistanceTo(Vector3 v)
        {
            float dx = this.X - v.X;
            float dy = this.Y - v.Y;
            float dz = this.Z - v.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public float DistanceToWithoutSquare(Vector3 v)
        {
            float dx = this.X - v.X;
            float dy = this.Y - v.Y;
            float dz = this.Z - v.Z;
            return (float)(dx * dx + dy * dy + dz * dz);
        }

        public float Size()
        {
            return DistanceTo(Vector3.Zero);
        }

        public Vector3 Normalize()
        {
            float size = Size();
            this.X /= size;
            this.Y /= size;
            this.Z /= size;
            return this;
        }

        public Vector3 Clone()
        {
            return new Vector3(this);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
        public static Vector3 findOrthonormalVector(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }
        /// <summary>
        /// Gets degree angle between a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double signedAngle(Vector3 a, Vector3 b)
        {
            double cos = a.DotProduct(b)/(Math.Sqrt(a.X*a.X+a.Y*a.Y+a.Z*a.Z)*Math.Sqrt(b.X*b.X+b.Y*b.Y+b.Z*b.Z));
            double angle=Math.Acos(cos)*180/Math.PI;
            double crossProduct = a.Y * b.Z - a.Z * b.Y+ a.Z * b.X - a.X * b.Z+ a.X * b.Y - a.Y * b.X;
            if (crossProduct < 0)
            {
                angle = -angle;
            }
            return angle;
        }
        public static double GetAngle(Vector3 a, Vector3 b)
        {
            double cos = a.DotProduct(b) / (Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z) * Math.Sqrt(b.X * b.X + b.Y * b.Y + b.Z * b.Z));
            double angle = Math.Acos(cos) * 180 / Math.PI;
            return angle;
        }
    }
}
