using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixVector
{
    public class Matrix3 : Matrix
    {
        public Matrix3()
            : base(3, 3)
        {
        }

        public Matrix3(float[,] matrix)
            : base(matrix)
        {
            if (rows != 3 || cols != 3)
            {
                throw new ArgumentException();
            }
        }
        public static double Determinant(Matrix3 matrix)
        {
            float[,] m = matrix.matrix;
            double deter = 0;
            deter = m[0, 0] * m[1, 1] * m[2, 2] + m[0, 1] * m[1, 2] * m[2, 1] +
                m[0, 2] * m[1, 0] * m[2, 1] - m[0, 0] * m[1, 2] * m[2, 1]
                - m[0, 1] * m[1, 0] * m[2, 2] - m[0, 2] * m[1, 1] * m[2, 0];
            return deter;
        }
        public static Matrix3 I()
        {
            return new Matrix3(new float[,] { 
        { 1.0f, 0.0f, 0.0f }, 
        { 0.0f, 1.0f, 0.0f }, 
        { 0.0f, 0.0f, 1.0f } });
        }

        public static Vector3 operator *(Matrix3 matrix3, Vector3 v)
        {
            float[,] m = matrix3.matrix;
            return new Vector3(
                m[0, 0] * v.X + m[0, 1] * v.Y + m[0, 2] * v.Z,
                m[1, 0] * v.X + m[1, 1] * v.Y + m[1, 2] * v.Z,
                m[2, 0] * v.X + m[2, 1] * v.Y + m[2, 2] * v.Z);
        }
        public static Vector3 operator *(Vector3 v, Matrix3 matrix3)
        {
            float[,] m = matrix3.matrix;
            return new Vector3(
                m[0, 0] * v.X + m[1, 0] * v.Y + m[2, 0] * v.Z,
                m[0, 1] * v.X + m[1, 1] * v.Y + m[2, 1] * v.Z,
                m[0, 2] * v.X + m[1, 2] * v.Y + m[2, 2] * v.Z);
        }

        public static Matrix3 operator *(Matrix3 mat1, Matrix3 mat2)
        {
            float[,] m1 = mat1.matrix;
            float[,] m2 = mat2.matrix;
            float[,] m3 = new float[3, 3];
            m3[0, 0] = m1[0, 0] * m2[0, 0] + m1[0, 1] * m2[1, 0] + m1[0, 2] * m2[2, 0];
            m3[0, 1] = m1[0, 0] * m2[0, 1] + m1[0, 1] * m2[1, 1] + m1[0, 2] * m2[2, 1];
            m3[0, 2] = m1[0, 0] * m2[0, 2] + m1[0, 1] * m2[1, 2] + m1[0, 2] * m2[2, 2];
            m3[1, 0] = m1[1, 0] * m2[0, 0] + m1[1, 1] * m2[1, 0] + m1[1, 2] * m2[2, 0];
            m3[1, 1] = m1[1, 0] * m2[0, 1] + m1[1, 1] * m2[1, 1] + m1[1, 2] * m2[2, 1];
            m3[1, 2] = m1[1, 0] * m2[0, 2] + m1[1, 1] * m2[1, 2] + m1[1, 2] * m2[2, 2];
            m3[2, 0] = m1[2, 0] * m2[0, 0] + m1[2, 1] * m2[1, 0] + m1[2, 2] * m2[2, 0];
            m3[2, 1] = m1[2, 0] * m2[0, 1] + m1[2, 1] * m2[1, 1] + m1[2, 2] * m2[2, 1];
            m3[2, 2] = m1[2, 0] * m2[0, 2] + m1[2, 1] * m2[1, 2] + m1[2, 2] * m2[2, 2];
            return new Matrix3(m3);
        }

        public static Matrix3 operator *(Matrix3 m, float scalar)
        {
            return new Matrix3(Multiply(m, scalar));
        }
    }
}
