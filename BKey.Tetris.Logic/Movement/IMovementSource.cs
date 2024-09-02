
using System;

namespace BKey.Tetris.Logic.Movement;

public interface IMovementSource
{
    Vector2 Velocity { get; }
    void Update(TimeSpan deltaT);
}