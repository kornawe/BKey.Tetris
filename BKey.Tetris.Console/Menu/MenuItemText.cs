using System;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public class MenuItemText : IMenuItem
{
    private string Text { get; }

    public bool Selectable => false;

    public MenuItemText(string text)
    {
        Text = text;
    }

    public void Display(bool isActive)
    {
        System.Console.Write(Text);
    }

    public void Select()
    {
        // Do Nothing
    }

    public void HandleInput(MenuRequest menuRequest)
    {
        // Do Nothing
    }

    public void HandleInput(string data)
    {
        // Do Nothing
    }
}