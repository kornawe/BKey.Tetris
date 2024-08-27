
using System;
using System.Collections.Generic;
using System.Linq;

namespace BKey.Tetris.Logic.Events;

public class SimpleEventBus : IEventBus
{
    private readonly Dictionary<Type, List<WeakReference>> _handlers = new();

    public void Subscribe<T>(Action<T> handler) where T : class
    {
        ArgumentNullException.ThrowIfNull(handler);

        if (!_handlers.TryGetValue(typeof(T), out var handlers))
        {
            handlers = new List<WeakReference>();
            _handlers[typeof(T)] = handlers;
        }

        handlers.Add(new WeakReference(handler));
    }

    public void Unsubscribe<T>(Action<T> handler) where T : class
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        if (_handlers.TryGetValue(typeof(T), out var handlers))
        {
            var weakReferenceToRemove = handlers.FirstOrDefault(wr => wr.Target as Action<T> == handler);
            if (weakReferenceToRemove != null)
            {
                handlers.Remove(weakReferenceToRemove);
            }
        }
    }

    public void Publish<T>(T eventMessage) where T : class
    {
        ArgumentNullException.ThrowIfNull(eventMessage);

        if (_handlers.TryGetValue(eventMessage.GetType(), out var handlers))
        {
            var handlersToInvoke = new List<Action<T>>();
            var handlersToRemove = new List<WeakReference>();

            foreach (var weakHandler in handlers)
            {
                if (weakHandler.Target is Action<T> handler)
                {
                    handlersToInvoke.Add(handler);
                }
                else
                {
                    handlersToRemove.Add(weakHandler);
                }
            }

            // Remove dead handlers
            foreach (var deadHandler in handlersToRemove)
            {
                handlers.Remove(deadHandler);
            }

            // Invoke live handlers
            foreach (var handler in handlersToInvoke)
            {
                handler(eventMessage);
            }
        }
    }
}