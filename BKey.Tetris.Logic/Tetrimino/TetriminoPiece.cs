using System;

namespace BKey.Tetris.Logic.Tetrimino;
public class TetriminoPiece
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
}
