using System;

namespace BKey.Tetris.Logic.Input;
public interface IInputQueue<T> : IDisposable where T : Enum
{
    public bool IsEmpty { get; }

    public T? Dequeue();

}
