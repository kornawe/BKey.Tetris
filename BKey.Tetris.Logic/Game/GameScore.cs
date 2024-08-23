using System;

namespace BKey.Tetris.Logic.Game;
public class GameScore : IGameScore
{

    public TimeSpan Elapsed
    {
        get
        {
            return DateTime.Now - Created;
        }
    }
    public int LinesCleared { get; private set; }
    public int PiecesPlaced { get; private set; }
    public DateTime Created { get; }

    public GameScore()
    {
        Created = DateTime.Now;
        LinesCleared = 0;
        PiecesPlaced = 0;
    }


    public void AddLinesCleared(int lines)
    {
        LinesCleared += lines;
    }

    public void AddPiecePlaced()
    {
        PiecesPlaced++;
    }
}
