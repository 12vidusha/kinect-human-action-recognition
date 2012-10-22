using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixVector
{
    public static class RotationMatrix
    {
        public static Matrix41 GetRotationMatrixX(double angle)
        {
            if (angle == 0.0)
            {
                return Matrix41.I;
            }
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix41(new float[4, 4] {
        { 1.0f, 0.0f, 0.0f, 0.0f }, 
        { 0.0f, cos, -sin, 0.0f }, 
        { 0.0f, sin, cos, 0.0f }, 
        { 0.0f, 0.0f, 0.0f, 1.0f } });
        }
        public static Matrix3 GetRotationMatrix3X(double angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix3(new float[3, 3] {
        { 1.0f, 0.0f, 0.0f}, 
        { 0.0f, cos, -sin }, 
        { 0.0f, sin, cos}});
        }
        public static Matrix3 GetRotationMatrix3Z(double angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix3(new float[3, 3] {
        { cos, -sin, 0.0f}, 
        { sin, cos, 0.0f }, 
        { 0.0f, 0.0f, 1.0f}});
        }
        public static Matrix3 GetRotationMatrix3Y(double angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix3(new float[3, 3] {
        { cos, 0.0f, sin}, 
        { 0.0f, 1.0f, 0.0f }, 
        { -sin, 0.0f, cos}});
        }
        public static Matrix3 GetRotationMatrix3Y(double angle, Vector3 u)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix3(new float[3, 3] {
        { cos+u.X*u.X*(1-cos), u.X*u.Y*(1-cos)-u.Z*sin, u.X*u.Z*(1-cos)+u.Y*sin}, 
        { u.Y*u.X*(1-cos)+u.Z*sin, cos+u.Y*u.Y*(1-cos) , u.Y*u.Z*(1-cos)-u.X*sin }, 
        { u.Z*u.X*(1-cos)-u.Y*sin, u.Z*u.Y*(1-cos)+u.X*sin, cos+u.Z*u.Z*(1-cos)}});
        }
        public static Matrix41 GetRotationMatrixY(double angle)
        {
            if (angle == 0.0)
            {
                return Matrix41.I;
            }
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix41(new float[4, 4] {
        { cos, 0.0f, sin, 0.0f }, 
        { 0.0f, 1.0f, 0.0f, 0.0f }, 
        { -sin, 0.0f, cos, 0.0f }, 
        { 0.0f, 0.0f, 0.0f, 1.0f } });
        }

        public static Matrix41 GetRotationMatrixZ(double angle)
        {
            if (angle == 0.0)
            {
                return Matrix41.I;
            }
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix41(new float[4, 4] {
        { cos, -sin, 0.0f, 0.0f }, 
        { sin, cos, 0.0f, 0.0f }, 
        { 0.0f, 0.0f, 1.0f, 0.0f }, 
        { 0.0f, 0.0f, 0.0f, 1.0f } });
        }

        public static Matrix41 GetRotationMatrix(double ax, double ay, double az)
        {
            Matrix41 my = null;
            Matrix41 mz = null;
            Matrix41 result = null;
            if (ax != 0.0)
            {
                result = GetRotationMatrixX(ax);
            }
            if (ay != 0.0)
            {
                my = GetRotationMatrixY(ay);
            }
            if (az != 0.0)
            {
                mz = GetRotationMatrixZ(az);
            }
            if (my != null)
            {
                if (result != null)
                {
                    result *= my;
                }
                else
                {
                    result = my;
                }
            }
            if (mz != null)
            {
                if (result != null)
                {
                    result *= mz;
                }
                else
                {
                    result = mz;
                }
            }
            if (result != null)
            {
                return result;
            }
            else
            {
                return Matrix41.I;
            }
        }

        public static Matrix41 GetRotationMatrix(Vector3 axis, double angle)
        {
            if (angle == 0.0)
            {
                return Matrix41.I;
            }

            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            float xx = x * x;
            float yy = y * y;
            float zz = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            float[,] matrix = new float[4, 4];

            matrix[0, 0] = xx + (1 - xx) * cos;
            matrix[1, 0] = xy * (1 - cos) + z * sin;
            matrix[2, 0] = xz * (1 - cos) - y * sin;
            matrix[3, 0] = 0.0f;

            matrix[0, 1] = xy * (1 - cos) - z * sin;
            matrix[1, 1] = yy + (1 - yy) * cos;
            matrix[2, 1] = yz * (1 - cos) + x * sin;
            matrix[3, 1] = 0.0f;

            matrix[0, 2] = xz * (1 - cos) + y * sin;
            matrix[1, 2] = yz * (1 - cos) - x * sin;
            matrix[2, 2] = zz + (1 - zz) * cos;
            matrix[3, 2] = 0.0f;

            matrix[3, 0] = 0.0f;
            matrix[3, 1] = 0.0f;
            matrix[3, 2] = 0.0f;
            matrix[3, 3] = 1.0f;

            return new Matrix41(matrix);
        }

        /// <param name="source">Should be normalized</param>
        /// <param name="destination">Should be normalized</param>
        public static Matrix41 GetRotationMatrix(Vector3 source, Vector3 destination)
        {
            Vector3 rotaxis = Vector3.CrossProduct(source, destination);
            if (rotaxis != Vector3.Zero)
            {
                rotaxis.Normalize();
                double angle = GetRotationAngle(source, destination);
                return GetRotationMatrix(rotaxis, angle);
            }
            else
            {
                return Matrix41.I;
            }
        }

        public static Vector3 GetRotationAxis(Vector3 source, Vector3 destination)
        {
            Vector3 rotaxis = Vector3.CrossProduct(source, destination);
            return rotaxis;
        }
        /// <param name="source">Should be normalized</param>
        /// <param name="destination">Should be normalized</param>
        public static double GetRotationAngle(Vector3 source, Vector3 destination)
        {
                double cos = (source.DotProduct(destination))/(Math.Sqrt(source.X*source.X+source.Y*source.Y+source.Z*source.Z)*Math.Sqrt(destination.X*destination.X+destination.Y*destination.Y+destination.Z*destination.Z));
                double angle = Math.Acos(cos);
                return angle;
        }
        public static double getAngleXY(Vector3 source)
        {
            double angle = Math.Atan(source.X/ source.Y);
            return angle;
        }
        public static double getAngleXZ(Vector3 source)
        {
            double angle = Math.Atan(source.X / source.Z);
            return angle;
        }
        public static double getAngleYZ(Vector3 source)
        {
            double angle = Math.Atan(source.Z / source.Y);
            return angle;
        }
    }
}
