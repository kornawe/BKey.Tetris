using BKey.Tetris.Logic.Tetrimino;

namespace BKey.Tetris.Logic.Board;
public interface IBoard
{
    int Height { get; }
    int Width { get; }

    public bool[,] Cells { get; }

    public TetriminoPiece CurrentTetrimino { get; }

    public void AddTetrmino(TetriminoPiece piece);

    bool CanMove(int deltaX, int deltaY);
    bool CanRotate();
    int ClearLines();
    void MoveTetrimino(int deltaX, int deltaY);
    void PlaceTetrimino();
    void RotateTetrimino();

    public IBoard Clone();

    public IReadonlyBoard AsReadonly();
}