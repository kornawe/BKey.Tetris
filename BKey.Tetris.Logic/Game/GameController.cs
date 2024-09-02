using BKey.Tetris.Logic.Board;
using BKey.Tetris.Logic.Events;
using BKey.Tetris.Logic.Input;
using BKey.Tetris.Logic.Movement;
using BKey.Tetris.Logic.Tetrimino;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Logic.Game;
public class GameController : IGameController
{
    private BoardBuffer BoardBuffer { get; }
    private ITetriminoFactory TetriminoFactory { get; }
    private IEventQueue<MovementRequestEvent> MovementQueue { get; }
    private IGameScore Score { get; }
    private MovementAggregator MovementAggregator { get; }
    private Stopwatch LoopStopwatch { get; }

    private GameState CurrentState { get; set; }

    private static readonly IntVector2 Left = new IntVector2(-1, 0);
    private static readonly IntVector2 Right = new IntVector2(1, 0);
    private static readonly IntVector2 Down = new IntVector2(0, 1);
    private static readonly IntVector2 None = IntVector2.Origin;

    private bool disposedValue;

    public GameController(
        BoardBuffer boardBuffer,
        ITetriminoFactory tetriminoFactory,
        IEventQueue<MovementRequestEvent> inputQueue,
        IGameScore score,
        IMovementSource fallSource)
    {
        BoardBuffer = boardBuffer;
        TetriminoFactory = tetriminoFactory;
        MovementQueue = inputQueue;
        CurrentState = GameState.NewPieceSpawn;
        Score = score;
        MovementAggregator = new MovementAggregator([
            fallSource
            ]);
        LoopStopwatch = new Stopwatch();
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        LoopStopwatch.Restart();
        while (!cancellationToken.IsCancellationRequested)
        {
            switch (CurrentState)
            {
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
                case GameState.GameOver:
                    break;
            }

            await Task.Delay(10);
        }
    }

    private void HandleMovement()
    {
        // Move the Tetrimino down (or based on user input) and check for collision
        // If movement is complete, move to the commit state

        var board = BoardBuffer.GetWriteBoard();

        var movements = MovementQueue.DequeueAll();
        foreach (var move in movements)
        {
            switch (move.Request)
            {
                case MovementRequest.Rotate:
                    board.RotateTetrimino();
                    break;
                case MovementRequest.Left:
                    board.MoveTetrimino(Left);
                    break;
                case MovementRequest.Right:
                    board.MoveTetrimino(Right);
                    break;
                case MovementRequest.Down:
                    board.MoveTetrimino(Down);
                    break;
                default:
                    continue;
            }


        }
        var automatedMovement = MovementAggregator.Update(LoopStopwatch.Elapsed);
        board.MoveTetrimino(automatedMovement);

        LoopStopwatch.Restart();
        CurrentState = GameState.Commit;
    }

    private void CommitTetrimino()
    {
        // Add the Tetrimino to the board and check if it is placed
        // If placed, move to the line clear state
        var board = BoardBuffer.GetWriteBoard();
        if (!board.CanMove(Down))
        {
            board.PlaceTetrimino();
            Score.AddPiecePlaced();
        }

        CurrentState = GameState.LineClear;
    }

    private void ClearLines()
    {
        // Check and clear any full lines on the board
        // After clearing lines, move to the new piece spawn state
        var board = BoardBuffer.GetWriteBoard();
        if (board.CurrentTetrimino == null)
        {
            var linesCleared = board.ClearLines();
            Score.AddLinesCleared(linesCleared);
        }

        CurrentState = GameState.NewPieceSpawn;
    }

    private void SpawnNewPiece()
    {
        var board = BoardBuffer.GetWriteBoard();
        if (board.CurrentTetrimino == null)
        {
            // Spawn a new Tetrimino and place it on the board
            board.AddTetrmino(TetriminoFactory.Next());
            if (!board.CanMove(None))
            {
                CurrentState = GameState.GameOver;
                return;
            }
        }

        // Move to the render state
        CurrentState = GameState.Render;
    }

    private void Render()
    {
        // Render the current state of the board and the active Tetrimino
        BoardBuffer.SwapBuffers();

        // Move back to the input state after rendering
        CurrentState = GameState.Movement;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                MovementQueue.Dispose();
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        System.GC.SuppressFinalize(this);
    }
}
