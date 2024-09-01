namespace BKey.Tetris.Console.Input;

public class MenuRequestEvent
{
    public MenuRequestType RequestType{ get; set; }
    public bool Handled { get; private set; }

    public MenuRequestEvent(MenuRequestType requestType) {
        RequestType = requestType;
    }

    public void Handle()
    {
        Handled = true;
    }
}