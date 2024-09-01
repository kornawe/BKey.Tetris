using System;

namespace BKey.Tetris.Logic.Events;

public interface IEventBus
{
    void Subscribe<T>(Action<T> handler) where T : class;
    void Unsubscribe<T>(Action<T> handler) where T : class;
    void Publish<T>(T eventMessage) where T : class;
}