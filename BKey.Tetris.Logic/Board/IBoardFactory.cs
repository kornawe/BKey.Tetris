namespace BKey.Tetris.Logic.Board;

public interface IBoardFactory
{
    IBoard Create(BoardCreateOptions boardCreateOptions);
}