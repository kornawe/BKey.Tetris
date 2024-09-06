
using BKey.Tetris.Console.Input;
using BKey.Tetris.Console.Menu;
using BKey.Tetris.Logic.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace BKey.Tetris.Console.Setup;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddTetrisConsole(this IServiceCollection services)
    {
        services.AddTetrisLogic();

        services.AddScoped<IKeyBindingProvider, KeyBindingProvider>();
        services.AddScoped<KeyListener>();
        services.AddScoped<MenuController>();
        services.AddScoped<MovementRequestKeyAdapter>();
        services.AddScoped<Game>();
        return services;
    }
}