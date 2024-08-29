using System;
using System.Threading;
using System.Threading.Tasks;

namespace BKey.Tetris.Logic.Game;
public interface IGameController : IDisposable
{
    Task Run(CancellationToken cancellationToken);
}