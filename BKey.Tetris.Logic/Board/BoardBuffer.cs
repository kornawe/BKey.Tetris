namespace BKey.Tetris.Logic.Board;
public class BoardBuffer
{
    private IBoard writeBoard;
    private IBoard readBoard;
    private readonly object lockObject = new object();

    public BoardBuffer(IBoard initialBoard)
    {
        writeBoard = initialBoard;
        readBoard = initialBoard.Clone();
    }

    public IBoard GetWriteBoard()
    {
        // TODO fix this locking.
        lock (lockObject)
        {
            return writeBoard;
        }
    }

    public IReadonlyBoard GetReadBoard()
    {
        lock (lockObject)
        {
            return readBoard.AsReadonly();
        }
    }

    public void SwapBuffers()
    {
        lock (lockObject)
        {
            readBoard = writeBoard.Clone();
        }
    }
}
