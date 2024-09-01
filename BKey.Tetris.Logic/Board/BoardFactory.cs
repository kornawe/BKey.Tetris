namespace BKey.Tetris.Logic.Board;
public class BoardFactory : IBoardFactory
{
    public IBoard Create(BoardCreateOptions boardCreateOptions)
    {
        // TODO validate create options.
        return new Board(boardCreateOptions.Width, boardCreateOptions.Height);
    }
}
