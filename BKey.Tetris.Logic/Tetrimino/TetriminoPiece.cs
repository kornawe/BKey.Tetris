using System;

namespace BKey.Tetris.Logic.Tetrimino;
public class TetriminoPiece : ITetriminoPiece
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Rotation { get; set; }
    public ConsoleColor Color { get; }
    public int[,] Shape { get; set; }

    public TetriminoPiece(int[,] shape, ConsoleColor color)
    {
        Shape = shape;
        X = 0;
        Y = 0;
        Rotation = 0;
        Color = color;
    }

    public TetriminoPiece(TetriminoPiece other)
    {
        this.X = other.X;
        this.Y = other.Y;
        this.Rotation = other.Rotation;
        this.Color = other.Color;

        int rows = other.Shape.GetLength(0);
        int cols = other.Shape.GetLength(1);
        Shape = new int[rows, cols];
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                Shape[i, j] = other.Shape[i, j];
            }
        }
    }
}
