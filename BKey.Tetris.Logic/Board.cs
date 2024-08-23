namespace BKey.Tetris.Logic;
public class Board : IBoard
{
    public int[,] Grid { get; }

    public int Width { get; }
    public int Height { get; }

    public Tetrimino? CurrentTetrimino { get; set; }

    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        Grid = new int[height, width];
    }

    public bool IsCollision(Tetrimino tetrimino)
    {
        for (int i = 0; i < tetrimino.Shape.GetLength(0); i++)
        {
            for (int j = 0; j < tetrimino.Shape.GetLength(1); j++)
            {
                if (tetrimino.Shape[i, j] != 0)
                {
                    int newX = tetrimino.X + j;
                    int newY = tetrimino.Y + i;

                    if (newX < 0 || newX >= Width || newY < 0 || newY >= Height || Grid[newY, newX] != 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void PlaceTetrimino(Tetrimino tetrimino)
    {
        for (int i = 0; i < tetrimino.Shape.GetLength(0); i++)
        {
            for (int j = 0; j < tetrimino.Shape.GetLength(1); j++)
            {
                if (tetrimino.Shape[i, j] != 0)
                {
                    Grid[tetrimino.Y + i, tetrimino.X + j] = tetrimino.Shape[i, j];
                }
            }
        }
    }

    public void ClearLines()
    {
        for (int i = Height - 1; i >= 0; i--)
        {
            bool isFullLine = true;
            for (int j = 0; j < Width; j++)
            {
                if (Grid[i, j] == 0)
                {
                    isFullLine = false;
                    break;
                }
            }

            if (isFullLine)
            {
                for (int k = i; k > 0; k--)
                {
                    for (int l = 0; l < Width; l++)
                    {
                        Grid[k, l] = Grid[k - 1, l];
                    }
                }

                for (int l = 0; l < Width; l++)
                {
                    Grid[0, l] = 0;
                }
                i++; // Re-check the same line after shifting down
            }
        }
    }
}

