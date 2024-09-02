
using System;

namespace BKey.Tetris.Logic.Movement;

public class MovementSourceConstantVelocity : IMovementSource
{
    public Vector2 Velocity { get; private set; }

    public MovementSourceConstantVelocity(Vector2 initialVelocity)
    {
        Velocity = initialVelocity;
    }

    public void Update(TimeSpan deltaT)
    {
        // Constant velocity means no change in velocity
    }
}