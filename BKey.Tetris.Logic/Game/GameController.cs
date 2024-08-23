using BKey.Tetris.Logic.Input;
using BKey.Tetris.Logic.Tetrimino;
using System.Threading.Tasks;

namespace BKey.Tetris.Logic.Game;
public class GameController : IGameController
{
    private IBoard Board { get; }
    private IGameDisplay Display { get; }
    private ITetriminoFactory TetriminoFactory { get; }
    private IInputQueue<MovementRequest> MovementQueue { get; }
    private IGameScore Score { get; }

    private GameState CurrentState { get; set; }

    private const int Left = -1;
    private const int Right = 1;
    private const int Up = -1;
    private const int Down = 1;
    private const int None = 0;

    public GameController(
        IBoard board,
        IGameDisplay display,
        ITetriminoFactory tetriminoFactory,
        IInputQueue<MovementRequest> inputQueue,
        IGameScore score)
    {
        Board = board;
        Display = display;
        TetriminoFactory = tetriminoFactory;
        MovementQueue = inputQueue;
        CurrentState = GameState.NewPieceSpawn;
        Score = score;
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

            await Task.Delay(10);
        }
    }

    private void HandleInput()
    {
        // Capture user input (e.g., left, right, rotate, drop)
        // For example, let's assume we're moving to the Rotation state after input
        // This would need to be expanded based on actual input handling

        if (MovementQueue.IsEmpty)
        {
            return;
        }

        CurrentState = GameState.Movement;
    }

    private void HandleMovement()
    {
        // Move the Tetrimino down (or based on user input) and check for collision
        // If movement is complete, move to the commit state

        var movement = MovementQueue.Dequeue();

        if (movement == MovementRequest.Rotate)
        {
            Board.RotateTetrimino(Board.CurrentTetrimino);
        }

        if (movement == MovementRequest.Left)
        {
            Board.MoveTetrimino(Board.CurrentTetrimino, Left, None);
        }

        if (movement == MovementRequest.Right)
        {
            Board.MoveTetrimino(Board.CurrentTetrimino, Right, None);
        }

        if (movement == MovementRequest.Down)
        {
            Board.MoveTetrimino(Board.CurrentTetrimino, None, Down);
        }

        CurrentState = GameState.Commit;
    }

    private void CommitTetrimino()
    {
        // Add the Tetrimino to the board and check if it is placed
        // If placed, move to the line clear state
        if (!Board.CanMove(Board.CurrentTetrimino, None, Down))
        {
            Board.PlaceTetrimino(Board.CurrentTetrimino);
            Score.AddPiecePlaced();
            Board.CurrentTetrimino = null;
            var linesCleared = Board.ClearLines();
            Score.AddLinesCleared(linesCleared);
        }

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
