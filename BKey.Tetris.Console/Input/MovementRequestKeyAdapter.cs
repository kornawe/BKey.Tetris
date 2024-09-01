using BKey.Tetris.Logic.Events;
using BKey.Tetris.Logic.Input;
using System;
using System.Collections.Generic;

namespace BKey.Tetris.Console.Input;
/// <summary>
/// Listens for <see cref="KeyPressEvent"/> and publishes <see cref="MovementRequestEvent"/>s.
/// </summary>
public class MovementRequestKeyAdapter : IDisposable
{
    private KeyEventAdapter<MovementRequest, MovementRequestEvent> KeyEventAdapter { get; }

    static Dictionary<ConsoleKey, MovementRequest> GameKeyMappings = new Dictionary<ConsoleKey, MovementRequest>
        {
            { ConsoleKey.LeftArrow, MovementRequest.Left},
            { ConsoleKey.RightArrow, MovementRequest.Right },
            { ConsoleKey.UpArrow, MovementRequest.Rotate },
            { ConsoleKey.DownArrow, MovementRequest.Down }
        };

    private bool disposedValue;

    public MovementRequestKeyAdapter(IEventBus eventBus)
    {
        KeyEventAdapter = new KeyEventAdapter<MovementRequest, MovementRequestEvent>(
            eventBus,
            GameKeyMappings,
            CreateEvent
            );
    }

    private static MovementRequestEvent CreateEvent(MovementRequest menuRequestType)
    {
        return new MovementRequestEvent(menuRequestType);
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