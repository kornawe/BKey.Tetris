using System;

namespace BKey.Tetris.Logic.Game;
public interface IGameScore
{
    public DateTime Created { get; }
    public TimeSpan Elapsed { get; }
    public int LinesCleared { get; }
    public int PiecesPlaced { get; }
    public void AddLinesCleared(int lines);
    public void AddPiecePlaced();
}
