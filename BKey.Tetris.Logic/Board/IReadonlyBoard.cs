using BKey.Tetris.Logic.Tetrimino;

namespace BKey.Tetris.Logic.Board;
public interface IReadonlyBoard
{
    int Height { get; }
    int Width { get; }

    // TODO make readonly
    public bool[,] Cells { get; }

    // TODO make readonly
    public TetriminoPiece? CurrentTetrimino { get; }
}
