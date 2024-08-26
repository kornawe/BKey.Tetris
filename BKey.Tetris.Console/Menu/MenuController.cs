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

    private Stack<IMenuItem> MenuStack {get; }
    private IInputQueue<MenuRequestType> MenuInput { get; }
    private CancellationToken CancellationToken { get; }
    private bool IsDirty { get; set; }
    private bool disposedValue;

    static Dictionary<ConsoleKey, MenuRequestType> MenuKeyMappings = new Dictionary<ConsoleKey, MenuRequestType>
        {
            { ConsoleKey.UpArrow, MenuRequestType.Up},
            { ConsoleKey.DownArrow, MenuRequestType.Down },
            { ConsoleKey.Enter, MenuRequestType.Select },
            { ConsoleKey.Escape, MenuRequestType.Back }
        };

    public MenuController(
        CancellationToken cancellationToken)
        : this(
            new ConsoleInputQueue<MenuRequestType>(MenuKeyMappings),
            cancellationToken)
    {
    }

    public MenuController(
        IInputQueue<MenuRequestType> menuInput,
        CancellationToken cancellationToken)
    {
        MenuStack = new Stack<IMenuItem>();
        MenuInput = menuInput;
        CancellationToken = cancellationToken;
        IsDirty = true;
    }

    public void Push(IMenuItem menuItem) {
        MenuStack.Push(menuItem);
        IsDirty = true;
    }

    public async Task Run()
    {
        if (MenuStack.Count == 0) {
            return;
        }

        while (!CancellationToken.IsCancellationRequested)
        {
            if (IsDirty && MenuStack.Count > 0) {
                await MenuStack.Peek().Display(true);
                IsDirty = false;
            }

            var request = MenuInput.Dequeue();
            if (request == MenuRequestType.None)
            {
                await Task.Delay(50);
                continue;
            }

            var menuRequest = await ForwardRequest(request);
            if (!menuRequest.Handled) {
                switch (request)
                {
                    case MenuRequestType.Back:
                        await GoBack();
                        break;
                }
            }

        }
        MenuInput.Dispose();
    }

    private async Task<MenuRequestEvent> ForwardRequest(MenuRequestType request) {
        var menuRequest = new MenuRequestEvent(request);

        if (MenuStack.Count == 0) {
            return menuRequest;
        }
        var menuItem = MenuStack.Peek();
        await menuItem.HandleInput(menuRequest);
        if (menuRequest.Handled)
        {
            IsDirty = true;
        }
        return menuRequest;
    }

    private async Task GoBack() {
        if (MenuStack.Count <= 1) {
            return;
        }
        var lastMenuItem = MenuStack.Pop();
        lastMenuItem.Dispose();
        await MenuStack.Peek().Display(true);
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
