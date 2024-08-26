using BKey.Tetris.Logic;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Console;
public class DisplayController
{
    private IGameDisplay display;

    public DisplayController( IGameDisplay display)
    {
        this.display = display;
    }

    public async Task RunDisplayLoop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            display.Draw();
            await Task.Delay(32);
        }
    }
}
