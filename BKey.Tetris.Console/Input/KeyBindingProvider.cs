
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BKey.Tetris.Console.Input;

public class KeyBindingProvider : IKeyBindingProvider
{
    private Dictionary<string, Dictionary<string, List<string>>> MappingSections { get; }

    public KeyBindingProvider()
    {
        var pathRoot = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var path = Path.Join(pathRoot, "Resources/keybinding.json");
        var json = File.ReadAllText(path);
        MappingSections = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(json)
            ?? new();
    }

    public IReadOnlyDictionary<ConsoleKey, TRequest> GetBinding<TRequest>() where TRequest : struct, Enum
    {
        // Dictionary to store the key mappings
        var keyMappings = new Dictionary<ConsoleKey, TRequest>();

        var section = GetSection<TRequest>();

        if (section == null) {
            return keyMappings;
        }

        // Parse the MenuMapping section
        foreach (var entry in MappingSections[section])
        {
            foreach (var key in entry.Value)
            {
                keyMappings.Add(ParseConsoleKey(key), ParseRequestType<TRequest>(entry.Key));
            }
        }

        // Output the parsed dictionary to verify
        foreach (var mapping in keyMappings)
        {
            System.Console.WriteLine($"{mapping.Key} => {mapping.Value}");
        }

        return keyMappings;
    }

    private static string? GetSection<TRequest>()
    {
        var requestType = typeof(TRequest).Name;
        return requestType
            .Replace("Request", "")
            .Replace("Type", "");
    }

        // Helper method to parse ConsoleKey from string
    private static ConsoleKey ParseConsoleKey(string key)
    {
        return Enum.TryParse(key, out ConsoleKey result) 
            ? result
            : throw new ArgumentException($"Invalid key: {key}");
    }

    // Helper method to parse MenuRequestType from string
    private static TRequest ParseRequestType<TRequest>(string requestType) where TRequest : struct, Enum
    {
        return Enum.TryParse(requestType, out TRequest result)
            ? result
            : throw new ArgumentException($"Invalid request type: {requestType}");
    }
}