using System;

namespace BKey.Tetris.Logic.Tetrimino;
public interface ITetriminoPiece
{
    ConsoleColor Color { get; }
    int Rotation { get; set; }
    int[,] Shape { get; set; }
    int X { get; set; }
    int Y { get; set; }
}