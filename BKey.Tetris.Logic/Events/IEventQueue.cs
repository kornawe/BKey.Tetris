using System;

namespace BKey.Tetris.Logic.Events;

public interface IEventQueue<T> : IDisposable where T : class
{
    int Count { get; }

    T Dequeue();
    T[] DequeueAll();
}