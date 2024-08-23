using BKey.Tetris.Logic.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Console.Input;
internal class ConsoleInputQueue<T> : IInputQueue<T> where T : Enum
{
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly Task listeningTask;

    private T Request { get; set; }
    private bool disposedValue;

    private Dictionary<ConsoleKey, T> KeyMappings { get; }

    public ConsoleInputQueue(Dictionary<ConsoleKey, T> keyMappings)
    {
        cancellationTokenSource = new CancellationTokenSource();

        KeyMappings = keyMappings;

        listeningTask = Task.Run(ListenForInput, cancellationTokenSource.Token);

    }

    public bool IsEmpty => Request.Equals(default(T));

    private async Task ListenForInput()
    {
        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {
            var key = System.Console.ReadKey(intercept: true).Key;

            if (KeyMappings.TryGetValue(key, out T? movementRequest))
            {
                Request = movementRequest;
            }

            await Task.Delay(10); // Small delay to prevent busy waiting
        }
    }

    public void Clear()
    {
        Request = default(T)!;
    }

    public T Dequeue()
    {
        var value = Request;
        Request = default(T)!;
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
