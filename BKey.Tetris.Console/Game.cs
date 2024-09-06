
using System;
using System.Threading;
using System.Threading.Tasks;
using BKey.Tetris.Console.Input;
using BKey.Tetris.Logic;
using BKey.Tetris.Logic.Board;
using BKey.Tetris.Logic.Events;
using BKey.Tetris.Logic.Game;
using BKey.Tetris.Logic.Input;
using BKey.Tetris.Logic.Movement;
using BKey.Tetris.Logic.Tetrimino;

namespace BKey.Tetris.Console;

public class Game
{
    private IEventBus EventBus { get; }
    private IBoardFactory BoardFactory { get; }

    private MovementRequestKeyAdapter MovementRequestKeyAdapter { get; }

    private IEventQueue<MovementRequestEvent> MovementRequestEventQueue { get; }
    
    public Game(
        IEventBus eventBus,
        IBoardFactory boardFactory,
        MovementRequestKeyAdapter movementRequestKeyAdapter,
        IEventQueue<MovementRequestEvent> movementRequestEventQueue)
    {
        EventBus = eventBus;
        BoardFactory = boardFactory;
        MovementRequestKeyAdapter = movementRequestKeyAdapter;
        MovementRequestEventQueue = movementRequestEventQueue;
    }

    public async Task Run(NewGameSettings newGameSettings) {
        var random = new Random(newGameSettings.Seed);
        var board = BoardFactory.Create(newGameSettings.BoardCreateOptions);
        var score = new GameScore();
        var boardBuffer = new BoardBuffer(board);
        var display = new ConsoleDisplay(boardBuffer, score);
        var factory = new TetriminoFactory(random);
        var movementSourceFall = new MovementSourceConstantVelocity(new Vector2(0, newGameSettings.Speed / 10f));
        var game = new GameController(boardBuffer, factory, MovementRequestEventQueue, score, movementSourceFall);
        var displayController = new DisplayController(display);

        var cancellationTokenSource = new CancellationTokenSource();

        var gameTask = game.Run(cancellationTokenSource.Token);
        var displayTask = displayController.RunDisplayLoop(cancellationTokenSource.Token);

        await gameTask;
        await displayTask;
    }
}