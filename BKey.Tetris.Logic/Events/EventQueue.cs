
using System;
using System.Collections.Generic;

namespace BKey.Tetris.Logic.Events;

public class EventQueue<T> : IDisposable where T : class
{

    private IEventBus EventBus { get; }
    private Queue<T> Items { get; }
    private bool disposedValue;

    public EventQueue(IEventBus eventBus)
    {
        EventBus = eventBus;
        Items = new Queue<T>();
        EventBus.Subscribe<T>(HandleEvent);
    }

    private void HandleEvent(T e)
    {
        lock (Items)
        {
            Items.Enqueue(e);
        }
    }

    public T Dequeue() {
        lock (Items)
        {
            return Items.Dequeue();
        }
    }

    public T[] DequeueAll() {
        lock (Items)
        {
            var results = new T[Items.Count];
            Items.CopyTo(results, 0);
            Items.Clear();
            return results;
        }
    }

    public int Count
    {
        get
        {
            lock (Items)
            {
                return Items.Count;
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            EventBus.Unsubscribe<T>(HandleEvent);
            Items.Clear();
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