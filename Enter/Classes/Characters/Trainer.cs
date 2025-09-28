using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Trainer
{

    // Might use a scale for tile lengths later
    private const float SpeedPxPerSec = 80f;
    private const float FrameDuration = 0.12f;
    private const float InteractionRange = 1f;
    private readonly float _visionRange = 4f;
    public enum Facing { Down, Up, Left, Right }
    private Facing _face = Facing.Down; // Facing direction
    public Vector2 Position { get; set; }
    private readonly bool _moving = false;  // Whether the trainer will hang around
    private bool _visible = false;  // Whether the trainer sees a player now

    public Trainer(Vector2 Pos, Facing face) : this(Pos, face, false, 4f) { }
    public Trainer(Vector2 Pos, Facing face, bool moving) : this(Pos, face, moving, 4f) { }
    public Trainer(Vector2 Pos, Facing face, float visionRange) : this(Pos, face, false, visionRange) { }
    public Trainer(Vector2 Pos, Facing face, bool moving, float visionRange)
    {
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
        }
        else Idle(gametime);
    }

    private bool IsVisible(Player player)
    {
        // Add vision blocking & position overlay mechanisms later
        Vector2 diff = player.Position - Position;
        bool xAligned = 0 == diff.X,
             yAligned = 0 == diff.Y,
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
        if (Vector2.DistanceSquared(player.Position, Position) <= InteractionRange)
        {
            _visible = false;
            // * Trigger Battle Scene *
            return; // Stop moving if in close range
        }
        Vector2 norm = Vector2.Normalize(player.Position - Position);   // X or Y already aligned
        float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
        Position += norm * SpeedPxPerSec * dt;
    }

    private void Idle(GameTime gametime)
    {
        if (_moving)
        {
            // TODO: random moving
        }
    }

}
