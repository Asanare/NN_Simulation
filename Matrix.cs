using System.Collections;
using UnityEngine;
using System;
[System.Serializable]
public class Matrix
{
    public float[,] matrix;
    private int _rows, _cols;


    public Matrix(int rows, int cols)
    {
        _rows = rows;
        _cols = cols;
        matrix = new float[rows, cols];
    }

    public float this[int row, int col]
    {
        get { return matrix[row, col]; }
        set { matrix[row, col] = value; }
    }

    /// <summary>
    /// Multiplies 2 given matrices
    /// </summary>
    /// <param name="mat1">First Matrix</param>
    /// <param name="mat2">Second Matrix</param>
    /// <returns>Matrix with the results of the multiplication</returns>
    public static Matrix Multiply(Matrix mat1, Matrix mat2)
    {
        if (mat1._cols != mat2._rows)
        {

            throw new Exception("Dimensions are incorrect. Matrix 1 columns: " + mat1._cols + " Matrix 2 rows: " + mat2._rows);
        }
        else
        {
            int r = mat1._rows;
            int c = mat2._cols;
            Matrix m = new Matrix(r, c);
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    float sum = 0;
                    for (int k = 0; k < mat1._cols; k++)
                    {
                        sum += mat1[i, k] * mat2[k, j];
                    }
                    m[i, j] = sum;
                }
            }
            return m;
        }

    }

    public static Matrix Transpose(Matrix m)
    {
        Matrix t = new Matrix(m._cols, m._rows);
        for (int i = 0; i < m._rows; i++)
            for (int j = 0; j < m._cols; j++)
                t[j, i] = m[i, j];
        return t;
    }

    private static Matrix Multiply(float n, Matrix m)
    {
        Matrix r = new Matrix(m._rows, m._cols);
        for (int i = 0; i < m._rows; i++)
            for (int j = 0; j < m._cols; j++)
                r[i, j] = m[i, j] * n;
        return r;
    }

    public static Matrix ApplyFunction(Func<float, float> function, Matrix m)
    {
        Matrix r = new Matrix(m._rows, m._cols);
        for (int i = 0; i < m._rows; i++)
            for (int j = 0; j < m._cols; j++)
                r[i, j] = (float)function(m[i, j]);
        return r;
    }
    public static Matrix ApplyFunction(Func<double, double> function, Matrix m)
    {
        Matrix r = new Matrix(m._rows, m._cols);
        for (int i = 0; i < m._rows; i++)
            for (int j = 0; j < m._cols; j++)
                r[i, j] = (float)function(m[i, j]);
        return r;
    }
    private static Matrix Add(Matrix m1, Matrix m2)
    {
        if (m1._rows != m2._rows || m1._cols != m2._cols) throw new Exception("Matrices must have the same dimensions");
        Matrix r = new Matrix(m1._rows, m1._cols);
        for (int i = 0; i < r._rows; i++)
            for (int j = 0; j < r._cols; j++)
                r[i, j] = m1[i, j] + m2[i, j];
        return r;
    }
    /// <summary>
    /// Creates a Matrix with given dimensions and initialises it with random floats in the range provided
    /// </summary>
    /// <param name="rows">Number of rows in the Matrix</param>
    /// <param name="columns">Number of columns in the Matrix</param>
    /// <param name="range_x">Lower bound of the range (inclusive)</param>
    /// <param name="range_y">Upper bound of the range (exclusive)</param>
    /// <returns></returns>
    public static Matrix Randomise(int rows, int columns, float range_x, float range_y)
    {
        if (range_x >= range_y)
        {
            throw new Exception("Invalid range");
        }
        else
        {
            Matrix m = new Matrix(rows, columns);


            for (int i = 0; i < rows; i++)
            {
                UnityEngine.Random.seed = (System.DateTime.Now.TimeOfDay.Milliseconds * (i + 1));
                for (int j = 0; j < columns; j++)
                {
                    m[i, j] = (float)Math.Round(UnityEngine.Random.value * (range_y - range_x) + range_x, 7);
                }
            }
            return m;
        }

    }
    /// <summary>
    /// Displays a matrix in a nice form
    /// </summary>
    /// <param name="m">Matrix to display</param>
    /// <returns>String that you can just print</returns>
    public static String Display(Matrix m)
    {
        String s = "";
        for (int i = 0; i < m._rows; i++)
        {
            for (int j = 0; j < m._cols; j++)
            {
                s += m[i, j] + ", ";
            }
            s += "\n";
        }
        return s;
    }

    public static Matrix ModifyElementWise(Matrix m, float n, float percentage)
    {
        for (int i = 0; i < m._rows; i++)
        {
            UnityEngine.Random.seed = (System.DateTime.Now.TimeOfDay.Milliseconds * (i + 1));
            for (int j = 0; j < m._cols; j++)
            {
                if (UnityEngine.Random.value <= percentage)
                {
                    if (m[i, j] + n > 1)
                    {
                        m[i, j] = 1;
                    }
                    else if (m[i, j] < -1)
                    {
                        m[i, j] = -1;
                    }
                    else
                    {
                        m[i, j] = m[i, j] + n;
                    }

                }

            }
        }
        return m;
    }
    public static Matrix ModifyElementWise(Matrix m, float n)
    {
        for (int i = 0; i < m._rows; i++)
        {
            for (int j = 0; j < m._cols; j++)
            {

                m[i, j] = m[i, j] + n;

            }

        }
        return m;
    }

    public static Matrix MergeMatrices(Matrix mat1, Matrix mat2)
    {
        if ((mat1._rows != mat2._rows) && (mat1._cols != mat2._cols))
        {
            throw new Exception("Dimensions must be exactly the same");
        }
        else
        {
            Matrix m = new Matrix(mat1._rows, mat2._cols);
            for (int i = 0; i < m._rows; i++)
            {
                UnityEngine.Random.seed = (System.DateTime.Now.TimeOfDay.Milliseconds * (i + 1));
                for (int j = 0; j < m._cols; j++)
                {
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        m[i, j] = mat1[i, j];
                    }
                    else
                    {
                        m[i, j] = mat2[i, j];
                    }
                }
            }
            return m;
        }
    }
    public static Matrix operator -(Matrix m)
    { return Matrix.Multiply(-1, m); }

    public static Matrix operator +(Matrix m1, Matrix m2)
    { return Matrix.Add(m1, m2); }

    public static Matrix operator +(Matrix m, float n)
    { return Matrix.ModifyElementWise(m, n); }

    public static Matrix operator -(Matrix m1, Matrix m2)
    { return Matrix.Add(m1, -m2); }

    public static Matrix operator -(Matrix m, float n)
    { return Matrix.ModifyElementWise(m, -n); }

    public static Matrix operator *(Matrix m1, Matrix m2)
    { return Matrix.Multiply(m1, m2); }

    public static Matrix operator *(float n, Matrix m)
    { return Matrix.Multiply(n, m); }

}
