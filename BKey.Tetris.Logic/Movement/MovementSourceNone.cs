using System;

namespace BKey.Tetris.Logic.Movement;

public class MovementSourceNone : IMovementSource
{
    public Vector2 Velocity { get; } = Vector2.Origin;

    public MovementSourceNone()
    {
    }

    public void Update(TimeSpan deltaT)
    {
    }
}