
# PokemonRedAndBlue

## Project Description
PokemonRedAndBlue is a MonoGame-based, Pokémon-inspired game. Players explore a route, visit a town with a Pokémon Center and Gym, and battle trainers and leaders. The game features 15–20 Pokémon, each with up to four moves, simplified stats, and basic AI. The project is designed for clarity and learning, with a focus on core gameplay and readable code.

## Key Documentation Update
- Most planning and documentation is now on our [public Jira board](https://cse3902group6.atlassian.net/jira/software/projects/SCRUM/boards/1?atlOrigin=eyJpIjoiNzllMGE4ZjUxYTMxNDM2ODliZmQ4NGQ2NzdlMjEzNzgiLCJwIjoiaiJ9)
- Pull requests are managed via GitHub
- The backlog is maintained in [`backlog.md`](backlog.md)

## Project Structure
- `Enter/` — Main game code: scenes, UI, sprites, textures, content pipeline, and XML data
  - `Classes/` — All core classes, organized by function (Animations, Behavior, Cameras, Music, Physics, Scenes, Sprites, Textures, etc.)
  - `Interfaces/` — Key interfaces (ISprite, IController, IPlayer, IScene, IState)
  - `Content/` — XML data, maps, and pipeline files
- `Content/` — Game assets, maps, XML data, and content pipeline files
- `obj/`, `bin/` — Build output and intermediate files
- Floating files: `Program.cs`, `Core.cs`, `Game.cs` (to be organized in a future refactor)

## Build & Run
### Requirements
- [.NET 8.0+ SDK](https://dotnet.microsoft.com/en-us/download)
- [MonoGame Framework](https://www.monogame.net/)
- Visual Studio or VS Code with MonoGame support

### Instructions
1. Clone the repository.
2. Open the solution in Visual Studio and set **Enter** as the startup project, or use VS Code.
3. Run the program, or use the terminal:
   ```bash
   dotnet run --project Enter/Enter.csproj
   ```
4. WHEN TESTING BATTLE NOTE: ONLY RUN AND FIGHT ARE IMPLEMENTED THE OTHERS WILL SHOW BLANK UI WHICH YOU CAN EXIT WITH TAB

## Content Pipeline
XML and image assets are managed via the MonoGame Content Pipeline. To rebuild content:
```
mgcb-editor /build Content/Content.mgcb
```
Or use the CLI:
```
dotnet mgcb build Content/Content.mgcb
```

## Key Features
- Tile-based collision system that prevents the player from walking through solid objects
- Accurate map boundary detection that keeps the player within the playable world
- Scene handling for battles and wild encounters
- Turn-based battle UI with state machine
- Health bar logic: color-only (green/yellow/red, always full width) or percent-based (scaled by HP)
- Sprite and texture atlas system
- XML-driven data for Pokémon, moves, maps, etc.
- Music and sound effect code/assets present (see below)

## Controls
| Action                        | Key(s)         |
|-------------------------------|----------------|
| Move player                   | Arrow Keys     |
| Exit game                     | Escape         |
| Exit a battle scene           | Tab            |
| Enter Wild Encounter Scene    | W              |
| Enter Trainer Battle          | Approach trainer from the side |
| Transport to Gym              | G              |
| Transport to City             | C              |
| Reset Game                    | R              |

## Known Bugs & Limitations
- Collision tiles are manually defined, requiring updates to TileCollisionProfile.cs whenever new tiles are added
- Data handling for Pokémon and trainers is spread out over multiple classes (needs centralization)
- Some merges occurred without full code review or did not follow the review template
- Music and sound effect code/assets are present, but not yet fully implemented in gameplay

## Planned Improvements
- Add clear separation between walkable, solid, and interactive tiles within TileCollisionProfile.cs
- Centralize Pokémon and trainer data handling
- Refactor floating files into appropriate subfolders
- Improve code review process and consistency
- Integrate music and sound effects into gameplay
- Add dialogue and more animations for battle scenes
- Expand XML-driven content (Pokémon, moves, maps)

## Recent Developments
- Health bars now use color-only logic (green/yellow/red) and reflect local HP values
- Instructional messages:
  - In Fight: "Press A to use Tackle" (drawn in black, 50px higher)
  - In Menu: "Use arrow keys to navigate and Enter to select" (drawn in black, 50px higher)
- Random damage, critical/miss messages, and win/lose color feedback
- No more reflection or temp objects for HP bar drawing; now uses explicit HP values
- Refactored UI code for clarity and maintainability
- Music and sound effect libraries and assets added (not yet fully implemented)
- Added full Pokémon data systems: move loading, stat generation (IV-based), and species catch rates.
- Implemented core battle math including damage calculation (STAB, typing, physical/special) and trainer AI move selection.
- Added catching probability logic and separate generation paths for wild vs. trainer Pokémon.

## Backlog
See [`backlog.md`](backlog.md) for current backlog, sprint summaries, and future plans.

## License
See LICENSE file.
