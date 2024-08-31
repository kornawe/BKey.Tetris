using System;
using System.Threading.Tasks;
using BKey.Tetris.Console.Input;
using BKey.Tetris.Logic.Settings;

namespace BKey.Tetris.Console.Menu;

public class MenuItemInputInt : IMenuItem
{
    private bool disposedValue;

    private string Tag { get; }

    private ISettingProvider<int> SettingProvider { get; }

    private bool IsSelected { get; set; }

    public bool Selectable => true;

    public MenuItemInputInt(
        string text,
        ISettingProvider<int> settingProvider)
    {
        Tag = text;
        SettingProvider = settingProvider;
    }

    public Task Display(bool isActive)
    {
        var value = SettingProvider.Get();

        if (isActive && IsSelected)
        {
            System.Console.Write($"> {Tag} [{value}]▲▼");
        }
        else if (isActive)
        {
            System.Console.Write($"> {Tag} [{value}]");
        }
        else
        {
            System.Console.Write($"  {Tag} [{value}]");
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
                    SettingProvider.Set(SettingProvider.Get() + 1);
                    menuRequest.Handle();
                }
                break;
            case MenuRequestType.Down:
                if (IsSelected)
                {
                    SettingProvider.Set(SettingProvider.Get() - 1);
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