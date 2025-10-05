using System;
using Enter.Classes.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Trainer
{

    // Might use a scale for tile lengths later
    public Vector2 Position { get; set; }

    private const float SpeedPxPerSec = 80f,
        InteractionRange = 64f, // might change based on scale?
        DefaultVisionRange = 256f,
        AlignMOE = 4f;  // Margin of Error for aligning checks
    private readonly float _visionRange = DefaultVisionRange;
    private readonly bool _moving = false;  // Whether the trainer will hang around when idling
    private readonly Texture2D _texture;
    private TrainerSprite _sprite;
    private int _spriteIndex;
    private Facing _facing = Facing.Down; // Facing direction
    private bool _visible = false;  // Whether the trainer sees a player now

    public Trainer(Texture2D texture, Vector2 Pos, Facing facing) : this(texture, Pos, facing, false) { }
    public Trainer(Texture2D texture, Vector2 Pos, Facing facing, bool moving) : this(texture, 0, Pos, facing, moving) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, bool moving) : this(texture, spriteIndex, Pos, facing, moving, DefaultVisionRange) { }
    public Trainer(Texture2D texture, Vector2 Pos, Facing facing, float visionRange) : this(texture, 0, Pos, facing, visionRange) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, float visionRange) : this(texture, spriteIndex, Pos, facing, false, visionRange) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, bool moving, float visionRange)
    {
        _texture = texture;
        _spriteIndex = spriteIndex;
        _sprite = new(_spriteIndex);
        Position = Pos;
        _facing = facing;
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
            _sprite.UpdateMovAnim(_facing);
        }
        else
        {
            Idle(gametime);
            _sprite.IdleReset(_facing);
        }
    }

    private bool IsVisible(Player player)
    {
        // Add vision blocking & position overlay mechanisms later
        Vector2 diff = player.Position - Position;
        bool xAligned = Math.Abs(diff.X) < AlignMOE,
             yAligned = Math.Abs(diff.Y) < AlignMOE,
             inVision = Math.Abs(Vector2.Distance(player.Position, Position)) < _visionRange;
        if (inVision) return InVisionRange(xAligned, yAligned, diff);
        return false;
    }

    private bool InVisionRange(bool xAligned, bool yAligned, Vector2 diff)
    {
        return _facing switch
        {
            Facing.Up => xAligned && diff.Y < 0,
            Facing.Down => xAligned && diff.Y > 0,
            Facing.Left => yAligned && diff.X < 0,
            Facing.Right => yAligned && diff.X > 0,
            _ => throw new Exception("Error reading facing direction"),
        };
        
    }

    private void GoToPlayer(Player player, GameTime gametime)
    {
        // Stop moving if it is nonmoving trainer, or in close range
        if (Math.Abs(Vector2.Distance(player.Position, Position)) <= InteractionRange)
        {
            _visible = false;
            // BattleMain.LoadScene();
            player.StopEnd();
            return;
        }
        Vector2 norm = Vector2.Normalize(player.Position - Position);
        float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
        Position += norm * SpeedPxPerSec * dt;
    }

    private void Idle(GameTime gametime)
    {
        if (_moving)
        {
            // Might add random moving later
        }
    }

    public void Draw(SpriteBatch spriteBatch, float scale)
    {
        _sprite.Draw(spriteBatch, _texture, scale, Position);
    }

    // Temporary methods for Sprint 2, not cyclic
    public void NextSprite()
    {
        _sprite = new(++_spriteIndex > TrainerSpriteFactory.MaxIndexOfSprites ? --_spriteIndex : _spriteIndex);
    }

    public void PrevSprite()
    {
        _sprite = new(--_spriteIndex < 0 ? ++_spriteIndex : _spriteIndex);
    }

}
