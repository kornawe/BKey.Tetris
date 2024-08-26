using System;
using System.Threading.Tasks;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public class MenuItemText : IMenuItem
{
    private bool disposedValue;

    private string Text { get; }

    public bool Selectable => false;

    public MenuItemText(string text)
    {
        Text = text;
    }

    public Task Display(bool isActive)
    {
        System.Console.Write(Text);
        return Task.CompletedTask;
    }

    public Task Select()
    {
        // Do Nothing
        return Task.CompletedTask;
    }

    public Task HandleInput(MenuRequestType menuRequest)
    {
        // Do Nothing
        return Task.CompletedTask;
    }

    public Task HandleInput(string data)
    {
        // Do Nothing
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
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