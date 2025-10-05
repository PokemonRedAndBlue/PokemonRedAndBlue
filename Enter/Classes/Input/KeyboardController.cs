using Enter.Classes.Characters;
using GameFile;
using Microsoft.Xna.Framework.Input;

namespace KeyboardController
{

    public enum Direction {None, Up, Down, Left, Right};

    public class KeyboardController
    {
        public Direction MoveDirection { get; set; } = Direction.None;
        public bool ResetRequested { get; private set; } = false;//added to reset 
        private KeyboardState prevState;
        private bool isInitialized = false;
        private Direction prevDirection = Direction.None;
        public void Update(Game1 game, Trainer trainer)
        {
            // Get the current state of keyboard input.
            KeyboardState keyboardState = Keyboard.GetState();
            if (!isInitialized)
            {
                prevState = keyboardState;
                isInitialized = true;
                MoveDirection = Direction.None;
            }

            // Check for reset key
            ResetRequested = IsNewlyDown(keyboardState, Keys.R); // R key for reset

            bool keyIsUp = keyboardState.IsKeyDown(Keys.Up);
            bool keyIsDown = keyboardState.IsKeyDown(Keys.Down);
            bool keyIsLeft = keyboardState.IsKeyDown(Keys.Left);
            bool keyIsRight = keyboardState.IsKeyDown(Keys.Right);

            Direction chosenDirection = ChooseDirection(keyboardState, keyIsUp, keyIsDown, keyIsLeft, keyIsRight);
            MoveDirection = chosenDirection;

            // Y => next tile
            if (IsNewlyDown(keyboardState, Keys.Y))
            {
                if (game?.TileCycler != null) game.TileCycler.Next();
            }

            // T => previous tile
            if (IsNewlyDown(keyboardState, Keys.T))
            {
                if (game?.TileCycler != null) game.TileCycler.Prev();
            }

            // O => previous trainer sprite
            if (IsNewlyDown(keyboardState, Keys.O)) trainer.PrevSprite();

            // P => next trainer sprite
            if (IsNewlyDown(keyboardState, Keys.P)) trainer.NextSprite();

            prevDirection = chosenDirection;
            prevState = keyboardState;

        }

        private Direction ChooseDirection(KeyboardState keyboardState, bool up, bool down, bool left, bool right)
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
            if (IsNewlyDown(keyboardState, Keys.Up)) return Direction.Up;
            if (IsNewlyDown(keyboardState, Keys.Right)) return Direction.Right;
            if (IsNewlyDown(keyboardState, Keys.Down)) return Direction.Down;
            if (IsNewlyDown(keyboardState, Keys.Left)) return Direction.Left;

            // If a new key is pressed while a key was already being pressed, will follow new key
            if (prevDirection != Direction.None && IsStillHeld(keyboardState, prevDirection))
                return prevDirection;

            // No key pressed
            return Direction.None;
        }

        private bool IsNewlyDown(KeyboardState current, Keys key)
        {
            return !prevState.IsKeyDown(key) && current.IsKeyDown(key);
        }
        
        private bool IsStillHeld(KeyboardState current, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return current.IsKeyDown(Keys.Up);
                case Direction.Down:
                    return current.IsKeyDown(Keys.Down);
                case Direction.Left:
                    return current.IsKeyDown(Keys.Left);
                case Direction.Right:
                    return current.IsKeyDown(Keys.Right);
                default:
                    return false;
            }
        }
    }
}