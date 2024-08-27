
using System.Collections;
using System.Collections.Generic;

namespace BKey.Tetris.Logic.Events;

public class EventQueue<T> : IEnumerable<T>, IReadOnlyCollection<T> where T : class
{

    private IEventBus EventBus { get; }
    private Queue<T> Items { get; }

    public EventQueue(IEventBus eventBus)
    {
        EventBus = eventBus;
        Items = new Queue<T>();
    }

    private void HandleEvent(T e)
    {
        lock (Items)
        {

        }
    }
}