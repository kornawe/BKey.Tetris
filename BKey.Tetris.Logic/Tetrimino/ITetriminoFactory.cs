using System.Collections.Generic;

namespace BKey.Tetris.Logic.Tetrimino;
public interface ITetriminoFactory
{
    TetriminoPiece Create(TetriminoType type);
    IEnumerable<TetriminoPiece> List();
    TetriminoPiece Next();
}