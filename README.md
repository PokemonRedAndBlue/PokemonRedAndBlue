# PokemonRedAndBlue

## Project Description (Anticipated Final Game): 
  This project is a simplified Pokémon-style game built with MonoGame. Players will explore a route leading to a town featuring a Pokémon Center and a Gym, where they can battle a gym trainer and leader. The game includes around 15–20 Pokémon, each with up to four learnable attacks, simplified stats (no IVs, EVs, or natures), and basic trainer AI that selects moves randomly. To streamline development, moves with similar effects and animations are reused, and complex battle mechanics like accuracy variations, priority moves, and status effects are omitted for a smoother early-game experience.

## Project structure (key folders)
- We have 2 main folders within our root directory: Enter and Code Review
  - Within our Enter Directory we have a few key folders as well:
    - Classes which contains ALL classes
      - Deeper we have directorys for different sub classes:
        -Animations
        -Behavior
        -Sprites
        -Input
        -Content
    - Interfaces which contains our few interfaces as of now
      - ISprite
      - IController
      - IPlayer
    - Enter also has some floating cs files like Program, Core and Game
  - Code Review is where we will place all of our pull code review markup files
- Additionally we have a backlog.md in the root directory containing backlog info

> Note: `Program.cs`, `Core.cs`, and `Game.cs` are planned to be organized into appropriate subfolders in a future refactor.

## Build & Run  
### Requirements
- [.NET 6.0+ SDK](https://dotnet.microsoft.com/en-us/download)
- [MonoGame Framework](https://www.monogame.net/)
- Visual Studio or VS Code with MonoGame support

### Instructions
1. Clone the repository.  
2. Open the solution in Visual Studio and set **Enter** as the startup project.  
3. Run the program, or use the terminal:
   ```bash
   dotnet run --project Enter/Enter.csproj

## Key Features
- Fully animated player movement and overworld navigation
- Trainer AI that approaches when the player enters their line of sight (even with collision wowwwwww)
- Gen I starter Pokémon and their evolutions with front-attack animations
- A Poké Ball throw and capture wobble animation
- Ability to cycle tiles for variety and testing
- A cycling view of all pokemon back sprites

## Controls
### Action	Key(s)
- Move player:	Arrow Keys
- Reset game state:	R
- Cycle tiles:	Y / T
- Switch trainer sprite:	O / P
- Exit game:	Escape

## Known Bugs & Limitations
- Lacks a dedicated Pokemon class — management currently hard-coded
- Heavy reliance on magic numbers and static logic
- Missing proper state management — a state machine would improve structure
- AI and battle systems are minimal and unrefined

## Planned Improvements
### Short-Term Goals
- Implement Pokemon and Move classes for cleaner management
- Introduce a simple state machine (Overworld, Battle, Dialogue, etc.)
- Increase use of Dictionaries for move and species lookup
- Reduce hard-coded values and improve abstraction

### Long-Term Goals
- Add camera follow and smoother overworld transitions
- Expand move variety and visual polish
- Strengthen architecture using consistent design patterns (e.g., Command pattern)