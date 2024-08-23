using System;

namespace BKey.Tetris.Console.Menu;
public class MenuOption
{
    public string Name { get; }
    public Action Action { get; }

    public MenuOption(string name, Action action)
    {
        Name = name;
        Action = action;
    }
}
