namespace BKey.Tetris;

internal class Program
{
    static void Main(string[] args)
    {
        IBoard board = new Board(10, 20);
        IDisplay display = new ConsoleDisplay(board);
        ITetriminoFactory factory = new TetriminoFactory();
        Game game = new Game(board, display, factory);
        game.Run();
    }
}
