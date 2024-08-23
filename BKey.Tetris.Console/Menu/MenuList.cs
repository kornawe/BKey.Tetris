using System;
using System.Collections.Generic;
using System.Linq;

namespace BKey.Tetris.Console.Menu;
internal class MenuList
{
    public string Title { get; }
    public string? SubTitle { get; }

    private List<MenuOption> Options { get; }

    private int SelectedIndex { get; set; }

    private bool _isDirty = true;

    public MenuList(string title, string? subTitle = null)
    {
        Title = title;
        SubTitle = subTitle;
        Options = new List<MenuOption>();
    }

    public void Add(MenuOption option)
    {
        Options.Add(option);
    }

    public void Up()
    {
        if (SelectedIndex > 0)
        {
            SelectedIndex--;
            _isDirty = true;
        }
    }

    public void Down()
    {
        if (SelectedIndex < Options.Count - 1)
        {
            SelectedIndex++;
            _isDirty = true;
        }
    }

    public void Select()
    {
        _isDirty = true;
        Options.ElementAt(SelectedIndex).Action();
    }

    public void Display()
    {
        if (!_isDirty)
        {
            return;
        }

        System.Console.Clear();
        System.Console.WriteLine(Title);
        if (!string.IsNullOrEmpty(SubTitle))
        {
            System.Console.WriteLine(SubTitle);
        }
        
        System.Console.WriteLine();

        for (int i = 0; i < Options.Count; i++)
        {
            if (i == SelectedIndex)
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine($"> {Options[i].Name}");
                System.Console.ResetColor();
            }
            else
            {
                System.Console.WriteLine($"  {Options[i].Name}");
            }
        }
        _isDirty = false;
    }
}
