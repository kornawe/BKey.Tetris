using BKey.Tetris.Logic.Input;
using BKey.Tetris.Logic.Tetrimino;
using System;
using System.Threading.Tasks;

namespace BKey.Tetris.Logic.Game;
public class GameControllerStated : IGameController
{
    private readonly IBoard Board;
    private IDisplay Display { get; }
    private ITetriminoFactory TetriminoFactory { get; }
    private IInputQueue InputQueue { get; }

    private GameState CurrentState { get; set; }

    public GameControllerStated(
        IBoard board,
        IDisplay display,
        ITetriminoFactory tetriminoFactory,
        IInputQueue inputQueue)
    {
        Board = board;
        Display = display;
        TetriminoFactory = tetriminoFactory;
        InputQueue = inputQueue;
        CurrentState = GameState.NewPieceSpawn;
    }

    public async Task Run()
    {
        while (true)
        {
            switch (CurrentState)
            {
                case GameState.Input:
                    HandleInput();
                    break;
                case GameState.Movement:
                    HandleMovement();
                    break;
                case GameState.Commit:
                    CommitTetrimino();
                    break;
                case GameState.LineClear:
                    ClearLines();
                    break;
                case GameState.NewPieceSpawn:
                    SpawnNewPiece();
                    break;
                case GameState.Render:
                    Render();
                    break;
            }

            await Task.Delay(50);
        }
    }

    private void HandleInput()
    {
        // Capture user input (e.g., left, right, rotate, drop)
        // For example, let's assume we're moving to the Rotation state after input
        // This would need to be expanded based on actual input handling

        if (InputQueue.IsEmpty)
        {
            return;
        }

        CurrentState = GameState.Movement;
    }

    private void HandleMovement()
    {
        // Move the Tetrimino down (or based on user input) and check for collision
        // If movement is complete, move to the commit state

        var movement = InputQueue.Dequeue();

        if (movement == InputRequest.Rotate)
        {
            Board.CurrentTetrimino.Rotate();
            if (Board.IsCollision(Board.CurrentTetrimino))
            {
                Board.CurrentTetrimino.Rotate(); // Reverse rotation
                Board.CurrentTetrimino.Rotate();
                Board.CurrentTetrimino.Rotate();
            }
        }

        if (movement == InputRequest.Left)
        {
            Board.CurrentTetrimino.X--;
            if (Board.IsCollision(Board.CurrentTetrimino))
            {
                Board.CurrentTetrimino.X++;
            }
        }

        if (movement == InputRequest.Right)
        {
            Board.CurrentTetrimino.X++;
            if (Board.IsCollision(Board.CurrentTetrimino))
            {
                Board.CurrentTetrimino.X--;
            }
        }

        if (movement == InputRequest.Down)
        {
            Board.CurrentTetrimino.Y++;
            if (Board.IsCollision(Board.CurrentTetrimino))
            {
                Board.CurrentTetrimino.Y--;
                Board.PlaceTetrimino(Board.CurrentTetrimino);
                Board.CurrentTetrimino = null;
                Board.ClearLines();
            }
        }

        CurrentState = GameState.Commit;
    }

    private void CommitTetrimino()
    {
        // Add the Tetrimino to the board and check if it is placed
        // If placed, move to the line clear state

        

        CurrentState = GameState.LineClear;
    }

    private void ClearLines()
    {
        // Check and clear any full lines on the board
        // After clearing lines, move to the new piece spawn state
        //Board.ClearLines();
        CurrentState = GameState.NewPieceSpawn;
    }

    private void SpawnNewPiece()
    {
        if (Board.CurrentTetrimino == null)
        {
            // Spawn a new Tetrimino and place it on the board
            Board.CurrentTetrimino = TetriminoFactory.Next();
            Board.CurrentTetrimino.X = Board.Width / 2 - Board.CurrentTetrimino.Shape.GetLength(1) / 2;
            Board.CurrentTetrimino.Y = 0;
        }

        // Move to the render state
        CurrentState = GameState.Render;
    }

    private void Render()
    {
        // Render the current state of the board and the active Tetrimino
        Display.Draw();

        // Move back to the input state after rendering
        CurrentState = GameState.Input;
    }
}
