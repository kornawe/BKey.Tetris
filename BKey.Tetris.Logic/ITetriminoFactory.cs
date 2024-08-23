using System.Collections.Generic;

namespace BKey.Tetris.Logic;
public interface ITetriminoFactory
{
    Tetrimino Create(TetriminoType type);
    IEnumerable<Tetrimino> List();
    Tetrimino Next();
}