using System;
using System.Threading.Tasks;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public interface IMenuItem : IDisposable
{

    public bool Selectable { get; }

    public Task Display(bool isActive);
    public Task Select();

    public Task HandleInput(MenuRequestType menuRequest);

    public Task HandleInput(string data);
}