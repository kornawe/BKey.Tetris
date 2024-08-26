using System;
using System.Threading;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public class MenuItemAction : IMenuItem
{
    private string Text { get; }
    private Action Action { get; }
    private CancellationTokenSource CancellationTokenSource { get; }

    public bool Selectable => true;

    public MenuItemAction(
        string text,
        Action action,
        CancellationTokenSource cancellationTokenSource)
    {
        Text = text;
        Action = action;
        CancellationTokenSource = cancellationTokenSource;
    }

    public void Display(bool isActive)
    {
        if (isActive)
        {
            System.Console.WriteLine($"> {Text}");
        }
        else
        {
            System.Console.WriteLine(Text);
        }
    }

    public void Select()
    {

    }

    public void HandleInput(MenuRequest menuRequest)
    {

    }

    public void HandleInput(string data)
    {

    }
}