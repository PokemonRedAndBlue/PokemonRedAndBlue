using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Player
{
    List<Rectangle> sprites = new List<Rectangle>
    {
        new Rectangle(9, 34, 16, 16), // 0 Down 1
        new Rectangle(26, 34, 16, 16), // 1 Down 2
        new Rectangle(43, 34, 16, 16), // 2 Down 3
        new Rectangle(60, 34, 16, 16), // 3 Up 1
        new Rectangle(77, 34, 16, 16), // 4 Up 2
        new Rectangle(94, 34, 16, 16), // 5 Up 3
        new Rectangle(111, 34, 16, 16), // 6 Left 1
        new Rectangle(128, 34, 16, 16), // 7 Left 2
        new Rectangle(145, 34, 16, 16), // 8 Right 1
        new Rectangle(162, 34, 16, 16), // 9 Right 2
    };

    public Vector2 Position { get; set; }
    private Texture2D texture;
    private enum Facing { Down, Up, Left, Right }
    private Facing facing = Facing.Down;
    private int currentFrameIndex = 0;
    private float animTimer = 0f; // animationTimer

     private float _speedPxPerSec = 80f; // movement speed
    private float _frameDuration = 0.12f;

    public Player(GameWindow Window)
    {
        Position = new Vector2(Window.ClientBounds.X, Window.ClientBounds.Y) * 0.5f;
    }

    public Player(Vector2 position)
    {
        Position = position;
    }

    public Player(Texture2D texture2, GameWindow Window)
    {
        Position = new Vector2(Window.ClientBounds.X, Window.ClientBounds.Y) * 0.5f;
        texture = texture2;
    }

    public void Update(GameTime gameTime, int axisX, int axisY)
    {
        // Determine if moving, and which way we face
        bool isMoving = axisX != 0 || axisY != 0;

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

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Movement
        if (isMoving)
        {
            Vector2 dir = new Vector2(axisX, axisY);
            // Normalize axis (already unit in each axis), move in pixels/sec
            Position += dir * _speedPxPerSec * dt;
        }

        // Animation
        animTimer += dt;
        if (isMoving)
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
        else
        {
            // Idle frame for the facing direction
            currentFrameIndex = IdleFrameFor(facing);
            animTimer = 0f;
        }
    }
    
    private (int start, int count) AnimRangeFor(Facing f)
    {
        switch (f)
        {
            case Facing.Down: return (0, 3); // 0..2
            case Facing.Up: return (3, 3); // 3..5
            case Facing.Left: return (6, 2); // 6..7
            case Facing.Right: return (8, 2); // 8..9
            default: return (0, 1);
        }
    }

    private int IdleFrameFor(Facing f)
    {
        switch (f)
        {
            case Facing.Down: return 0;
            case Facing.Up: return 3;
            case Facing.Left: return 6;
            case Facing.Right: return 8;
            default: return 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch, float scale)
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


    public void Stop()
    {
        // TODO: Clear command state
    }
    
}
