using BKey.Tetris.Console.Input;
using BKey.Tetris.Console.Menu;
using BKey.Tetris.Logic;
using BKey.Tetris.Logic.Board;
using BKey.Tetris.Logic.Events;
using BKey.Tetris.Logic.Game;
using BKey.Tetris.Logic.Input;
using BKey.Tetris.Logic.Tetrimino;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Console;

internal class Program
{
    static Dictionary<ConsoleKey, MovementRequest> GameKeyMappings = new Dictionary<ConsoleKey, MovementRequest>
        {
            { ConsoleKey.LeftArrow, MovementRequest.Left},
            { ConsoleKey.RightArrow, MovementRequest.Right },
            { ConsoleKey.UpArrow, MovementRequest.Rotate },
            { ConsoleKey.DownArrow, MovementRequest.Down }
        };

    private static IEventBus EventBus { get; }

    static Program()
    {
        EventBus = new SimpleEventBus();
    }

    static async Task Main(string[] args)
    {
        using var keyListener = new KeyListener().Attach(EventBus);

        var version = System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString(3) ?? string.Empty;
        var menuCancellationSource = new CancellationTokenSource();
        var keyBindingProvider = new KeyBindingProvider();
        var mainMenuController = new MenuController(EventBus, keyBindingProvider, menuCancellationSource.Token);

        var mainMenu = new MenuItemList([
            new MenuItemText("Da Shape Game"),
            new MenuItemText(version),
            new MenuItemAction("New Game", async () => {
                    await CreateStartGameMenu(mainMenuController, menuCancellationSource);
                }),
            new MenuItemAction("Quit", menuCancellationSource.CancelAsync),
        ]);

        mainMenuController.Push(mainMenu);
        await mainMenuController.Run();
    }

    static Task CreateStartGameMenu(MenuController menuController, CancellationTokenSource cancellationTokenSource)
    {
        var settings = new NewGameSettings();

        menuController.Push(new MenuItemList([
            new MenuItemText("New Game"),
            new MenuItemInputInt("Width", 6, 80, 2, 10),
            new MenuItemInputInt("Height", 10, 40, 2, 20),
            new MenuItemText("Seed"),
            new MenuItemAction("Start", async () => { await StartGame(); }),
        ]));
        return Task.CompletedTask;
    }

    static async Task StartGame()
    {
        var random = new Random();
        IBoard board = new Board(10, 20);
        var score = new GameScore();
        var boardBuffer = new BoardBuffer(board);
        IGameDisplay display = new ConsoleDisplay(boardBuffer, score);
        ITetriminoFactory factory = new TetriminoFactory(random);
        using var movementKeyAdapter = new MovementRequestKeyAdapter(EventBus);
        using var inputQueue = new EventQueue<MovementRequestEvent>(EventBus);
        var game = new GameController(boardBuffer, factory, inputQueue, score);
        var displayController = new DisplayController(display);

        var cancellationTokenSource = new CancellationTokenSource();

        var gameTask = game.Run(cancellationTokenSource.Token);
        var displayTask = displayController.RunDisplayLoop(cancellationTokenSource.Token);

        await gameTask;
        await displayTask;
    }

}
