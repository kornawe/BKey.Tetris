using BKey.Tetris.Logic.Movement;
using BKey.Tetris.Logic.Tetrimino;

namespace BKey.Tetris.Logic.Board;
public class Board : IBoard, IReadonlyBoard
{
    public int Width { get; }
    public int Height { get; }

    public bool[,] Cells { get; }

    public TetriminoPiece? CurrentTetrimino { get; private set; }

    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new bool[height, width];
    }

    public Board(Board other)
    {
        Width = other.Width;
        Height = other.Height;
        Cells = new bool[Height, Width];
        // TODO make better
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                Cells[i, j] = other.Cells[i, j];
            }
        }
        if (other.CurrentTetrimino != null)
        {
            CurrentTetrimino = new TetriminoPiece(other.CurrentTetrimino);
        }
    }

    public void AddTetrmino(TetriminoPiece piece)
    {
        piece.X = Width / 2 - piece.Shape.GetLength(1) / 2;
        piece.Y = 0;

        CurrentTetrimino = piece;
    }

    internal bool CanMove(TetriminoPiece tetrimino, int deltaX, int deltaY)
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

    public bool CanMove(int deltaX, int deltaY)
    {
        if (CurrentTetrimino == null)
        {
            return false;
        }
        return CanMove(CurrentTetrimino, deltaX, deltaY);
    }

    public bool CanMove(IntVector2 vector2)
    {
        return CanMove(vector2.X, vector2.Y);
    }

    internal bool CanRotate(TetriminoPiece tetrimino)
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

    public bool CanRotate()
    {
        if (CurrentTetrimino == null)
        {
            return false;
        }
        return CanRotate(CurrentTetrimino);
    }

    public void MoveTetrimino(TetriminoPiece tetrimino, int deltaX, int deltaY)
    {
        if (CanMove(tetrimino, deltaX, deltaY))
        {
            tetrimino.X += deltaX;
            tetrimino.Y += deltaY;
        }
    }

    public void MoveTetrimino(IntVector2 vector2)
    {
        MoveTetrimino(vector2.X, vector2.Y);
    }

    public void MoveTetrimino(int deltaX, int deltaY)
    {
        if (CurrentTetrimino == null)
        {
            return;
        }
        MoveTetrimino(CurrentTetrimino, deltaX, deltaY);
    }

    public void RotateTetrimino(TetriminoPiece tetrimino)
    {
        if (CanRotate(tetrimino))
        {
            tetrimino.Shape = RotateShape(tetrimino.Shape);
            tetrimino.Rotation = (tetrimino.Rotation + 90) % 360;
        }
    }

    public void RotateTetrimino()
    {
        if (CurrentTetrimino == null)
        {
            return;
        }
        RotateTetrimino(CurrentTetrimino);
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

    public void PlaceTetrimino()
    {
        if (CurrentTetrimino == null)
        {
            return;
        }
        PlaceTetrimino(CurrentTetrimino);
        CurrentTetrimino = null;
    }

    private static int[,] RotateShape(int[,] shape)
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

    public IBoard Clone()
    {
        return new Board(this);
    }

    public IReadonlyBoard AsReadonly()
    {
        return this;
    }
}


