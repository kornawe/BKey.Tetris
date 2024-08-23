using BKey.Tetris.Logic.Input;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Console.Input;
internal class ConsoleInputQueue : IInputQueue
{
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly Task listeningTask;

    private MovementRequest MovementRequest { get; set; }
    private bool disposedValue;

    public ConsoleInputQueue()
    {
        cancellationTokenSource = new CancellationTokenSource();
        listeningTask = Task.Run(ListenForInput, cancellationTokenSource.Token);
    }

    public bool IsEmpty => MovementRequest == MovementRequest.None;

    private async Task ListenForInput()
    {
        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {
            var key = System.Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    MovementRequest = MovementRequest.Left;
                    break;
                case ConsoleKey.RightArrow:
                    MovementRequest = MovementRequest.Right;
                    break;
                case ConsoleKey.UpArrow:
                    MovementRequest = MovementRequest.Rotate;
                    break;
                case ConsoleKey.DownArrow:
                    MovementRequest = MovementRequest.Down;
                    break;
            }

            await Task.Delay(10); // Small delay to prevent busy waiting
        }
    }

    public void Clear()
    {
        MovementRequest = MovementRequest.None;
    }

    public MovementRequest? Dequeue()
    {
        var value = MovementRequest;
        MovementRequest = MovementRequest.None;
        return value;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                listeningTask?.Dispose();
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
