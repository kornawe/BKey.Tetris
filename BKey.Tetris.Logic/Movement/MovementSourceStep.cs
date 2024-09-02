using System;

namespace BKey.Tetris.Logic.Movement;

public class MovementSourceStep : IMovementSource
{
    private Vector2 stepVelocity;
    private Vector2 currentVelocity;
    private TimeSpan stepDuration;

    public MovementSourceStep()
    {
        stepVelocity = new Vector2(0, 0);
        currentVelocity = new Vector2(0, 0);
        stepDuration = TimeSpan.Zero;
    }

    public Vector2 Velocity => currentVelocity;

    public void Update(TimeSpan deltaT)
    {
        if (stepDuration > TimeSpan.Zero)
        {
            // Calculate the velocity required to achieve the step within the given deltaT
            currentVelocity = stepVelocity / (float)deltaT.TotalSeconds;

            // Reduce the step duration by the elapsed time
            stepDuration -= deltaT;
        }
        else
        {
            // Reset the velocity when the step is complete
            currentVelocity = new Vector2(0, 0);
        }
    }

    public void ApplyStep(int stepX, int stepY, TimeSpan stepTime)
    {
        stepVelocity = new Vector2(stepX, stepY);
        stepDuration = stepTime;
    }
}
