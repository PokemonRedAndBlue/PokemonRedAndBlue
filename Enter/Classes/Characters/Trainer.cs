using System;
using Enter.Classes.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using TrainerMethods;

namespace Enter.Classes.Characters;

public class Trainer
{
    // Trainer Team init
    private Team _trainersTeam;

    // Unique identifier for this trainer instance
    public string TrainerID { get; private set; }

    // Might use a scale for tile lengths later
    public Vector2 Position { get; set; }

    private const float SpeedPxPerSec = 80f,
        InteractionRange = 64f, // might change based on scale?
        DefaultVisionRange = 256f,
        AlignMOE = 1f;  // Margin of Error for aligning checks, will be changed to tile based later
    private readonly float _visionRange = DefaultVisionRange;
    private readonly bool _moving = false;  // Whether the trainer will hang around when idling
    private readonly Texture2D _texture;
    private TrainerSprite _sprite;
    private int _spriteIndex;
    private Facing _facing = Facing.Down; // Facing direction
    private bool _visible = false;  // Whether the trainer sees a player now
    public bool colided = false;  // Whether the trainer has collided with the player
    public bool HasBeenDefeated { get; set; } = false;  // Whether this trainer has been defeated in battle
    public bool IsApproachingPlayer { get; private set; } = false;  // Whether trainer is currently walking to player

    public Trainer(Texture2D texture, Vector2 Pos, Facing facing, string trainerId) 
        : this(texture, Pos, facing, false, trainerId) { }
    public Trainer(Texture2D texture, Vector2 Pos, Facing facing, bool moving, string trainerId) 
        : this(texture, 0, Pos, facing, moving, trainerId) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, bool moving, string trainerId) 
        : this(texture, spriteIndex, Pos, facing, moving, DefaultVisionRange, trainerId) { }
    public Trainer(Texture2D texture, Vector2 Pos, Facing facing, float visionRange, string trainerId) 
        : this(texture, 0, Pos, facing, visionRange, trainerId) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, float visionRange, string trainerId) 
        : this(texture, spriteIndex, Pos, facing, false, visionRange, trainerId) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, bool moving, float visionRange, string trainerId)
    {
        _texture = texture;
        _spriteIndex = spriteIndex;
        _sprite = new(_spriteIndex);
        Position = Pos;
        _facing = facing;
        _moving = moving;
        _visionRange = visionRange;
        TrainerID = trainerId;
        colided = false;
        _trainersTeam = new Team();
    }

    public void Update(GameTime gametime, Player player)    // TODO: Sprites
    {
        // If trainer has been defeated, they can't trigger battles but can still interact
        if (HasBeenDefeated)
        {
            // Allow interaction when player is close but don't chase or battle
            if (Math.Abs(Vector2.Distance(player.Position, Position)) <= InteractionRange)
            {
                colided = true; // Allow dialogue but won't trigger battle since HasBeenDefeated=true
            }
            _sprite.IdleReset(_facing);
            return;
        }

        // First-time spotting logic for undefeated trainers
        if (!_visible && IsVisible(player))
        {
            _visible = true;
            IsApproachingPlayer = true;
        }

        if (_visible)
        {
            player.Stop();
            bool hasReachedPlayer = GoToPlayer(player, gametime);
            if (hasReachedPlayer)
            {
                colided = true;
                IsApproachingPlayer = false;
            }
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

    private bool GoToPlayer(Player player, GameTime gametime)
    {
        // Stop moving if it is nonmoving trainer, or in close range
        if (Math.Abs(Vector2.Distance(player.Position, Position)) <= InteractionRange)
        {
            _visible = false;
            player.StopEnd();
            return true; // Reached the player
        }
        Vector2 norm = Vector2.Normalize(player.Position - Position);
        float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
        Position += norm * SpeedPxPerSec * dt;
        return false; // Still approaching
    }

    private void Idle(GameTime gametime)
    {
        if (_moving)
        {
            // Might add random moving later
        }
    }

    public void Draw(SpriteBatch spriteBatch, float scale = 1f)
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
