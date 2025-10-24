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

    public Vector2 Position { get; set; } = Vector2.Zero;
    private const float SpeedPxPerSec = 120f; // movement speed
    protected static readonly Dictionary<Facing, Vector2> _directions = new()
    {
        { Facing.Up,    new Vector2(0, -1) },
        { Facing.Down,  new Vector2(0, 1) },
        { Facing.Left,  new Vector2(-1, 0) },
        { Facing.Right, new Vector2(1, 0) },
    };
    private readonly Texture2D _texture;
    private readonly PlayerSprite _sprite = new();
    private bool _seenByTrainer = false;
    private Facing _facing = Facing.Down;

    public Player(Texture2D texture2, GameWindow Window)
    {
        Position = new Vector2(Window.ClientBounds.X, Window.ClientBounds.Y) * 0.5f;
        _texture = texture2;
    }
    public Player(Texture2D texture2, Vector2 position)
    {
        Position = position;
        _texture = texture2;
    }

    public void Update(GameTime gameTime, KeyboardController keyboard, Camera Cam)
    {
        // Determine if moving, and which way we face
        bool isMoving = (!_seenByTrainer) && UpdateDirection(keyboard);

        if (isMoving)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdatePosition(dt, Cam);
            _sprite.UpdateMovAnim(_facing, dt);
        }
        else
        {
            // Idle frame for the facing direction
            _sprite.IdleReset(_facing);
        }
    }

    /// <summary>
    /// Update player's facing, return whether player is moving. 
    /// </summary>
    /// <param name="keyboard"></param>
    /// <returns></returns>
    private bool UpdateDirection(KeyboardController keyboard)
    {
        switch (keyboard.MoveDirection)
        {
            case Direction.Left:
                _facing = Facing.Left;
                break;
            case Direction.Right:
                _facing = Facing.Right;
                break;
            case Direction.Up:
                _facing = Facing.Up;
                break;
            case Direction.Down:
                _facing = Facing.Down;
                break;
            default:
                return false;
        }
        return true;
    }

    private void UpdatePosition(float dt, Camera Cam)
    {
        // Normalize axis (already unit in each axis), move in pixels/sec
        Vector2 diffPos = _directions[_facing] * SpeedPxPerSec * dt;
        Cam.DiffPos = diffPos;
        Position += diffPos;
    }

    public void Draw(SpriteBatch spriteBatch, float scale)
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
