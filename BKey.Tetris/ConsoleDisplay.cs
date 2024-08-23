using System;

namespace BKey.Tetris;
public class ConsoleDisplay : IDisplay
{
    private readonly IBoard board;

    public ConsoleDisplay(IBoard board)
    {
        this.board = board;
    }

    public void Draw()
    {
        Console.Clear();

        int width = board.Width;
        int height = board.Height;

        // Draw the top border
        Console.WriteLine(new string('-', width + 2));

        // Draw the grid with borders
        for (int i = 0; i < height; i++)
        {
            Console.Write("|"); // Left border

            for (int j = 0; j < width; j++)
            {
                bool isTetriminoPart = false;

                if (board.CurrentTetrimino != null)
                {
                    int shapeWidth = board.CurrentTetrimino.Shape.GetLength(1);
                    int shapeHeight = board.CurrentTetrimino.Shape.GetLength(0);

                    for (int m = 0; m < shapeHeight; m++)
                    {
                        for (int n = 0; n < shapeWidth; n++)
                        {
                            if (board.CurrentTetrimino.Shape[m, n] != 0)
                            {
                                int x = board.CurrentTetrimino.X + n;
                                int y = board.CurrentTetrimino.Y + m;

                                if (x == j && y == i)
                                {
                                    Console.ForegroundColor = board.CurrentTetrimino.Color;
                                    Console.Write("#");
                                    Console.ResetColor();
                                    isTetriminoPart = true;
                                    break;
                                }
                            }
                        }
                        if (isTetriminoPart) break;
                    }
                }

                if (!isTetriminoPart)
                {
                    Console.Write(board.Grid[i, j] == 0 ? "." : "#");
                }
            }

            Console.WriteLine("|"); // Right border
        }

        // Draw the bottom border
        Console.WriteLine(new string('-', width + 2));
    }
}

