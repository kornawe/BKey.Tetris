using BKey.Tetris.Console.Input;
using BKey.Tetris.Logic.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKey.Tetris.Console.Menu;
internal class MenuController : IDisposable
{

    private MenuList MenuList { get; }
    private IInputQueue<MenuRequest> MenuInput { get; }
    private bool disposedValue;

    static Dictionary<ConsoleKey, MenuRequest> MenuKeyMappings = new Dictionary<ConsoleKey, MenuRequest>
        {
            { ConsoleKey.UpArrow, MenuRequest.Up},
            { ConsoleKey.DownArrow, MenuRequest.Down },
            { ConsoleKey.Enter, MenuRequest.Select },
            { ConsoleKey.Escape, MenuRequest.Back }
        };

    public MenuController (MenuList menuList) : this (menuList, new ConsoleInputQueue<MenuRequest>(MenuKeyMappings))
    {
    }

    public MenuController (MenuList menuList, IInputQueue<MenuRequest> menuInput)
    {
        MenuList = menuList;
        MenuInput = menuInput;
    }

    public async Task Run()
    {
        var keepErGoin = true;
        while (keepErGoin)
        {
            MenuList.Display();

            switch (MenuInput.Dequeue())
            {
                case MenuRequest.Up:
                    MenuList.Up();
                    break;
                case MenuRequest.Down:
                    MenuList.Down();
                    break;
                case MenuRequest.Select:
                    keepErGoin = false;
                    MenuList.Select();
                    break;
            }

            await Task.Delay(50);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                MenuInput.Dispose();
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
