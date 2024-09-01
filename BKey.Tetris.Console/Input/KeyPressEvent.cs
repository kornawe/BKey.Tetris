using System;

namespace BKey.Tetris.Console.Input;

public class KeyPressEvent
{
    public ConsoleKeyInfo KeyInfo { get; }

    public KeyPressEvent(ConsoleKeyInfo keyInfo) {
        KeyInfo = keyInfo;
    }
}