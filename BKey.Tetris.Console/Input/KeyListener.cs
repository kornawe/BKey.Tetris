using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BKey.Tetris.Logic.Events;

namespace BKey.Tetris.Console.Input;

public class KeyListener
{
    private IEventBus EventBus { get; }
    private readonly HashSet<ConsoleKey> _pressedKeys;
    private CancellationTokenSource? _cancellationTokenSource;

    public KeyListener(IEventBus eventBus)
    {
        EventBus = eventBus;
        _pressedKeys = new HashSet<ConsoleKey>();
    }

    public Task StartListeningAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        return Task.Run(async () =>
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
                    EventBus.Publish(new KeyPressEvent(keyInfo));
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
    }

    public void StopListening()
    {
        _cancellationTokenSource?.Cancel();
    }

}