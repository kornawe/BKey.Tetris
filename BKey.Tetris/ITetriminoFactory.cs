using System.Collections.Generic;

namespace BKey.Tetris;
public interface ITetriminoFactory
{
    Tetrimino Create(TetriminoType type);
    IEnumerable<Tetrimino> List();
    Tetrimino Next();
}