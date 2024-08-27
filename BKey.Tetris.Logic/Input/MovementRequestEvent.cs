namespace BKey.Tetris.Logic.Input;
public class MovementRequestEvent
{
    public MovementRequest Request { get; }

    public MovementRequestEvent(MovementRequest request)
    {
        Request = request;
    }
}
