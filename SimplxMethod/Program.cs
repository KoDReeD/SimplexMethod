using System;
using SimplxMethod;

double[,] matrix = {
    {2, 5,2,12},
    {7,1,2,18},
    {-3,-4,-6,0}
};

static void PrintMatrix(double[,] matrix)
{
    int numRows = matrix.GetLength(0);
    int numCols = matrix.GetLength(1);

    for (int i = 0; i < numRows; i++)
    {
        for (int j = 0; j < numCols; j++)
        {
            if (j == 0)
            {
                Console.Write($"{matrix[i, numCols-1],-10}");
            }
            else if (j == numCols - 1)
            {
                continue;
            }
            Console.Write($"{matrix[i, j],-10}");
        }
        Console.WriteLine();
    }
}

static bool CheckOptimality(double[,] matrix)
{
    int lastRowIndex = matrix.GetLength(0) - 1;
    int numCols = matrix.GetLength(1);

    for (int j = 0; j < numCols; j++)
    {
        if (matrix[lastRowIndex, j] < 0)
        {
            return false;
        }
    }
    
    return true;
}

static int FindEnteringColumn(double[,] matrix)
{
    int lastRowIndex = matrix.GetLength(0) - 1;
    int numCols = matrix.GetLength(1);
    double minVal = double.MaxValue;
    int pivotCol = -1;

    for (int j = 0; j < numCols; j++)
    {
        double currentVal = matrix[lastRowIndex, j];
        if (currentVal < minVal)
        {
            minVal = currentVal;
            pivotCol = j;
        }
    }

    return pivotCol;
}

static int FindPivotRow(double[,] matrix, int pivotCol)
{
    int numRows = matrix.GetLength(0);
    int numCols = matrix.GetLength(1);
    double minRatio = double.MaxValue;
    int pivotRow = -1;

    for (int i = 0; i < numRows - 1; i++)
    {
        if (matrix[i, pivotCol] > 0)
        {
            double ratio = matrix[i, numCols - 1] / matrix[i, pivotCol];
            if (ratio < minRatio)
            {
                minRatio = ratio;
                pivotRow = i;
            }
        }
    }
    return pivotRow;
}

static void UpdateMatrix(double[,] matrix, int pivotRow, int pivotCol)
{
    int numRows = matrix.GetLength(0);
    int numCols = matrix.GetLength(1);
    double pivotElement = matrix[pivotRow, pivotCol];

    for (int j = 0; j < numCols; j++)
    {
        if (j == Helper.PivotCol)
        {
            matrix[pivotRow, j] = 1 / pivotElement;
        }
        else
        {
            matrix[pivotRow, j] /= pivotElement;
        }
    }

    for (int i = 0; i < numRows; i++)
    {
        if (i != pivotRow)
        {
            double factor = matrix[i, pivotCol] * -1;
            for (int j = 0; j < numCols; j++)
            {
                if (j == pivotCol)
                {
                    matrix[i, j] = factor * matrix[pivotRow, j];
                }
                else
                {
                    matrix[i, j] += factor * matrix[pivotRow, j];
                }
            }
        }
    }
}

static void PrintReshenie(double[,] matrix)
{
    int numRows = matrix.GetLength(0);
    int numCols = matrix.GetLength(1);
    Console.WriteLine("Оптимальное решение:");
    
    for (int j = 0; j < numCols - 1; j++)
    {
        bool isBasic = false;
        int basicRow = -1;
        
        for (int i = 0; i < numRows - 1; i++)
        {
            if (matrix[i, j] == 1 && !isBasic)
            {
                isBasic = true;
                basicRow = i;
            }
            else if (matrix[i, j] != 0)
            {
                isBasic = false;
                break;
            }
        }
        
        if (isBasic)
        {
            Console.Write($"x{j + 1} = {matrix[basicRow, numCols - 1]}");
        }
        else
        {
            Console.Write($"x{j + 1} = 0");
        }
        
        Console.Write("    ");
    }
    
    Console.WriteLine($"F = {matrix[numRows - 1, numCols - 1]}");
    
}

Console.WriteLine($"Таблица {Helper.Stepik}");
PrintMatrix(matrix);
Console.WriteLine();

while (!CheckOptimality(matrix))
{  
    Helper.PivotCol = FindEnteringColumn(matrix);
    Helper.PivotRow = FindPivotRow(matrix, Helper.PivotCol);

    UpdateMatrix(matrix, Helper.PivotRow, Helper.PivotCol);
    Console.WriteLine($"Таблица {++Helper.Stepik}");
    PrintMatrix(matrix);
    Console.WriteLine();
}

PrintReshenie(matrix);