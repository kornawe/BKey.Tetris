
using System;
using System.Collections.Generic;

namespace BKey.Tetris.Console.Input;

public interface IKeyBindingProvider {

    public IReadOnlyDictionary<ConsoleKey, TRequest> GetBinding<TRequest>() where TRequest : struct, Enum;
}