### Author of Code Review:
Arnav Malisetty

### Date of Code Review:
10/02/2025

### Sprint Number:
Sprint 2

### Name of .cs File(s) Being Reviewed:
Enter/Classes/Characters/Player.cs
Enter/Classes/Characters/Trainer.cs
Enter/Classes/Sprites/TrainerSprite.cs

### Author of the .cs File(s) Being Reviewed:
Kai Fan

### Number of Minutes Taken to Complete Review:
~30 minutes

--------------------------------------------------------------------------------------------------------

### Comments on Readability for Player.cs:
Enum/fields could be grouped at top; the isMoving parentheses fix is good and clarifies intent.

Add null checks for texture in Draw(); ensure seenByTrainer is reset where appropriate.

--------------------------------------------------------------------------------------------------------

### Comments on Readability for Trainer.cs:
Duplicate/conflicting constant and field declarations (InteractionRange, DefaultVisionRange, _moving, Position) make reading not super easy

--------------------------------------------------------------------------------------------------------

### Comments on Readability for TrainerSprite.cs:
Good separation of animation logic; consider making sprite table static readonly and document dependency on Trainer.Facing.

Ensure C# collection initializer syntax is used and Draw checks texture for null; otherwise animation stepping looks correct.