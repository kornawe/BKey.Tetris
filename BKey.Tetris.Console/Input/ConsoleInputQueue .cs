using BKey.Tetris.Logic.Input;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Console.Input;
internal class ConsoleInputQueue : IInputQueue
{
    private readonly ConcurrentQueue<InputRequest> queue;
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly Task listeningTask;
    private bool disposedValue;

    public ConsoleInputQueue()
    {
        queue = new ConcurrentQueue<InputRequest>();
        cancellationTokenSource = new CancellationTokenSource();
        listeningTask = Task.Run(ListenForInput, cancellationTokenSource.Token);
    }

    public void Enqueue(InputRequest item)
    {
        queue.Enqueue(item);
    }

    public bool TryDequeue(out InputRequest item)
    {
        return queue.TryDequeue(out item);
    }

    public bool IsEmpty => queue.IsEmpty;

    private async Task ListenForInput()
    {
        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {
            if (System.Console.KeyAvailable)
            {
                var key = System.Console.ReadKey(intercept: true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        Enqueue(InputRequest.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        Enqueue(InputRequest.Right);
                        break;
                    case ConsoleKey.UpArrow:
                        Enqueue(InputRequest.Rotate);
                        break;
                    case ConsoleKey.DownArrow:
                        Enqueue(InputRequest.Down);
                        break;
                }
            }

            await Task.Delay(10); // Small delay to prevent busy waiting
        }
    }

    public void Clear()
    {
        queue.Clear();
    }

    public InputRequest? Dequeue()
    {
        if (queue.TryDequeue(out var item)) {
            return item;
        }
        else
        {
            return null;
        }
    }

    public InputRequest? Peek()
    {
        throw new NotImplementedException();
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
            queue.Clear();
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
