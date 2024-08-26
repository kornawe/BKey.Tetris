﻿using BKey.Tetris.Console.Input;
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
        var keyListener = new KeyListener();
        var listenTask = keyListener.StartListeningAsync();

        var runner = Task.Run(() =>
        {
            foreach (var keyEvent in keyListener.GetEvents())
            {
                System.Console.WriteLine($"{keyEvent.Timestamp}: {keyEvent.EventType} - {keyEvent.Key}");
            }
        });

        System.Console.WriteLine("Press 'Q' to stop listening.");

        while (true)
        {
            if (System.Console.KeyAvailable && System.Console.ReadKey(intercept: true).Key == ConsoleKey.Q)
            {
                keyListener.StopListening();
                break;
            }
            await Task.Delay(100);
        }
        System.Console.WriteLine("Key listening stopping.");
        await runner;
        System.Console.WriteLine("Key listening stopped.");
        return;

        var version = System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString(3) ?? string.Empty;
        var menuCancellationSource = new CancellationTokenSource();
        var mainMenuController = new MenuController(menuCancellationSource.Token);

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

    static Task CreateStartGameMenu(MenuController menuController, CancellationTokenSource cancellationTokenSource) {
        menuController.Push(new MenuItemList([
            new MenuItemText("New Game"),
            new MenuItemText("Width"),
            new MenuItemText("Height"),
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
        using var inputQueue = new ConsoleInputQueue<MovementRequest>(GameKeyMappings);
        var game = new GameController(boardBuffer, factory, inputQueue, score);
        var displayController = new DisplayController(display);

        var gameTask = game.Run();
        var displayTask = displayController.RunDisplayLoop(new System.Threading.CancellationToken());

        await gameTask;
        await displayTask;
    }

}
