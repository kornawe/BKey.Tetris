using BKey.Tetris.Logic;
using BKey.Tetris.Logic.Game;
using System;

namespace BKey.Tetris.Console;
public class ConsoleDisplay : IGameDisplay
{
    private readonly IBoard board;
    private readonly IGameScore gameScore;

    public ConsoleDisplay(IBoard board, IGameScore gameScore)
    {
        this.board = board;
        this.gameScore = gameScore;
    }

    public void Draw()
    {
        System.Console.Clear();

        // Display the game score stats
        System.Console.WriteLine($"Time: {gameScore.Elapsed.ToString(@"hh\:mm\:ss")} | Lines Cleared: {gameScore.LinesCleared} | Pieces Placed: {gameScore.PiecesPlaced}");

        int width = board.Width;
        int height = board.Height;

        // Draw the top border
        System.Console.WriteLine(new string('-', width + 2));

        // Draw the grid with borders
        for (int i = 0; i < height; i++)
        {
            System.Console.Write("|"); // Left border

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
                                    System.Console.ForegroundColor = board.CurrentTetrimino.Color;
                                    System.Console.Write("#");
                                    System.Console.ResetColor();
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
                    System.Console.Write(board.Cells[i, j] ? "#" : ".");
                }
            }

            System.Console.WriteLine("|"); // Right border
        }

        // Draw the bottom border
        System.Console.WriteLine(new string('-', width + 2));
    }
}
