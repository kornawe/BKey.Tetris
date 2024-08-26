using System;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public interface IMenuItem
{

    public bool Selectable { get; }

    public void Display(bool isActive);
    public void Select();

    public void HandleInput(MenuRequest menuRequest);

    public void HandleInput(string data);
}