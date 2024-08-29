
using System;
using System.Collections.Generic;
using System.Text.Json;
using BKey.Tetris.Logic.Events;

namespace BKey.Tetris.Console.Input;

public class KeyEventAdapter<TRequest, TEvent> : IDisposable where TEvent : class where TRequest : struct, Enum
{
    private IEventBus EventBus { get; }
    private Func<TRequest, TEvent> EventFactory { get; }
    private IReadOnlyDictionary<ConsoleKey, TRequest> KeyMapping { get; }


    private bool disposedValue;

    public KeyEventAdapter(
        IEventBus eventBus,
        IReadOnlyDictionary<ConsoleKey, TRequest> keyMapping,
        Func<TRequest, TEvent> eventFactory)
    {
        EventBus = eventBus;
        KeyMapping = keyMapping;
        EventFactory = eventFactory;
        EventBus.Subscribe<KeyPressEvent>(HandleKeyPress);
    }

    private void HandleKeyPress(KeyPressEvent keyPressEvent)
    {
        if (KeyMapping.TryGetValue(keyPressEvent.KeyInfo.Key, out var request))
        {
            var newEvent = EventFactory(request);
            EventBus.Publish(newEvent);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                EventBus.Unsubscribe<KeyPressEvent>(HandleKeyPress);
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