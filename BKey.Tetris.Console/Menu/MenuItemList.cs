using System;
using System.Collections.Generic;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public class MenuItemList : IMenuItem
{
    private List<IMenuItem> Items { get; }
    private int SelectedIndex { get; set; }

    public bool Selectable => true;

    public MenuItemList(IEnumerable<IMenuItem> menuItems)
    {
        Items = new List<IMenuItem>(menuItems);
        SelectedIndex = -1;
        Down();
    }

    public void Display(bool isActive)
    {
        System.Console.Clear();
        for (var i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            item.Display(i == SelectedIndex);
            System.Console.WriteLine();
        }
    }

    public void Select()
    {
        if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
        {
            Items[SelectedIndex].Select();
        }
    }

    public void Up()
    {
        for (var i = SelectedIndex - 1; i >= 0; i--)
        {
            if (Items[i].Selectable)
            {
                SelectedIndex = i;
                break;
            }
        }
    }

    public void Down()
    {
        for (var i = SelectedIndex + 1; i < Items.Count; i++)
        {
            if (Items[i].Selectable)
            {
                SelectedIndex = i;
                break;
            }
        }
    }

    public void Back()
    {
        // Do Nothing
    }

    public void HandleInput(MenuRequest menuRequest)
    {
        switch (menuRequest)
        {
            case MenuRequest.None:
                break;
            case MenuRequest.Up:
                Up();
                break;
            case MenuRequest.Down:
                Down();
                break;
            case MenuRequest.Select:
                Select();
                break;
            default:
                break;
        }
    }

    public void HandleInput(string data)
    {
        if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
        {
            Items[SelectedIndex].HandleInput(data);
        }
    }
}