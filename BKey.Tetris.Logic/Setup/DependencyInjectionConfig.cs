
using BKey.Tetris.Logic.Board;
using BKey.Tetris.Logic.Events;
using Microsoft.Extensions.DependencyInjection;

namespace BKey.Tetris.Logic.Setup;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddTetrisLogic(this IServiceCollection services)
    {
        // Singletons
        services.AddSingleton<IBoardFactory, BoardFactory>();

        // Scoped services - One per user
        services.AddScoped<IEventBus, SimpleEventBus>();
        services.AddScoped(typeof(IEventQueue<>), typeof(EventQueue<>));

        return services;
    }
}