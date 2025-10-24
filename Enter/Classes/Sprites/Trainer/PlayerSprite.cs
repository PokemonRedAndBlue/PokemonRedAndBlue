using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Facing = Enter.Classes.Characters.Facing;

namespace Enter.Classes.Sprites;

public class PlayerSprite
{

    public const int SpriteSize = 16;
    private const float FrameDuration = 0.12f;
    private readonly List<Rectangle> sprites =
    [
        new(9,   34, SpriteSize, SpriteSize), // 0 - Down 1
        new(26,  34, SpriteSize, SpriteSize), // 1 - Down 2
        new(43,  34, SpriteSize, SpriteSize), // 2 - Down 3
        new(60,  34, SpriteSize, SpriteSize), // 3 - Up 1
        new(77,  34, SpriteSize, SpriteSize), // 4 - Up 2
        new(94,  34, SpriteSize, SpriteSize), // 5 - Up 3
        new(111, 34, SpriteSize, SpriteSize), // 6 - Left 1
        new(128, 34, SpriteSize, SpriteSize), // 7 - Left 2
        new(145, 34, SpriteSize, SpriteSize), // 8 - Right 1
        new(162, 34, SpriteSize, SpriteSize), // 9 - Right 2
    ];
    private int currentFrameIndex = 0;
    private float animTimer = 0f; // animationTimer

    public void UpdateMovAnim(Facing facing, float dt)
    {
        animTimer += dt;
        // Cycle through the frames for the current facing
        if (animTimer >= FrameDuration)
        {
            animTimer -= FrameDuration;

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

    public void IdleReset(Facing facing)
    {
        currentFrameIndex = IdleFrameFor(facing);
        animTimer = 0f;

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

    public void Draw(SpriteBatch spriteBatch, Texture2D texture, float scale, Vector2 Position)  // TODO: check window bounds
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

}
