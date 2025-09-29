using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Trainer
{

    // Might use a scale for tile lengths later
    private const float SpeedPxPerSec = 80f;
    private const float FrameDuration = 0.12f;
    private const float InteractionRange = 16f;
    private const float DefaultVisionRange = 64f;
    private const float AlignMOE = 4f;  // Margin of Error for aligning checks
    private readonly float _visionRange = DefaultVisionRange;
    public enum Facing { Down, Up, Left, Right }
    private Facing _face = Facing.Down; // Facing direction
    public Vector2 Position { get; set; }
    private readonly bool _moving = false;  // Whether the trainer will hang around
    private bool _visible = false;  // Whether the trainer sees a player now
    private readonly Texture2D _texture;

        // TODO: Move to Sprite class later
        readonly List<Rectangle> sprites =
        [
            new(9, 85, 16, 16),   // 0 - Down 1
            new(26, 85, 16, 16),  // 1 - Down 2
            new(43, 85, 16, 16),  // 2 - Down 3
            new(60, 85, 16, 16),  // 3 - Up 1
            new(77, 85, 16, 16),  // 4 - Up 2
            new(94, 85, 16, 16),  // 5 - Up 3
            new(111, 85, 16, 16), // 6 - Left 1
            new(128, 85, 16, 16), // 7 - Left 2
            new(145, 85, 16, 16), // 8 - Right 1
            new(162, 85, 16, 16), // 9 - Right 2
        ];
        private int _currentFrameIndex = 0;
        private float _animTimer = 0f;

    public Trainer(Texture2D texture, Vector2 Pos, Facing face) : this(texture, Pos, face, false) { }
    public Trainer(Texture2D texture, Vector2 Pos, Facing face, bool moving) : this(texture, Pos, face, moving, DefaultVisionRange) { }
    public Trainer(Texture2D texture, Vector2 Pos, Facing face, float visionRange) : this(texture, Pos, face, false, visionRange) { }
    public Trainer(Texture2D texture, Vector2 Pos, Facing face, bool moving, float visionRange)
    {
        _texture = texture;
        Position = Pos;
        _face = face;
        _moving = moving;
        _visionRange = visionRange;
    }

    public void Update(GameTime gametime, Player player)    // TODO: Sprites
    {
        if (!_visible && IsVisible(player)) _visible = true;    // decrease number of condition checks
        if (_visible)
        {
            player.Stop();
            GoToPlayer(player, gametime);
            UpdateMovAnim();
        }
        else
        {
            Idle(gametime);
            _currentFrameIndex = IdleFrameFor(_face);
            _animTimer = 0f;
        }
    }

    private bool IsVisible(Player player)
    {
        // Add vision blocking & position overlay mechanisms later
        Vector2 diff = player.Position - Position;
        bool xAligned = Math.Abs(diff.X) < AlignMOE,
             yAligned = Math.Abs(diff.Y) < AlignMOE,
             inVision = Math.Abs(Vector2.Distance(player.Position, Position)) < _visionRange;
        return _face switch
        {
            Facing.Up => xAligned && inVision && diff.Y < 0,
            Facing.Down => xAligned && inVision && diff.Y > 0,
            Facing.Left => yAligned && inVision && diff.X < 0,
            Facing.Right => yAligned && inVision && diff.X > 0,
            _ => throw new NotImplementedException(),
        };
    }

    private void GoToPlayer(Player player, GameTime gametime)
    {
        // Stop moving if it is nonmoving trainer, or in close range
        if (_moving || Vector2.DistanceSquared(player.Position, Position) <= InteractionRange)
        {
            _visible = false;
            // BattleMain.LoadScene();
            player.StopEnd();
            return;
        }
        Vector2 norm = Vector2.Normalize(player.Position - Position);   // X or Y already aligned
        float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
        Position += norm * SpeedPxPerSec * dt;
    }

        // TODO: Move to Sprite class later
        private void UpdateMovAnim()
        {
            // Cycle through the frames for the current facing
            if (_animTimer >= FrameDuration)
            {
                _animTimer -= FrameDuration;

                var (start, count) = AnimRangeFor(_face);
                // Advance within that sub-range
                if (_currentFrameIndex < start || _currentFrameIndex >= start + count)
                    _currentFrameIndex = start;
                else
                    _currentFrameIndex++;

                if (_currentFrameIndex >= start + count)
                    _currentFrameIndex = start;
            }
        }

        // TODO: Move to Sprite class later
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

        // TODO: Move to Sprite class later
        // Also add nonmoving trainer sprites later
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

    private void Idle(GameTime gametime)
    {
        if (_moving)
        {
            // Might add random moving later
        }
    }

        // TODO: Move to Sprite class later
        public void Draw(SpriteBatch spriteBatch, float scale)  // TODO: check window bounds
        {
            if (_texture == null) return; // no texture yet

            Rectangle src = sprites[_currentFrameIndex];

            // Center the drawn sprite on Position (optional; adjust to your layout)
            var origin = Vector2.Zero;
            var destPos = Position; // top-left; change to center if preferred

            var destRect = new Rectangle(
                (int)destPos.X,
                (int)destPos.Y,
                (int)(src.Width * scale),
                (int)(src.Height * scale)
            );

            spriteBatch.Draw(_texture, destRect, src, Color.White, 0f, origin, SpriteEffects.None, 0f);
        }

}
