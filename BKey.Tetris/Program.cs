namespace BKey.Tetris;

internal class Program
{
    static void Main(string[] args)
    {
        IBoard board = new Board(10, 20);
        IDisplay display = new ConsoleDisplay(board);
        Game game = new Game(board, display);
        game.Run();
    }
}
