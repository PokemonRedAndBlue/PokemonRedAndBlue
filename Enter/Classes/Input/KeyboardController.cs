using Microsoft.Xna.Framework.Input;
using Enter.Classes.Characters;
using Enter.Classes.Behavior;
using Microsoft.Xna.Framework;

namespace Enter.Classes.Input;

public enum Direction {None, Up, Down, Left, Right};

public class KeyboardController
{
    public Direction MoveDirection { get; set; } = Direction.None;
    private KeyboardState prevState, currState;
    private bool isInitialized = false;
    private Direction prevDirection = Direction.None;
    public void Update(Game1 game, GameTime gameTime, Player player, Trainer trainer)
    {
        // Get the current state of keyboard input.
        currState = Keyboard.GetState();
        if (!isInitialized)
        {
            prevState = currState;
            isInitialized = true;
            MoveDirection = Direction.None;
        }

        bool keyIsUp = currState.IsKeyDown(Keys.Up),
             keyIsDown = currState.IsKeyDown(Keys.Down),
             keyIsLeft = currState.IsKeyDown(Keys.Left),
             keyIsRight = currState.IsKeyDown(Keys.Right);

        Direction chosenDirection = ChooseDirection(keyIsUp, keyIsDown, keyIsLeft, keyIsRight);
        MoveDirection = chosenDirection;

        Command.UpdateCommands(game, gameTime, this, player, trainer);

        prevDirection = chosenDirection;
        prevState = currState;
    }

    private Direction ChooseDirection(bool up, bool down, bool left, bool right)
    {
        int countDown = (up ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);

        // If only 1 key is held down
        if (countDown == 1)
        {
            if (up) return Direction.Up;
            if (down) return Direction.Down;
            if (left) return Direction.Left;
            if (right) return Direction.Right;
        }

        // If 2 keys are held down at the same time
        if (IsNewlyDown(Keys.Up)) return Direction.Up;
        if (IsNewlyDown(Keys.Right)) return Direction.Right;
        if (IsNewlyDown(Keys.Down)) return Direction.Down;
        if (IsNewlyDown(Keys.Left)) return Direction.Left;

        // If a new key is pressed while a key was already being pressed, will follow new key
        if (prevDirection != Direction.None && IsStillHeld(prevDirection))
            return prevDirection;

        // No key pressed
        return Direction.None;
    }

    public bool IsNewlyDown(Keys key)
    {
        return !prevState.IsKeyDown(key) && currState.IsKeyDown(key);
    }
    
    private bool IsStillHeld(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return currState.IsKeyDown(Keys.Up);
            case Direction.Down:
                return currState.IsKeyDown(Keys.Down);
            case Direction.Left:
                return currState.IsKeyDown(Keys.Left);
            case Direction.Right:
                return currState.IsKeyDown(Keys.Right);
            default:
                return false;
        }
    }
}
