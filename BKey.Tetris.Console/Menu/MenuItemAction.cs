using System;
using System.Threading.Tasks;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public class MenuItemAction : IMenuItem
{
    private bool disposedValue;

    private string Text { get; }
    private Func<Task> Action { get; }

    public bool Selectable => true;

    public MenuItemAction(
        string text,
        Func<Task> action)
    {
        Text = text;
        Action = action;
    }

    public Task Display(bool isActive)
    {
        if (isActive)
        {
            System.Console.Write($"> {Text}");
        }
        else
        {
            System.Console.Write($"  {Text}");
        }
        return Task.CompletedTask;
    }

    public async Task Select()
    {
        await Action();
    }

    public Task HandleInput(MenuRequestType menuRequest)
    {
        return Task.CompletedTask;
    }

    public Task HandleInput(string data)
    {
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