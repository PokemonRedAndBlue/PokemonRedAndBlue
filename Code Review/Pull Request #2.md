### Author of Code Review:
Arnav Malisetty

### Date of Code Review:
09/29/2025

### Sprint Number:
Sprint 2

### Name of .cs File(s) Being Reviewed:
    Enter/Classes/Characters/Player.cs
    Enter/Classes/Characters/Trainer.cs
    Enter/Game.cs

### Author of the .cs File(s) Being Reviewed:
Kai Fan

### Number of Minutes Taken to Complete Review:
~35 minutes

--------------------------------------------------------------------------------------------------------

### Comments for Player.cs:
Easy to read / understand (method names, structure, comments, consistent formatting).
Specific things that reduce readability (long-ish methods, nested conditionals, TODOs without context).
use of constants, magic numbers, and unguarded nulls

--------------------------------------------------------------------------------------------------------

### Comments for Trainer.cs:

could be more readable: enum placement and duplicated fields
Concrete fixes => remove duplicate declarations, ensure enum and fields grouped, unify naming style

Methods with multiple responsibilities/functions 

--------------------------------------------------------------------------------------------------------

### Comments on for Game.cs:
Tidy up the LoadContent()

correct use of Window.ClientBounds, correct update ordering (controller -> player -> trainer).
missing null checks