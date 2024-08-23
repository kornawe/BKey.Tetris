using BKey.Tetris.Logic.Tetrimino;

namespace BKey.Tetris.Logic;
public interface IBoard
{
    int Width { get; }
    int Height { get; }
    int[,] Grid { get; }
    TetriminoPiece? CurrentTetrimino { get; set; }
    bool IsCollision(TetriminoPiece tetrimino);
    void PlaceTetrimino(TetriminoPiece tetrimino);
    void ClearLines();
}

