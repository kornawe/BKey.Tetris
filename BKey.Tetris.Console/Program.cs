using BKey.Tetris.Logic;
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
        Game game = new Game(board, display, factory);
        game.Run();
    }
}
