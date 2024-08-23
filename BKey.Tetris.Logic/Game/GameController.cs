using BKey.Tetris.Logic.Tetrimino;
using System;

namespace BKey.Tetris.Logic.Game;
public class GameController
{
    private readonly IBoard board;
    private readonly IDisplay display;
    private readonly ITetriminoFactory tetriminoFactory;


    public GameController(
        IBoard board,
        IDisplay display,
        ITetriminoFactory tetriminoFactory)
    {
        this.board = board;
        this.display = display;
        this.tetriminoFactory = tetriminoFactory;
        SpawnTetrimino();
    }

    private void SpawnTetrimino()
    {
        board.CurrentTetrimino = tetriminoFactory.Next();
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
