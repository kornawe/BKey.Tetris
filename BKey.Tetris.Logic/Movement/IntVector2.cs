
namespace BKey.Tetris.Logic.Movement;

public readonly struct IntVector2
{
    public int X { get; }
    public int Y { get; }

    public IntVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static IntVector2 Origin => new IntVector2(0, 0);

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X + b.X, a.Y + b.Y);
    }

    public static IntVector2 operator *(IntVector2 a, int scalar)
    {
        return new IntVector2(a.X * scalar, a.Y * scalar);
    }
}