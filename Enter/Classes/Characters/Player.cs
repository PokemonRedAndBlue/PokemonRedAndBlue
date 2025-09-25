using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Player
{
    // TODO: Consider putting the sprite stuffs to a sprite class or state machine, otherwise this class is too large
    readonly List<Rectangle> sprites =
    [
        new(9, 34, 16, 16),   // 0 - Down 1
        new(26, 34, 16, 16),  // 1 - Down 2
        new(43, 34, 16, 16),  // 2 - Down 3
        new(60, 34, 16, 16),  // 3 - Up 1
        new(77, 34, 16, 16),  // 4 - Up 2
        new(94, 34, 16, 16),  // 5 - Up 3
        new(111, 34, 16, 16), // 6 - Left 1
        new(128, 34, 16, 16), // 7 - Left 2
        new(145, 34, 16, 16), // 8 - Right 1
        new(162, 34, 16, 16), // 9 - Right 2
    ];

    public Vector2 Position { get; set; } = Vector2.Zero;
    private bool seenByTrainer = false;
    private readonly Texture2D texture;
    private enum Facing { Down, Up, Left, Right }
    private Facing facing = Facing.Down;
    private int currentFrameIndex = 0;
    private float animTimer = 0f; // animationTimer

    private readonly float _speedPxPerSec = 80f; // movement speed
    private readonly float _frameDuration = 0.12f;

    public Player(GameWindow Window) : this(null, Window) { }

    public Player(Vector2 position) : this(null, position) { }

    public Player(Texture2D texture2, GameWindow Window)
    {
        Position = new Vector2(Window.ClientBounds.X, Window.ClientBounds.Y) * 0.5f;
        texture = texture2;
    }
    public Player(Texture2D texture2, Vector2 position)
    {
        Position = position;
        texture = texture2;
    }

    public void Update(GameTime gameTime, int axisX, int axisY)
    {
        // Determine if moving, and which way we face
        bool isMoving = (!seenByTrainer) && axisX != 0 || axisY != 0;

        // If both non-zero ever slip through, pick a priority (down/up over left/right)
        if (axisX != 0 && axisY != 0)
        {
            // Choose one; here we prefer vertical
            axisX = 0;
        }

        if (axisY < 0) facing = Facing.Up;
        else if (axisY > 0) facing = Facing.Down;
        else if (axisX < 0) facing = Facing.Left;
        else if (axisX > 0) facing = Facing.Right;

        if (isMoving)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Movement
            // Normalize axis (already unit in each axis), move in pixels/sec
            Position += new Vector2(axisX, axisY) * _speedPxPerSec * dt;

            // Animation
            animTimer += dt;
            UpdateMovAnim();
        }
        else
        {
            // Idle frame for the facing direction
            currentFrameIndex = IdleFrameFor(facing);
            animTimer = 0f;
        }
    }

    private void UpdateMovAnim()
    {
        // Cycle through the frames for the current facing
        if (animTimer >= _frameDuration)
        {
            animTimer -= _frameDuration;

            var (start, count) = AnimRangeFor(facing);
            // Advance within that sub-range
            if (currentFrameIndex < start || currentFrameIndex >= start + count)
                currentFrameIndex = start;
            else
                currentFrameIndex++;

            if (currentFrameIndex >= start + count)
                currentFrameIndex = start;
        }
    }

    private static (int start, int count) AnimRangeFor(Facing f)
    {
        return f switch
        {
            Facing.Down => (0, 3),  // 0..2
            Facing.Up => (3, 3),  // 3..5
            Facing.Left => (6, 2),  // 6..7
            Facing.Right => (8, 2),  // 8..9
            _ => (0, 1),
        };
    }

    private static int IdleFrameFor(Facing f)
    {
        return f switch
        {
            Facing.Down => 0,
            Facing.Up => 3,
            Facing.Left => 6,
            Facing.Right => 8,
            _ => 0,
        };
    }

    public void Draw(SpriteBatch spriteBatch, float scale)  // TODO: check window bounds
    {
        if (texture == null) return; // no texture yet

        Rectangle src = sprites[currentFrameIndex];

        // Center the drawn sprite on Position (optional; adjust to your layout)
        var origin = Vector2.Zero;
        var destPos = Position; // top-left; change to center if preferred

        var destRect = new Rectangle(
            (int)destPos.X,
            (int)destPos.Y,
            (int)(src.Width * scale),
            (int)(src.Height * scale)
        );

        spriteBatch.Draw(texture, destRect, src, Color.White, 0f, origin, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Call this method when player enters the trainer's vision range. 
    /// </summary>
    public void Stop()
    {
        seenByTrainer = true;
    }

    /// <summary>
    /// Call this method when any other stage ends or interrupts the trainer going to player process. 
    /// </summary>
    public void StopEnd()
    {
        seenByTrainer = false;
    }

}
