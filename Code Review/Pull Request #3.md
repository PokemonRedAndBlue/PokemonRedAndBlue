### Author of Code Review:
Kai Fan

### Date of Code Review:
10/02/2025

### Sprint Number:
Sprint 2

### Name of .cs File(s) Being Reviewed:
    Game.cs
    KeyboardController.cs (almost the same from the one we had)
    Tiles.cs
    TileCycler.cs
    TileLoader.cs

### Author of the .cs File(s) Being Reviewed:
Arnav Malisetty

### Number of Minutes Taken to Complete Review:
25 min

# Comments
The directory isn't right in general. 
Also namespaces.
### Game.cs
    Need to improve on naming conventions.
    I like the ?. operation, new thing to learn for me.
### Tiles.cs
    Again naming conventions, use _varName for private fields.
    Good abstraction for Draw methods.
### TileCycler.cs
    Good ?? operation.
    Looks good to me in general. 
### TileLoader.cs
    Cool way to use block using.
    But definitely need some abstraction, too many lines of code in just one method. 