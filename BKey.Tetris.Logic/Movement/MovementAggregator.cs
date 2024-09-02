
using System;
using System.Collections.Generic;

namespace BKey.Tetris.Logic.Movement;

public class MovementAggregator
{
    private readonly IEnumerable<IMovementSource> movementSources;
    private Vector2 accumulatedVelocity;

    public MovementAggregator(IEnumerable<IMovementSource> movementSources)
    {
        this.movementSources = movementSources;
        accumulatedVelocity = Vector2.Origin;
    }

    public IntVector2 Update(TimeSpan deltaT)
    {
        var totalVelocity = Vector2.Origin;

        // Sum up the velocities from all movement sources
        foreach (var source in movementSources)
        {
            source.Update(deltaT);
            totalVelocity += source.Velocity;
        }

        // Accumulate the velocity with the new velocity
        accumulatedVelocity += totalVelocity * (float)deltaT.TotalSeconds;

        // Apply integer movement to the game piece
        var result = new IntVector2(
            (int)Math.Floor(accumulatedVelocity.X),
            (int)Math.Floor(accumulatedVelocity.Y)
        );

        // Retain the fractional part of the velocity for the next update
        accumulatedVelocity -= result;

        return result;
    }

    public void Reset()
    {
        accumulatedVelocity = Vector2.Origin;
    }
}
