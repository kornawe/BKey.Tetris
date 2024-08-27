
using System;
using System.Collections.Generic;
using BKey.Tetris.Console.Input;
using BKey.Tetris.Logic.Events;

namespace BKey.Tetris.Console.Menu;

/// <summary>
/// Listens for <see cref="KeyPressEvent"/> and publishes <see cref="MenuRequestEvent"/>s.
/// </summary>
public class MenuRequestKeyAdapter : IDisposable
{
    private KeyEventAdapter<MenuRequestType, MenuRequestEvent> KeyEventAdapter { get; }

    static Dictionary<ConsoleKey, MenuRequestType> MenuKeyMappings = new Dictionary<ConsoleKey, MenuRequestType>
        {
            { ConsoleKey.UpArrow, MenuRequestType.Up},
            { ConsoleKey.DownArrow, MenuRequestType.Down },
            { ConsoleKey.Enter, MenuRequestType.Select },
            { ConsoleKey.RightArrow, MenuRequestType.Select },
            { ConsoleKey.Escape, MenuRequestType.Back },
            { ConsoleKey.LeftArrow, MenuRequestType.Back },
        };

    private bool disposedValue;

    public MenuRequestKeyAdapter(IEventBus eventBus)
    {
        KeyEventAdapter = new KeyEventAdapter<MenuRequestType, MenuRequestEvent>(
            eventBus,
            MenuKeyMappings,
            CreateMenuEvent
            );
    }

    private static MenuRequestEvent CreateMenuEvent(MenuRequestType menuRequestType)
    {
        return new MenuRequestEvent(menuRequestType);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                KeyEventAdapter.Dispose();
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