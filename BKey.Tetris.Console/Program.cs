using BKey.Tetris.Console.Input;
using BKey.Tetris.Console.Menu;
using BKey.Tetris.Console.Setup;
using BKey.Tetris.Logic.Board;
using BKey.Tetris.Logic.Game;
using BKey.Tetris.Logic.Input;
using BKey.Tetris.Logic.Settings;
using Microsoft.Extensions.DependencyInjection;
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

    private static IServiceProvider ServiceProvider { get; }

    static Program()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddTetrisConsole();

        ServiceProvider = services.BuildServiceProvider();
    }

    static async Task Main(string[] args)
    {
        using var userScope = ServiceProvider.CreateScope();
        var keyListener = userScope.ServiceProvider.GetRequiredService<KeyListener>();
        keyListener.Attach();

        var version = System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString(3) ?? string.Empty;
        var menuCancellationSource = new CancellationTokenSource();
        var keyBindingProvider = new KeyBindingProvider();
        var mainMenuController = userScope.ServiceProvider.GetRequiredService<MenuController>();

        var mainMenu = new MenuItemList([
            new MenuItemText("Da Shape Game"),
            new MenuItemText(version),
            new MenuItemAction("New Game", async () => {
                    await CreateStartGameMenu(userScope.ServiceProvider, mainMenuController);
                }),
            new MenuItemAction("Quit", menuCancellationSource.CancelAsync),
        ]);

        mainMenuController.Push(mainMenu);
        await mainMenuController.Run(menuCancellationSource.Token);
    }

    static Task CreateStartGameMenu(IServiceProvider serviceProvider, MenuController menuController)
    {
        var settings = new NewGameSettings();

        menuController.Push(new MenuItemList([
            new MenuItemText("New Game"),
            new MenuItemInputInt(
                "Width",
                new SettingProvider<BoardCreateOptions, int>(settings.BoardCreateOptions, s => s.Width)),
            new MenuItemInputInt(
                "Height",
                new SettingProvider<BoardCreateOptions, int>(settings.BoardCreateOptions, s => s.Height)),
                new MenuItemInputInt(
                "Speed",
                new SettingProvider<NewGameSettings, int>(settings, s => s.Speed)),
            new MenuItemInputInt(
                "Seed",
                new SettingProvider<NewGameSettings, int>(settings, s => s.Seed)),
            new MenuItemAction("Start", async () => { await StartGame(serviceProvider, settings); }),
            new MenuItemBack(menuController),
        ]));
        return Task.CompletedTask;
    }

    public static async Task StartGame(IServiceProvider serviceProvider, NewGameSettings gameSettings)
    {
        var game = serviceProvider.GetRequiredService<Game>();
        await game.Run(gameSettings);
    }

}
