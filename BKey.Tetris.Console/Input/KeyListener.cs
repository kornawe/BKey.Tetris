using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BKey.Tetris.Logic.Events;

namespace BKey.Tetris.Console.Input;

public class KeyListener : IDisposable
{
    private readonly HashSet<ConsoleKey> _pressedKeys;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private bool disposedValue;

    public KeyListener()
    {
        _pressedKeys = new HashSet<ConsoleKey>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public KeyListener Attach(IEventBus eventBus)
    {
        Task.Run(async () =>
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (!System.Console.KeyAvailable) {
                    await Task.Delay(10);
                    continue;
                }

                var keyInfo = System.Console.ReadKey(intercept: true);

                // Handle KeyDown event
                if (!_pressedKeys.Contains(keyInfo.Key))
                {
                    _pressedKeys.Add(keyInfo.Key);
                    eventBus.Publish(new KeyPressEvent(keyInfo));
                }

                // Handle KeyUp event
                _pressedKeys.RemoveWhere(k =>
                {
                    if (!System.Console.KeyAvailable)
                    {
                        return true;
                    }
                    return false;
                });
            }

            _pressedKeys.Clear();
        }, _cancellationTokenSource.Token);
        return this;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                _cancellationTokenSource?.Cancel();
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