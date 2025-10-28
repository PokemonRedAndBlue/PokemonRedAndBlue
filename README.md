# PokemonRedAndBlue

## Project Description (Anticipated Final Game): 
  This project is a simplified Pokémon-style game built with MonoGame. Players will explore a route leading to a town featuring a Pokémon Center and a Gym, where they can battle a gym trainer and leader. The game includes around 15–20 Pokémon, each with up to four learnable attacks, simplified stats (no IVs, EVs, or natures), and basic trainer AI that selects moves randomly. To streamline development, moves with similar effects and animations are reused, and complex battle mechanics like accuracy variations, priority moves, and status effects are omitted for a smoother early-game experience.

## Key Update To Docummentation
  -Most of our doccumentation and planning has been moved to a Jira board which has been made public and is available at the following link:
  https://cse3902group6.atlassian.net/jira/software/projects/SCRUM/boards/1?atlOrigin=eyJpIjoiNzllMGE4ZjUxYTMxNDM2ODliZmQ4NGQ2NzdlMjEzNzgiLCJwIjoiaiJ9
  -Pull requests were all done via github

## Project structure (key folders)
- We have 2 main folders within our root directory: Enter and Code Review
  - Within our Enter Directory we have a few key folders as well:
    - Classes which contains ALL classes
      - Deeper we have directorys for different sub classes:
        -Animations
        -Behavior
        -Cameras
        -Physics
        -Scenes
        -Sprites
        -Input
        -Content
    - Interfaces which contains our few interfaces as of now
      - ISprite
      - IController
      - IPlayer
      - IScene
      - IState
    - Enter also has some floating cs files like Program, Core and Game
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
- Tile-based collision system that prevents the player from walking through solid objects
- Accurate map boundary detection that keeps the player within the playable world
- Scene handling for battles and wild encounters
- Background on player and trainer removed
- Pseudo route walkable by player and trainer approachable with triggerable battle

## Controls
### Action	Key(s)
- Move player:	Arrow Keys
- Exit game:	Escape
- Exit the battle scene with trainer: Tab
- Enter Wild Encounter Scene - 'w' key press
- Enter Trainer Battle - approach trainer from side

## Known Bugs & Limitations
- Collision tiles are manually defined, requiring updates to TileCollisionProfile.cs whenever new tiles are added

## Planned Improvements
- Add clear separation between walkable, solid, and interactive tiles within TileCollisionProfile.cs
- Overall get game functioning

### Short-Term Goals
- Add clear separation between walkable, solid, and interactive tiles within TileCollisionProfile.cs
- Preserve player position when entering a battle scene
- Use pokemon generator to make wild pokemon truly random (including stats etc)

### Long-Term Goals
- Integrate interaction zones (e.g., door triggers, tall grass, signposts) into the collision framework
- Add dialouge for trainers and battle scenes
- Integrate full animations for battle sequence
- Add sound
- Have pokemon appear after poke ball is thrown
