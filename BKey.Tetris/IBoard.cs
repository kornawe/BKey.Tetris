using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKey.Tetris;
public interface IBoard
{
    int Width { get; }
    int Height { get; }
    int[,] Grid { get; }
    Tetrimino? CurrentTetrimino { get; set; }
    bool IsCollision(Tetrimino tetrimino);
    void PlaceTetrimino(Tetrimino tetrimino);
    void ClearLines();
}

