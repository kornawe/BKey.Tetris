using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public class MenuItemList : IMenuItem
{
    private bool disposedValue;

    private List<IMenuItem> Items { get; }
    private int SelectedIndex { get; set; }

    public bool Selectable => true;

    public MenuItemList(IEnumerable<IMenuItem> menuItems)
    {
        Items = new List<IMenuItem>(menuItems);
        SelectedIndex = -1;
        Down();
    }

    public Task Display(bool isActive)
    {
        System.Console.Clear();
        for (var i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            item.Display(i == SelectedIndex);
            System.Console.WriteLine();
        }
        return Task.CompletedTask;
    }

    private void Up()
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

    private void Down()
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

    public async Task HandleInput(MenuRequestEvent menuRequest)
    {
        if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
        {
            await Items[SelectedIndex].HandleInput(menuRequest);
        }
        if (menuRequest.Handled) {
            return;
        }

        switch (menuRequest.RequestType)
        {
            case MenuRequestType.Up:
                Up();
                menuRequest.Handle();
                break;
            case MenuRequestType.Down:
                Down();
                menuRequest.Handle();
                break;
        }
    }

    public async Task HandleInput(string data)
    {
        if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
        {
            await Items[SelectedIndex].HandleInput(data);
        }
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
            Items.Clear();
            SelectedIndex = -1;
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