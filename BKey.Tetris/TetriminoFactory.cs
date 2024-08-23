using System;
using System.Collections.Generic;
using System.Linq;

namespace BKey.Tetris;
public class TetriminoFactory : ITetriminoFactory
{
    private Random random;

    public TetriminoFactory() : this(new Random())
    {
    }

    public TetriminoFactory(Random random)
    {
        this.random = random;
    }

    public Tetrimino Create(TetriminoType type)
    {
        switch (type)
        {
            case TetriminoType.I:
                return new Tetrimino(
                    new int[,]
                    {
                        { 1, 1, 1, 1 }
                    },
                    ConsoleColor.Cyan
                );

            case TetriminoType.O:
                return new Tetrimino(
                    new int[,]
                    {
                        { 1, 1 },
                        { 1, 1 }
                    },
                    ConsoleColor.Yellow
                );

            case TetriminoType.T:
                return new Tetrimino(
                    new int[,]
                    {
                        { 0, 1, 0 },
                        { 1, 1, 1 }
                    },
                    ConsoleColor.Magenta
                );

            case TetriminoType.S:
                return new Tetrimino(
                    new int[,]
                    {
                        { 0, 1, 1 },
                        { 1, 1, 0 }
                    },
                    ConsoleColor.Green
                );

            case TetriminoType.Z:
                return new Tetrimino(
                    new int[,]
                    {
                        { 1, 1, 0 },
                        { 0, 1, 1 }
                    },
                    ConsoleColor.Red
                );

            case TetriminoType.J:
                return new Tetrimino(
                    new int[,]
                    {
                        { 1, 0, 0 },
                        { 1, 1, 1 }
                    },
                    ConsoleColor.Blue
                );

            case TetriminoType.L:
                return new Tetrimino(
                    new int[,]
                    {
                        { 0, 0, 1 },
                        { 1, 1, 1 }
                    },
                    ConsoleColor.DarkYellow
                );

            default:
                throw new ArgumentOutOfRangeException(nameof(type), "Invalid Tetrimino type");
        }
    }

    public IEnumerable<Tetrimino> List()
    {
        return Enum.GetValues<TetriminoType>()
            .Select(t => Create(t))
            .ToList();
    }

    public Tetrimino Next()
    {
        var values = Enum.GetValues(typeof(TetriminoType));
        var randomType = (TetriminoType)values.GetValue(random.Next(values.Length));
        return Create(randomType);
    }
}
