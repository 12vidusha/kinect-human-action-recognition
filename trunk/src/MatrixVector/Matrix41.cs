using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixVector
{
    public class Matrix41 : Matrix
    {
        public static Matrix41 I = NewI();

        public Matrix41()
            : base(4, 4)
        {
        }

        public Matrix41(float[,] matrix)
            : base(matrix)
        {
            if (rows != 4 || cols != 4)
            {
                throw new ArgumentException();
            }
        }

        public static Matrix41 NewI()
        {
            return new Matrix41(new float[,] { 
        { 1.0f, 0.0f, 0.0f, 0.0f }, 
        { 0.0f, 1.0f, 0.0f, 0.0f }, 
        { 0.0f, 0.0f, 1.0f, 0.0f },
        { 0.0f, 0.0f, 0.0f, 1.0f } });
        }

        public static Vector3 operator *(Matrix41 matrix4, Vector3 v)
        {
            float[,] m = matrix4.matrix;
            float w = m[3, 0] * v.X + m[3, 1] * v.Y + m[3, 2] * v.Z + m[3, 3];
            return new Vector3(
                (m[0, 0] * v.X + m[0, 1] * v.Y + m[0, 2] * v.Z + m[0, 3]) / w,
                (m[1, 0] * v.X + m[1, 1] * v.Y + m[1, 2] * v.Z + m[1, 3]) / w,
                (m[2, 0] * v.X + m[2, 1] * v.Y + m[2, 2] * v.Z + m[2, 3]) / w
                );
        }

        public static Matrix41 operator *(Matrix41 mat1, Matrix41 mat2)
        {
            float[,] m1 = mat1.matrix;
            float[,] m2 = mat2.matrix;
            float[,] m3 = new float[4, 4];
            m3[0, 0] = m1[0, 0] * m2[0, 0] + m1[0, 1] * m2[1, 0] + m1[0, 2] * m2[2, 0] + m1[0, 3] * m2[3, 0];
            m3[0, 1] = m1[0, 0] * m2[0, 1] + m1[0, 1] * m2[1, 1] + m1[0, 2] * m2[2, 1] + m1[0, 3] * m2[3, 1];
            m3[0, 2] = m1[0, 0] * m2[0, 2] + m1[0, 1] * m2[1, 2] + m1[0, 2] * m2[2, 2] + m1[0, 3] * m2[3, 2];
            m3[0, 3] = m1[0, 0] * m2[0, 3] + m1[0, 1] * m2[1, 3] + m1[0, 2] * m2[2, 3] + m1[0, 3] * m2[3, 3];
            m3[1, 0] = m1[1, 0] * m2[0, 0] + m1[1, 1] * m2[1, 0] + m1[1, 2] * m2[2, 0] + m1[1, 3] * m2[3, 0];
            m3[1, 1] = m1[1, 0] * m2[0, 1] + m1[1, 1] * m2[1, 1] + m1[1, 2] * m2[2, 1] + m1[1, 3] * m2[3, 1];
            m3[1, 2] = m1[1, 0] * m2[0, 2] + m1[1, 1] * m2[1, 2] + m1[1, 2] * m2[2, 2] + m1[1, 3] * m2[3, 2];
            m3[1, 3] = m1[1, 0] * m2[0, 3] + m1[1, 1] * m2[1, 3] + m1[1, 2] * m2[2, 3] + m1[1, 3] * m2[3, 3];
            m3[2, 0] = m1[2, 0] * m2[0, 0] + m1[2, 1] * m2[1, 0] + m1[2, 2] * m2[2, 0] + m1[2, 3] * m2[3, 0];
            m3[2, 1] = m1[2, 0] * m2[0, 1] + m1[2, 1] * m2[1, 1] + m1[2, 2] * m2[2, 1] + m1[2, 3] * m2[3, 1];
            m3[2, 2] = m1[2, 0] * m2[0, 2] + m1[2, 1] * m2[1, 2] + m1[2, 2] * m2[2, 2] + m1[2, 3] * m2[3, 2];
            m3[2, 3] = m1[2, 0] * m2[0, 3] + m1[2, 1] * m2[1, 3] + m1[2, 2] * m2[2, 3] + m1[2, 3] * m2[3, 3];
            m3[3, 0] = m1[3, 0] * m2[0, 0] + m1[3, 1] * m2[1, 0] + m1[3, 2] * m2[2, 0] + m1[3, 3] * m2[3, 0];
            m3[3, 1] = m1[3, 0] * m2[0, 1] + m1[3, 1] * m2[1, 1] + m1[3, 2] * m2[2, 1] + m1[3, 3] * m2[3, 1];
            m3[3, 2] = m1[3, 0] * m2[0, 2] + m1[3, 1] * m2[1, 2] + m1[3, 2] * m2[2, 2] + m1[3, 3] * m2[3, 2];
            m3[3, 3] = m1[3, 0] * m2[0, 3] + m1[3, 1] * m2[1, 3] + m1[3, 2] * m2[2, 3] + m1[3, 3] * m2[3, 3];
            return new Matrix41(m3);
        }

        public static Matrix41 operator *(Matrix41 m, float scalar)
        {
            return new Matrix41(Multiply(m, scalar));
        }
    }
}
