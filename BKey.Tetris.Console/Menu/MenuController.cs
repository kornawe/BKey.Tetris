using BKey.Tetris.Console.Input;
using BKey.Tetris.Logic.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Console.Menu;
public class MenuController : IDisposable
{

    private CancellationToken CancellationToken { get; }

    private Stack<IMenuItem> MenuStack { get; }
    private MenuRequestKeyAdapter RequestKeyAdapter { get; }

    private IEventQueue<MenuRequestEvent> MenuRequestEventQueue { get; }
    private bool IsDirty { get; set; }
    private bool disposedValue;

    public MenuController(
        IEventBus eventBus,
        IKeyBindingProvider keyBindingProvider,
        CancellationToken cancellationToken)
    {
        MenuStack = new Stack<IMenuItem>();
        CancellationToken = cancellationToken;
        RequestKeyAdapter = new MenuRequestKeyAdapter(eventBus, keyBindingProvider.GetBinding<MenuRequestType>());
        MenuRequestEventQueue = new EventQueue<MenuRequestEvent>(eventBus);

        // Mark dirty so the first pass causes a redraw.
        IsDirty = true;
    }

    public void Push(IMenuItem menuItem)
    {
        MenuStack.Push(menuItem);
        IsDirty = true;
    }

    public async Task Run()
    {
        if (MenuStack.Count == 0)
        {
            return;
        }

        while (!CancellationToken.IsCancellationRequested)
        {
            if (IsDirty && MenuStack.Count > 0)
            {
                await MenuStack.Peek().Display(true);
                IsDirty = false;
            }

            var requests = MenuRequestEventQueue.DequeueAll();
            if (requests.Length == 0)
            {
                await Task.Delay(50);
                continue;
            }

            var request = requests[requests.Length - 1];

            var menuRequest = await ForwardRequest(request);
            if (!menuRequest.Handled)
            {
                switch (request.RequestType)
                {
                    case MenuRequestType.Back:
                        Back();
                        break;
                }
            }

        }
        MenuRequestEventQueue.Dispose();
    }

    private async Task<MenuRequestEvent> ForwardRequest(MenuRequestEvent request)
    {

        if (MenuStack.Count == 0)
        {
            return request;
        }
        var menuItem = MenuStack.Peek();
        await menuItem.HandleInput(request);
        if (request.Handled)
        {
            IsDirty = true;
        }
        return request;
    }

    public Task Back()
    {
        if (MenuStack.Count <= 1)
        {
            return Task.CompletedTask;
        }
        var lastMenuItem = MenuStack.Pop();
        lastMenuItem.Dispose();
        IsDirty = true;
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                MenuRequestEventQueue.Dispose();
                RequestKeyAdapter.Dispose();
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
