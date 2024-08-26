using BKey.Tetris.Console.Input;
using BKey.Tetris.Console.Menu;
using BKey.Tetris.Logic;
using BKey.Tetris.Logic.Board;
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

    static async Task Main(string[] args)
    {
        var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        var menuCancellationSource = new CancellationTokenSource();
        var mainMenu = new MenuItemList(new IMenuItem[] {
            new MenuItemText("Da Shape Game"),
            new MenuItemText(version),
            new MenuItemAction("New Game", StartNewGame, menuCancellationSource),
            new MenuItemAction("Quit", QuitGame, menuCancellationSource),
        });

        var mainMenuController = new MenuController(mainMenu, menuCancellationSource.Token);

        await mainMenuController.Run();

        await StartGame();
    }

    static async Task StartGame()
    {
        var random = new Random();
        IBoard board = new Board(10, 20);
        var score = new GameScore();
        var boardBuffer = new BoardBuffer(board);
        IGameDisplay display = new ConsoleDisplay(boardBuffer, score);
        ITetriminoFactory factory = new TetriminoFactory(random);
        using var inputQueue = new ConsoleInputQueue<MovementRequest>(GameKeyMappings);
        var game = new GameController(boardBuffer, factory, inputQueue, score);
        var displayController = new DisplayController(display);

        var gameTask = game.Run();
        var displayTask = displayController.RunDisplayLoop(new System.Threading.CancellationToken());

        await gameTask;
        await displayTask;
    }

    static private void StartNewGame()
    {

    }

    static private void QuitGame()
    {
        Environment.Exit(0);
    }
}
