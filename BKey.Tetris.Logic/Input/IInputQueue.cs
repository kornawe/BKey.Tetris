using System;

namespace BKey.Tetris.Logic.Input;
public interface IInputQueue : IDisposable
{
    public bool IsEmpty { get; }

    public void Clear();
    public MovementRequest? Dequeue();

}
