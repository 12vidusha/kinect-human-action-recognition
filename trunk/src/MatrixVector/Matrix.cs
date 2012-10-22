using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixVector
{
    public class Matrix
    {
        public float[,] matrix;
        public int rows;
        public int cols;

        public Matrix(int rows, int cols)
        {
            this.matrix = new float[rows, cols];
            this.rows = rows;
            this.cols = cols;
        }

        public Matrix(float[,] matrix)
        {
            this.matrix = matrix;
            this.rows = matrix.GetLength(0);
            this.cols = matrix.GetLength(1);
        }

        protected static float[,] Multiply(Matrix matrix, float scalar)
        {
            int rows = matrix.rows;
            int cols = matrix.cols;
            float[,] m1 = matrix.matrix;
            float[,] m2 = new float[rows, cols];
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    m2[i, j] = m1[i, j] * scalar;
                }
            }
            return m2;
        }

        protected static float[,] Multiply(Matrix matrix1, Matrix matrix2)
        {
            int m1rows = matrix1.rows;
            int m1cols = matrix1.cols;
            int m2rows = matrix2.rows;
            int m2cols = matrix2.cols;
            if (m1cols != m2rows)
            {
                throw new ArgumentException();
            }
            float[,] m1 = matrix1.matrix;
            float[,] m2 = matrix2.matrix;
            float[,] m3 = new float[m1rows, m2cols];
            for (int i = 0; i < m1rows; ++i)
            {
                for (int j = 0; j < m2cols; ++j)
                {
                    float sum = 0;
                    for (int it = 0; it < m1cols; ++it)
                    {
                        sum += m1[i, it] * m2[it, j];
                    }
                    m3[i, j] = sum;
                }
            }
            return m3;
        }

        public static Matrix operator *(Matrix m, float scalar)
        {
            return new Matrix(Multiply(m, scalar));
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return new Matrix(Multiply(m1, m2));
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < rows; ++i)
            {
                if (i > 0)
                {
                    res += "|";
                }
                for (int j = 0; j < cols; ++j)
                {
                    if (j > 0)
                    {
                        res += ",";
                    }
                    res += matrix[i, j];
                }
            }
            return "(" + res + ")";
        }
    }
}
