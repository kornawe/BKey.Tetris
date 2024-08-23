using BKey.Tetris.Logic.Input;
using BKey.Tetris.Logic.Tetrimino;
using System;
using System.Threading.Tasks;

namespace BKey.Tetris.Logic.Game;
public class GameController : IGameController
{
    private IBoard Board { get; }
    private IDisplay Display { get; }
    private ITetriminoFactory TetriminoFactory { get; }
    private IInputQueue InputQueue { get; }


    public GameController(
        IBoard board,
        IDisplay display,
        ITetriminoFactory tetriminoFactory,
        IInputQueue inputQueue)
    {
        Board = board;
        Display = display;
        TetriminoFactory = tetriminoFactory;
        InputQueue = inputQueue;
        SpawnTetrimino();
    }

    private void SpawnTetrimino()
    {
        Board.CurrentTetrimino = TetriminoFactory.Next();
        Board.CurrentTetrimino.X = Board.Width / 2 - Board.CurrentTetrimino.Shape.GetLength(1) / 2;
        Board.CurrentTetrimino.Y = 0;
    }

    public async Task Run()
    {
        while (true)
        {
            Display.Draw();
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.LeftArrow)
            {
                Board.CurrentTetrimino.X--;
                if (Board.IsCollision(Board.CurrentTetrimino))
                {
                    Board.CurrentTetrimino.X++;
                }
            }
            else if (key == ConsoleKey.RightArrow)
            {
                Board.CurrentTetrimino.X++;
                if (Board.IsCollision(Board.CurrentTetrimino))
                {
                    Board.CurrentTetrimino.X--;
                }
            }
            else if (key == ConsoleKey.UpArrow)
            {
                Board.CurrentTetrimino.Rotate();
                if (Board.IsCollision(Board.CurrentTetrimino))
                {
                    Board.CurrentTetrimino.Rotate(); // Reverse rotation
                    Board.CurrentTetrimino.Rotate();
                    Board.CurrentTetrimino.Rotate();
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                Board.CurrentTetrimino.Y++;
                if (Board.IsCollision(Board.CurrentTetrimino))
                {
                    Board.CurrentTetrimino.Y--;
                    Board.PlaceTetrimino(Board.CurrentTetrimino);
                    Board.ClearLines();
                    SpawnTetrimino();
                    if (Board.IsCollision(Board.CurrentTetrimino))
                    {
                        Console.WriteLine("Game Over!");
                        break;
                    }
                }
            }
        }
    }
}
