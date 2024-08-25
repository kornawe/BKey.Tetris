﻿using BKey.Tetris.Console.Input;
using BKey.Tetris.Console.Menu;
using BKey.Tetris.Logic;
using BKey.Tetris.Logic.Board;
using BKey.Tetris.Logic.Game;
using BKey.Tetris.Logic.Input;
using BKey.Tetris.Logic.Tetrimino;
using System;
using System.Collections.Generic;
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
        var mainMenu = new MenuList("Da Shape Game", version);
        mainMenu.Add(new MenuOption("New Game", StartNewGame));
        mainMenu.Add(new MenuOption("Quit", QuitGame));

        var mainMenuController = new MenuController(mainMenu);

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
