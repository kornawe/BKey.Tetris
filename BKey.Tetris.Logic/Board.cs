using BKey.Tetris.Logic.Tetrimino;

namespace BKey.Tetris.Logic;
public class Board : IBoard
{
    public int Width { get; }
    public int Height { get; }

    public bool[,] Cells { get; }

    public TetriminoPiece CurrentTetrimino { get; set; }

    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new bool[height, width];
    }

    public bool CanMove(TetriminoPiece tetrimino, int deltaX, int deltaY)
    {
        for (int i = 0; i < tetrimino.Shape.GetLength(0); i++)
        {
            for (int j = 0; j < tetrimino.Shape.GetLength(1); j++)
            {
                if (tetrimino.Shape[i, j] != 0)
                {
                    int newX = tetrimino.X + j + deltaX;
                    int newY = tetrimino.Y + i + deltaY;

                    if (newX < 0 || newX >= Width || newY < 0 || newY >= Height || Cells[newY, newX])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public bool CanRotate(TetriminoPiece tetrimino)
    {
        int[,] rotatedShape = RotateShape(tetrimino.Shape);
        for (int i = 0; i < rotatedShape.GetLength(0); i++)
        {
            for (int j = 0; j < rotatedShape.GetLength(1); j++)
            {
                if (rotatedShape[i, j] != 0)
                {
                    int newX = tetrimino.X + j;
                    int newY = tetrimino.Y + i;

                    if (newX < 0 || newX >= Width || newY < 0 || newY >= Height || Cells[newY, newX])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void MoveTetrimino(TetriminoPiece tetrimino, int deltaX, int deltaY)
    {
        if (CanMove(tetrimino, deltaX, deltaY))
        {
            tetrimino.X += deltaX;
            tetrimino.Y += deltaY;
        }
    }

    public void RotateTetrimino(TetriminoPiece tetrimino)
    {
        if (CanRotate(tetrimino))
        {
            tetrimino.Shape = RotateShape(tetrimino.Shape);
            tetrimino.Rotation = (tetrimino.Rotation + 90) % 360;
        }
    }

    public void PlaceTetrimino(TetriminoPiece tetrimino)
    {
        for (int i = 0; i < tetrimino.Shape.GetLength(0); i++)
        {
            for (int j = 0; j < tetrimino.Shape.GetLength(1); j++)
            {
                if (tetrimino.Shape[i, j] != 0)
                {
                    Cells[tetrimino.Y + i, tetrimino.X + j] = true;
                }
            }
        }
    }

    private int[,] RotateShape(int[,] shape)
    {
        int rowCount = shape.GetLength(0);
        int colCount = shape.GetLength(1);
        int[,] rotatedShape = new int[colCount, rowCount];

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                rotatedShape[j, rowCount - 1 - i] = shape[i, j];
            }
        }

        return rotatedShape;
    }

    public int ClearLines()
    {
        var linesCleared = 0;
        for (int y = Height - 1; y >= 0; y--)
        {
            bool isLineFull = true;
            for (int x = 0; x < Width; x++)
            {
                if (!Cells[y, x])
                {
                    isLineFull = false;
                    break;
                }
            }

            if (isLineFull)
            {
                ClearLine(y);
                y++; // Recheck the same line after clearing
                linesCleared++;
            }
        }
        return linesCleared;
    }

    private void ClearLine(int line)
    {
        for (int y = line; y > 0; y--)
        {
            for (int x = 0; x < Width; x++)
            {
                Cells[y, x] = Cells[y - 1, x];
            }
        }

        for (int x = 0; x < Width; x++)
        {
            Cells[0, x] = false;
        }
    }
}


