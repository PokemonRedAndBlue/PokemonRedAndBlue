using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Enter.Classes.Cameras;
using Enter.Classes.Input;
using Enter.Classes.Sprites;

namespace Enter.Classes.Characters;

public enum Facing { Down, Up, Left, Right }
public class Player
{
    //For collision
    public Tilemap Map { get; set; }
    public HashSet<Point> SolidTiles { get; set; }

    // Pixel-space render position (top-left of sprite)
    public Vector2 Position { get; private set; }
    private const float SpeedPxPerSec = 80f; // movement speed (pixels/sec)

    // Tile-space state
    public Point TilePos { get; private set; } // current tile position
    private Point _targetTilePos;           // target tile when stepping
    private bool _isTileMoving = false;     // currently stepping between tiles
    private bool _initializedTileFromPosition = false;

    protected static readonly Dictionary<Facing, Vector2> _directions = new()
    {
        { Facing.Up,    new Vector2(0, -1) },
        { Facing.Down,  new Vector2(0, 1) },
        { Facing.Left,  new Vector2(-1, 0) },
        { Facing.Right, new Vector2(1, 0) },
    };
    private static readonly Dictionary<Direction, Facing> _facings = new()
    {
        { Direction.Up, Facing.Up },
        { Direction.Down, Facing.Down },
        { Direction.Left, Facing.Left },
        { Direction.Right, Facing.Right },
    };
    private readonly Texture2D _texture;
    private readonly PlayerSprite _sprite = new();
    private bool _seenByTrainer = false;
    private Facing _facing = Facing.Down;

    public Player(Texture2D texture2, GameWindow Window)
    {
        _texture = texture2;
    }
    public Player(Texture2D texture2, Vector2 position)
    {
        Position = position;
        _texture = texture2;
    }
    public Player(Texture2D texture2, Point tilePos)
    {
        TilePos = tilePos;
        _texture = texture2;
    }

    public void Update(GameTime gameTime, KeyboardController keyboard, Camera Cam)
    {
        EnsureTileInitialized();

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        bool hasIntent = (!_seenByTrainer) && UpdateDirection(keyboard);

        if (!_isTileMoving)
        {
            if (hasIntent)
                TryBeginStepFromFacing();

            // if we cannot enter next tile
            if (!_isTileMoving)
            {
                _sprite.IdleReset(_facing);
                return;
            }
        }

        PerformStep(dt, Cam);
        HandleArrivalAtTarget();
        _sprite.UpdateMovAnim(_facing, dt);
    }

    private void TryBeginStepFromFacing()
    {
        Point dir = FacingToPoint(_facing);
        Point next = new(TilePos.X + dir.X, TilePos.Y + dir.Y);
        if (CanEnter(next))
        {
            _targetTilePos = next;
            _isTileMoving = true;
        }
    }

    private void PerformStep(float dt, Camera Cam)
    {
        Vector2 targetPx = TileToPixel(_targetTilePos);
        Vector2 toTarget = targetPx - Position;

        Vector2 stepDir = Vector2.Zero;
        if (toTarget.X != 0) stepDir.X = System.MathF.Sign(toTarget.X);
        if (toTarget.Y != 0) stepDir.Y = System.MathF.Sign(toTarget.Y);

        Vector2 delta = stepDir * SpeedPxPerSec * dt;
        Vector2 newPos = Position + delta;

        // Clamp per-axis to avoid overshoot
        if ((stepDir.X > 0 && newPos.X > targetPx.X) || (stepDir.X < 0 && newPos.X < targetPx.X))
            newPos.X = targetPx.X;
        if ((stepDir.Y > 0 && newPos.Y > targetPx.Y) || (stepDir.Y < 0 && newPos.Y < targetPx.Y))
            newPos.Y = targetPx.Y;

        Cam.DiffPos = newPos - Position;
        Position = newPos;
    }

    private void HandleArrivalAtTarget()
    {
        Vector2 targetPx = TileToPixel(_targetTilePos);
        if (Position == targetPx)
        {
            TilePos = _targetTilePos;
            _isTileMoving = false;
        }
    }

    /// <summary>
    /// Update player's facing, return whether there is movement intent from input.
    /// </summary>
    private bool UpdateDirection(KeyboardController keyboard)
    {
        if (keyboard.MoveDirection == Direction.None) return false;
        _facing = _facings[keyboard.MoveDirection];
        return true;
    }

    private void EnsureTileInitialized()
    {
        if (_initializedTileFromPosition || Map == null) return;
        TilePos = PixelToTile(Position);
        Position = TileToPixel(TilePos); // snap to grid
        _targetTilePos = TilePos;
        _initializedTileFromPosition = true;
    }

    public void SetTilePosition(Point tile)
    {
        TilePos = tile;
        _targetTilePos = tile;
        if (Map != null)
            Position = TileToPixel(tile);
        _initializedTileFromPosition = true;
    }

    private static Point FacingToPoint(Facing facing)
    {
        return new Point((int)_directions[facing].X, (int)_directions[facing].Y);
    }

    private bool CanEnter(Point tile)
    {
        if (Map == null) return true;
        // bounds
        if (tile.X < 0 || tile.Y < 0 || tile.X >= Map.MapWidth || tile.Y >= Map.MapHeight)
            return false;
        // solids
        if (SolidTiles != null && SolidTiles.Contains(tile))
            return false;
        return true;
    }

    private Vector2 TileToPixel(Point tile)
    {
        int tileW = Map?.TileWidth ?? PlayerSprite.SpriteSize; // scale?
        int tileH = Map?.TileHeight ?? PlayerSprite.SpriteSize;
        return new Vector2(tile.X * tileW, tile.Y * tileH);
    }

    private Point PixelToTile(Vector2 pos)
    {
        int tileW = Map?.TileWidth ?? PlayerSprite.SpriteSize; // scale?
        int tileH = Map?.TileHeight ?? PlayerSprite.SpriteSize;
        return new Point((int)System.MathF.Floor(pos.X / tileW), (int)System.MathF.Floor(pos.Y / tileH));
    }

    public void Draw(SpriteBatch spriteBatch, float scale = 1f)
    {
        _sprite.Draw(spriteBatch, _texture, scale, Position);
    }

    /// <summary>
    /// Call this method when player enters the trainer's vision range. 
    /// </summary>
    public void Stop()
    {
        _seenByTrainer = true;
    }

    /// <summary>
    /// Call this method when any other stage ends or interrupts the trainer going to player process. 
    /// </summary>
    public void StopEnd()
    {
        _seenByTrainer = false;
    }

}
