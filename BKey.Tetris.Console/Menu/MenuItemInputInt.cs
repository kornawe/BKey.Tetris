using System;
using System.Threading.Tasks;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public class MenuItemInputInt : IMenuItem
{
    private bool disposedValue;

    private string Tag { get; }
    private int Min { get; }
    private int Max { get; }
    private int Step { get; }

    private int Value { get; set; }
    private bool IsSelected { get; set; }

    public bool Selectable => true;

    public MenuItemInputInt(
        string text,
        int min,
        int max,
        int step,
        int initialValue)
    {
        Tag = text;
        Value = initialValue;
        Min = min;
        Max = max;
        Step = step;
    }

    public Task Display(bool isActive)
    {
        if (isActive && IsSelected)
        {
            System.Console.Write($"> {Tag} [{Value}]▲▼");
        }
        else if (isActive)
        {
            System.Console.Write($"> {Tag} [{Value}]");
        }
        else
        {
            System.Console.Write($"  {Tag} [{Value}]");
        }
        
        return Task.CompletedTask;
    }

    public Task HandleInput(MenuRequestEvent menuRequest)
    {
        switch (menuRequest.RequestType)
        {
            case MenuRequestType.Select:
                IsSelected = !IsSelected;
                menuRequest.Handle();
                break;
            case MenuRequestType.Up:
                if (IsSelected)
                {
                    Value = Math.Min(Value + Step, Max);
                    menuRequest.Handle();
                }
                break;
            case MenuRequestType.Down:
                if (IsSelected)
                {
                    Value = Math.Max(Value - Step, Min);
                    menuRequest.Handle();
                }
                break;
            case MenuRequestType.Back:
                if (IsSelected)
                {
                    IsSelected = false;
                    menuRequest.Handle();
                }
                break;
        }

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