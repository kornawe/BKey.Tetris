using System;

namespace BKey.Tetris.Logic.Movement;

public class MovementSourceConstantAcceleration : IMovementSource
{
    private Vector2 acceleration;
    public Vector2 Velocity { get; private set; }

    public MovementSourceConstantAcceleration(Vector2 initialVelocity, Vector2 acceleration)
    {
        Velocity = initialVelocity;
        this.acceleration = acceleration;
    }

    public void Update(TimeSpan deltaT)
    {
        float deltaSeconds = (float)deltaT.TotalSeconds;
        Velocity += acceleration * deltaSeconds;
    }
}