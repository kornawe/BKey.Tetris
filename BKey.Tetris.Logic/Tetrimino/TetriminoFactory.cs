using System;
using System.Collections.Generic;
using System.Linq;

namespace BKey.Tetris.Logic.Tetrimino;
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

    public TetriminoPiece Create(TetriminoType type)
    {
        switch (type)
        {
            case TetriminoType.I:
                return new TetriminoPiece(
                    new int[,]
                    {
                        { 1, 1, 1, 1 }
                    },
                    ConsoleColor.Cyan
                );

            case TetriminoType.O:
                return new TetriminoPiece(
                    new int[,]
                    {
                        { 1, 1 },
                        { 1, 1 }
                    },
                    ConsoleColor.Yellow
                );

            case TetriminoType.T:
                return new TetriminoPiece(
                    new int[,]
                    {
                        { 0, 1, 0 },
                        { 1, 1, 1 }
                    },
                    ConsoleColor.Magenta
                );

            case TetriminoType.S:
                return new  (
                    new int[,]
                    {
                        { 0, 1, 1 },
                        { 1, 1, 0 }
                    },
                    ConsoleColor.Green
                );

            case TetriminoType.Z:
                return new TetriminoPiece(
                    new int[,]
                    {
                        { 1, 1, 0 },
                        { 0, 1, 1 }
                    },
                    ConsoleColor.Red
                );

            case TetriminoType.J:
                return new TetriminoPiece(
                    new int[,]
                    {
                        { 1, 0, 0 },
                        { 1, 1, 1 }
                    },
                    ConsoleColor.Blue
                );

            case TetriminoType.L:
                return new TetriminoPiece(
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

    public IEnumerable<TetriminoPiece> List()
    {
        return Enum.GetValues<TetriminoType>()
            .Select(t => Create(t))
            .ToList();
    }

    public TetriminoPiece Next()
    {
        var values = Enum.GetValues(typeof(TetriminoType));
        var randomType = (TetriminoType)values.GetValue(random.Next(values.Length));
        return Create(randomType);
    }
}
