using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Facing = Enter.Classes.Characters.Facing;

namespace Enter.Classes.Sprites;

public class TrainerSprite
{

    private const float FrameDuration = 0.12f;
    private readonly List<Rectangle> _sprites;
    private int _currentFrameIndex = 0;
    private float _animTimer = 0f;

    public TrainerSprite() : this(0) { }
    public TrainerSprite(int spriteIndex)
    {
        _sprites = new TrainerSpriteFactory(spriteIndex).Sprites;
    }

    public void UpdateMovAnim(Facing facing)
    {
        // Cycle through the frames for the current facing
        if (_animTimer >= FrameDuration)
        {
            _animTimer -= FrameDuration;

            var (start, count) = AnimRangeFor(facing);
            // Advance within that sub-range
            if (_currentFrameIndex < start || _currentFrameIndex >= start + count)
                _currentFrameIndex = start;
            else
                _currentFrameIndex++;

            if (_currentFrameIndex >= start + count)
                _currentFrameIndex = start;
        }
    }

    private static (int start, int count) AnimRangeFor(Facing facing)
    {
        return facing switch
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
        _currentFrameIndex = IdleFrameFor(facing);
        _animTimer = 0f;
    }

    // Also add nonmoving trainer sprites later
    private static int IdleFrameFor(Facing facing)
    {
        return facing switch
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

        Rectangle src = _sprites[_currentFrameIndex];

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
