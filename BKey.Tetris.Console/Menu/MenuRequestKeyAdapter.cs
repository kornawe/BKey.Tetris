
using System;
using System.Collections.Generic;
using BKey.Tetris.Console.Input;
using BKey.Tetris.Logic.Events;

namespace BKey.Tetris.Console.Menu;

public class MenuRequestKeyAdapter
{
    private IEventBus EventBus { get; }

    static Dictionary<ConsoleKey, MenuRequestType> MenuKeyMappings = new Dictionary<ConsoleKey, MenuRequestType>
        {
            { ConsoleKey.UpArrow, MenuRequestType.Up},
            { ConsoleKey.DownArrow, MenuRequestType.Down },
            { ConsoleKey.Enter, MenuRequestType.Select },
            { ConsoleKey.Escape, MenuRequestType.Back }
        };

    public MenuRequestKeyAdapter(IEventBus eventBus)
    {
        EventBus = eventBus;
        EventBus.Subscribe<KeyPressEvent>(HandleKeyPress);
    }

    private void HandleKeyPress(KeyPressEvent keyPressEvent)
    {
        if (MenuKeyMappings.TryGetValue(keyPressEvent.KeyInfo.Key, out MenuRequestType movementRequest))
        {
            var menuRequestEvent = new MenuRequestEvent(movementRequest);
            EventBus.Publish(menuRequestEvent);
        }
    }
}