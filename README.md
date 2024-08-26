# BKey.Tetris
A game to practice some dev skills.

# Building the Project

Project supports both Windows and Linux

## Prerequisites

- .Net 8.0 SDK
  - https://dotnet.microsoft.com/en-us/download/dotnet/8.0

## Visual Studio

1. Open the .sln
2. Set the startup project to BKey.Tetris.Console
3. Build
4. Debug

## Visual Studio Code

1. Install the C# Dev Kit
  - https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit
2. Run and Debug

## Command Line

1. `dotnet build`
2. `dotnet run --project BKey.Tetris.Console`

# Upcoming work
- Loss detection
  - Fix locking in BoardBuffer
- Logging
- New game configuration
- Automatic Piece falling
- Next piece queue
- Smooth animation
- Colored pieces once placed
- Pause menu
- Sound (Maybe)
  - Event Bus?
- Adjustable keybinding
- App versioning
- Unit Tests
- CI/CD
- Game saving and loading
- Adding users
- Leader boards
- Web UI