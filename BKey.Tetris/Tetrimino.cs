using System;

namespace BKey.Tetris;
public class Tetrimino
{
    public int[,] Shape { get; private set; }
    public int X { get; set; }
    public int Y { get; set; }
    public ConsoleColor Color { get; set; } // Add color property

    public Tetrimino(int[,] shape, ConsoleColor color)
    {
        Shape = shape;
        X = 0;
        Y = 0;
        Color = color;
    }

    public void Rotate()
    {
        int length = Shape.GetLength(0);
        int[,] newShape = new int[length, length];

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                newShape[j, length - i - 1] = Shape[i, j];
            }
        }

        Shape = newShape;
    }
}
