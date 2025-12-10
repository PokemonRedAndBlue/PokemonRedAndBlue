
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

## Controls
### Action  Key(s)
- Move player:  Arrow Keys
- Exit game:  Escape
- Exit the battle scene with trainer: Tab
- Enter Wild Encounter Scene - 'w' key press (only a debug feature but if you cant get one by chance test this way!)
- Enter Trainer Battle - approach the trainer from the side
- Walk to the bottom of Route 1 to get to Cerulean City
- Walk to the left of Cerulean City to get to Route 1
- Walk to the door of the Gym within the Cerulean City to get into the Gym

## Controls
| Action                        | Key(s)         |
|-------------------------------|----------------|
| Move player                   | Arrow Keys     |
| Change Move in Battle         | Arrow L and R  |
| Exit game                     | Escape         |
| Force Exit a battle scene     | Tab            |
| Enter Wild Encounter Scene    | W              |
| Enter Trainer Battle          | Approach trainer from the side |
| Reset Game                    | R              |

## Known Bugs & Limitations
- You cannot go from the city back to the overworld, and when you come from the overworld to the city you are   facing down instead of right
- Bag/Pokémon submenus are stubbed/blank in battle (tab out to exit)
- Limited move set and AI depth; balance is minimal

## Recent Improvements
- Added player deploy pokéball animation
- Enlarged battle/instruction text and improved effectiveness messaging layout
- Treated warnings as errors and removed unused/useless fields/usings to keep builds clean
- Dynamic movesets and damage; damage is calculated based on type level and move!
- Custom gym leader and 'pokemon'
- Title Screen and multiple intros!

## If We Had More Time
- Flesh out Bag/Pokémon/Run submenus with real interactions and UI states
- Add richer AI, move pools, and status effects
- Implement overworld backtracking (city <-> route) and polish spawn/orientation persistence
- Add battle transitions, camera polish, and additional animations (hit, faint, idle variants)
- Introduce save/load slots and in-game settings (audio, speed, accessibility)

## Additional Doccumentation
- Demo Video: https://youtu.be/DWcQq7iNob4 (or attatched in carmen)
- Slides: attatched on carmen

## Backlog
No Backlog for this sprint yay! We did everything we planned from the begining (miraculously)! Just contains a few of out goals at the begining of the sprint.

## License
See LICENSE file.
