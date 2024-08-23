using BKey.Tetris.Logic;
using BKey.Tetris.Logic.Game;
using BKey.Tetris.Logic.Tetrimino;
using System;

namespace BKey.Tetris.Console;

internal class Program
{
    static void Main(string[] args)
    {
        var random = new Random();
        IBoard board = new Board(10, 20);
        IDisplay display = new ConsoleDisplay(board);
        ITetriminoFactory factory = new TetriminoFactory(random);
        GameController game = new GameController(board, display, factory);
        game.Run();
    }
}
