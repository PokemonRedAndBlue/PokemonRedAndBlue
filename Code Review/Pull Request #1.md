### Author of Code Review:
Wyatt Black

### Date of Code Review:
09/25/2025

### Sprint Number:
Sprint 2

### Name of .cs File(s) Being Reviewed:
    Player.cs
    KeyboardController.cs
    Game.cs

### Author of the .cs File(s) Being Reviewed:
Danial Chaudhry

### Number of Minutes Taken to Complete Review:
15 min

# Comments
### Player.cs
    The name conventions for private fields were not consistently followed;
    Field "List<Rectangle> sprites" missing identifier, maybe private?
    Update is working well, but some abstractions might be needed for functions like movements;
    Sprite stuffs definitely need to be put into a separate sprite class later;
### KeyboardController.cs
    Keyboard controller working good, might need a command interface later as a transition;
    Namespace incorrect.
### Game.cs
    Definitely need to put direction updates somewhere else, command class expected. 
