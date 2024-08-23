using BKey.Tetris.Logic.Tetrimino;

namespace BKey.Tetris.Logic;
public interface IBoard
{
    int Height { get; }
    int Width { get; }

    public bool[,] Cells { get; }

    public TetriminoPiece CurrentTetrimino { get; set; }

    bool CanMove(TetriminoPiece tetrimino, int deltaX, int deltaY);
    bool CanRotate(TetriminoPiece tetrimino);
    void ClearLines();
    void MoveTetrimino(TetriminoPiece tetrimino, int deltaX, int deltaY);
    void PlaceTetrimino(TetriminoPiece tetrimino);
    void RotateTetrimino(TetriminoPiece tetrimino);
}