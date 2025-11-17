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
    public Vector2 Position { get; private set; }
    public Tilemap Map { get; set; }

    private const float SpeedPxPerSec = 80f,
        InteractionRange = 64f, // might change based on scale?
        DefaultVisionRange = 256f,
        AlignMOE = 1f;  // Margin of Error for aligning checks, will be changed to tile based later
    private const int DefaultVisionRangeTiles = 4;
    private static readonly Vector2 SpriteHalfSizeVector = 0.5f * new Vector2(PlayerSprite.SpriteSize);
    private readonly int _visionRangeTiles = DefaultVisionRangeTiles;
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
        : this(texture, 0, Pos, facing, moving, DefaultVisionRangeTiles, trainerId) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, bool moving, string trainerId)
        : this(texture, spriteIndex, Pos, facing, moving, DefaultVisionRangeTiles, trainerId) { }
    public Trainer(Texture2D texture, Vector2 Pos, Facing facing, int visionRangeTiles, string trainerId)
        : this(texture, 0, Pos, facing, visionRangeTiles, trainerId) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, int visionRangeTiles, string trainerId)
        : this(texture, spriteIndex, Pos, facing, false, visionRangeTiles, trainerId) { }
    public Trainer(Texture2D texture, int spriteIndex, Vector2 Pos, Facing facing, bool moving, int visionRangeTiles, string trainerId)
    {
        _texture = texture;
        _spriteIndex = spriteIndex;
        _sprite = new(_spriteIndex);
        Position = Pos;
        _facing = facing;
        _moving = moving;
        _visionRangeTiles = visionRangeTiles;
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
            _sprite.IdleReset(_facing);
        }
    }

    private bool IsVisible(Player player)
    {
        if (!IsWithinVisionRange(player)) return false;

        Point playerTile = player.TilePos;
        return HasLineOfSight(GetTilePosition(), playerTile);
    }

    private bool HasLineOfSight(Point trainerTile, Point playerTile)
    {
        bool sameColumn = trainerTile.X == playerTile.X;
        bool sameRow = trainerTile.Y == playerTile.Y;

        if (!sameColumn && !sameRow) return false;

        return _facing switch
        {
            Facing.Up => sameColumn && playerTile.Y < trainerTile.Y,
            Facing.Down => sameColumn && playerTile.Y > trainerTile.Y,
            Facing.Left => sameRow && playerTile.X < trainerTile.X,
            Facing.Right => sameRow && playerTile.X > trainerTile.X,
            _ => throw new Exception("Error reading facing direction"),
        };
        
    }

    private bool IsWithinVisionRange(Player player)
    {
        Point trainerTile = GetTilePosition();
        Point playerTile = player.TilePos;
        int dx = System.Math.Abs(trainerTile.X - playerTile.X);
        int dy = System.Math.Abs(trainerTile.Y - playerTile.Y);
        int maxDelta = System.Math.Max(dx, dy);
        return maxDelta <= _visionRangeTiles;
    }

    private Vector2 GetWorldCenterPosition()
    {
        return Position + SpriteHalfSizeVector;
    }

    private bool GoToPlayer(Player player, GameTime gametime)
    {
        Vector2 playerCenter = player.GetWorldCenterPosition();
        Vector2 trainerCenter = GetWorldCenterPosition();
        Point trainerTile = GetTilePosition();
        Point playerTile = player.TilePos;
        // Stop moving if it is nonmoving trainer, or in close range
        if (IsWithinOneTile(trainerTile, playerTile))
        {
            _visible = false;
            player.StopEnd();
            return true; // Reached the player
        }
        Vector2 norm = Vector2.Normalize(playerCenter - trainerCenter);
        float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
        Position += norm * SpeedPxPerSec * dt;
        return false; // Still approaching
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

    private bool IsWithinOneTile(Point trainerTile, Point playerTile)
    {
        int dx = System.Math.Abs(trainerTile.X - playerTile.X);
        int dy = System.Math.Abs(trainerTile.Y - playerTile.Y);
        return dx + dy <= 1;
    }

    private Point GetTilePosition()
    {
        int tileW = Map?.TileWidth ?? PlayerSprite.SpriteSize;
        int tileH = Map?.TileHeight ?? PlayerSprite.SpriteSize;
        Vector2 center = GetWorldCenterPosition();
        return new Point(
            (int)System.MathF.Floor(center.X / tileW),
            (int)System.MathF.Floor(center.Y / tileH)
        );
    }
    
}
