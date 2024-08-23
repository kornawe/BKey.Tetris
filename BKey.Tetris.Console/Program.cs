using BKey.Tetris.Console.Input;
using BKey.Tetris.Logic;
using BKey.Tetris.Logic.Game;
using BKey.Tetris.Logic.Tetrimino;
using System;
using System.Threading.Tasks;

namespace BKey.Tetris.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        var random = new Random();
        IBoard board = new Board(10, 20);
        IDisplay display = new ConsoleDisplay(board);
        ITetriminoFactory factory = new TetriminoFactory(random);
        using var inputQueue = new ConsoleInputQueue();
        var game = new GameControllerStated(board, display, factory, inputQueue);
        await game.Run();
    }
}
