namespace BKey.Tetris.Logic;
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

