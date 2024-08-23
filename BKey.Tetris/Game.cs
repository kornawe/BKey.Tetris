using System;

namespace BKey.Tetris;
public class Game
{
    private readonly IBoard board;
    private readonly IDisplay display;
    private Tetrimino currentTetrimino;
    private Random random;

    private static readonly int[][,] TetriminoShapes = new int[][,]
    {
        new int[,] { { 1, 1, 1, 1 } }, // I
        new int[,] { { 1, 1 }, { 1, 1 } }, // O
        new int[,] { { 0, 1, 0 }, { 1, 1, 1 } }, // T
        new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }, // S
        new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }, // Z
        new int[,] { { 1, 0, 0 }, { 1, 1, 1 } }, // L
        new int[,] { { 0, 0, 1 }, { 1, 1, 1 } }  // J
    };

    public Game(IBoard board, IDisplay display)
    {
        this.board = board;
        this.display = display;
        random = new Random();
        SpawnTetrimino();
    }

    private void SpawnTetrimino()
    {
        board.CurrentTetrimino = new Tetrimino(TetriminoShapes[random.Next(TetriminoShapes.Length)]);
        board.CurrentTetrimino.X = board.Width / 2 - board.CurrentTetrimino.Shape.GetLength(1) / 2;
        board.CurrentTetrimino.Y = 0;
    }

    public void Run()
    {
        while (true)
        {
            display.Draw();
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.LeftArrow)
            {
                board.CurrentTetrimino.X--;
                if (board.IsCollision(board.CurrentTetrimino))
                {
                    board.CurrentTetrimino.X++;
                }
            }
            else if (key == ConsoleKey.RightArrow)
            {
                board.CurrentTetrimino.X++;
                if (board.IsCollision(board.CurrentTetrimino))
                {
                    board.CurrentTetrimino.X--;
                }
            }
            else if (key == ConsoleKey.UpArrow)
            {
                board.CurrentTetrimino.Rotate();
                if (board.IsCollision(board.CurrentTetrimino))
                {
                    board.CurrentTetrimino.Rotate(); // Reverse rotation
                    board.CurrentTetrimino.Rotate();
                    board.CurrentTetrimino.Rotate();
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                board.CurrentTetrimino.Y++;
                if (board.IsCollision(board.CurrentTetrimino))
                {
                    board.CurrentTetrimino.Y--;
                    board.PlaceTetrimino(board.CurrentTetrimino);
                    board.ClearLines();
                    SpawnTetrimino();
                    if (board.IsCollision(board.CurrentTetrimino))
                    {
                        Console.WriteLine("Game Over!");
                        break;
                    }
                }
            }
        }
    }
}
