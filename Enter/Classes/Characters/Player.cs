using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Enter.Classes.Cameras;
using Enter.Classes.Input;
using Enter.Classes.Sprites;
using Enter.Classes.Behavior;

namespace Enter.Classes.Characters;

public enum Facing { Down, Up, Left, Right }
public class Player
{
    //For collision
    public Tilemap Map { get; set; }
    public Team thePlayersTeam = new Team();
    public HashSet<Point> SolidTiles { get; set; }

    // Pixel-space render position (top-left of sprite)
    private Vector2 _pixelPosition;
    private static readonly Vector2 SpriteHalfSizeVector = 0.5f * new Vector2(PlayerSprite.SpriteSize);
    public Vector2 Position { get; private set; }
    private const float SpeedPxPerSec = 80f; // movement speed (pixels/sec)

    // Tile-space state
    public Point TilePos { get; private set; } // current tile position
    private TileMoveCommand _activeMoveCommand;
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

    public Player(Texture2D texture2, GameWindow Window, Team team)
    {
        _texture = texture2;
        _pixelPosition = Vector2.Zero;
        thePlayersTeam = team;
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
        _pixelPosition = TileToPixel(tilePos);
    }

    public Vector2 GetWorldCenterPosition() => GetSnappedPixelPosition() + SpriteHalfSizeVector;

    public Point GetTileFromWorldPosition(Vector2 worldPosition) => PixelToTile(worldPosition);

    public void Update(GameTime gameTime, KeyboardController keyboard, Camera Cam)
    {
        EnsureTileInitialized();

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        bool hasIntent = (!_seenByTrainer) && UpdateDirection(keyboard);

        if (_activeMoveCommand == null)
        {
            if (hasIntent)
                TryBeginStepFromFacing();

            // if we cannot enter next tile
            if (_activeMoveCommand == null)
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
        if (!CanEnter(next)) return;

        Vector2 targetPixel = TileToPixel(next);
        _activeMoveCommand = new TileMoveCommand(TilePos, next, _pixelPosition, targetPixel);
    }

    private void PerformStep(float dt, Camera Cam)
    {
        if (_activeMoveCommand == null) return;

        Vector2 newPos = _activeMoveCommand.Advance(_pixelPosition, SpeedPxPerSec, dt);
        Cam.DiffPos = newPos - _pixelPosition;
        _pixelPosition = newPos;
    }

    private void HandleArrivalAtTarget()
    {
        if (_activeMoveCommand == null || !_activeMoveCommand.HasArrived(_pixelPosition)) return;

        TilePos = _activeMoveCommand.TargetTile;
        _activeMoveCommand = null;
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
        TilePos = PixelToTile(_pixelPosition);
        _pixelPosition = TileToPixel(TilePos); // snap to grid
        _activeMoveCommand = null;
        _initializedTileFromPosition = true;
    }

    public void SetTilePosition(Point tile)
    {
        TilePos = tile;
        _pixelPosition = TileToPixel(tile);
        _initializedTileFromPosition = true;
        _activeMoveCommand = null;
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

    public Point PixelToTile(Vector2 pos)
    {
        int tileW = Map?.TileWidth ?? PlayerSprite.SpriteSize; // scale?
        int tileH = Map?.TileHeight ?? PlayerSprite.SpriteSize;
        return new Point((int)System.MathF.Floor(pos.X / tileW), (int)System.MathF.Floor(pos.Y / tileH));
    }

    public void Draw(SpriteBatch spriteBatch, float scale = 1f)
    {
        _sprite.Draw(spriteBatch, _texture, scale, GetSnappedPixelPosition());
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

    private Vector2 GetSnappedPixelPosition()
    {
        float snappedX = System.MathF.Round(_pixelPosition.X);
        float snappedY = System.MathF.Round(_pixelPosition.Y);
        return new Vector2(snappedX, snappedY);
    }

}
