using BKey.Tetris.Logic;
using BKey.Tetris.Logic.Board;
using BKey.Tetris.Logic.Game;
using System;

namespace BKey.Tetris.Console;
public class ConsoleDisplay : IGameDisplay
{
    private readonly BoardBuffer _boardBuffer;
    private readonly IGameScore gameScore;

    private bool _firstDraw = true;

    private (int Left, int Top) ScorePosition = (0, 0);
    private (int Left, int Top) BoardPosition = (0, 1);

    public ConsoleDisplay(BoardBuffer boardBuffer, IGameScore gameScore)
    {
        _boardBuffer = boardBuffer;
        this.gameScore = gameScore;
    }

    public void Draw()
    {
        if (_firstDraw)
        {
            _firstDraw = false;
            System.Console.CursorVisible = false;
            System.Console.Clear();
        }

        // Display the game score stats
        System.Console.SetCursorPosition(ScorePosition.Left, ScorePosition.Top);
        System.Console.WriteLine($"Time: {gameScore.Elapsed.ToString(@"hh\:mm\:ss")} | Lines Cleared: {gameScore.LinesCleared} | Pieces Placed: {gameScore.PiecesPlaced}");

        System.Console.SetCursorPosition(BoardPosition.Left, BoardPosition.Top);
        var board = _boardBuffer.GetReadBoard();

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

    public void Dispose() {
        System.Console.CursorVisible = true;
    }
}
