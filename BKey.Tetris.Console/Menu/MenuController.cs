using BKey.Tetris.Console.Input;
using BKey.Tetris.Logic.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Console.Menu;
internal class MenuController : IDisposable
{

    private IMenuItem MenuItem { get; }
    private IInputQueue<MenuRequest> MenuInput { get; }
    private CancellationToken CancellationToken { get; }
    private bool disposedValue;

    static Dictionary<ConsoleKey, MenuRequest> MenuKeyMappings = new Dictionary<ConsoleKey, MenuRequest>
        {
            { ConsoleKey.UpArrow, MenuRequest.Up},
            { ConsoleKey.DownArrow, MenuRequest.Down },
            { ConsoleKey.Enter, MenuRequest.Select },
            { ConsoleKey.Escape, MenuRequest.Back }
        };

    public MenuController(
        IMenuItem menuList,
        CancellationToken cancellationToken)
        : this(
            menuList,
            new ConsoleInputQueue<MenuRequest>(MenuKeyMappings),
            cancellationToken)
    {
    }

    public MenuController(
        IMenuItem menuItem,
        IInputQueue<MenuRequest> menuInput,
        CancellationToken cancellationToken)
    {
        MenuItem = menuItem;
        MenuInput = menuInput;
        CancellationToken = cancellationToken;
    }

    public async Task Run()
    {
        while (!CancellationToken.IsCancellationRequested)
        {
            var menuRequest = MenuInput.Dequeue();
            if (menuRequest != MenuRequest.None)
            {
                MenuItem.HandleInput(menuRequest);
                MenuItem.Display(true);
            }

            await Task.Delay(50);
        }
        MenuInput.Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                MenuInput.Dispose();
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
        GC.SuppressFinalize(this);
    }
}
