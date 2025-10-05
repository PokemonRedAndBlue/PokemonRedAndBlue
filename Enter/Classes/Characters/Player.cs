using Enter.Classes.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public enum Facing { Down, Up, Left, Right }
public class Player
{

    public Vector2 Position { get; set; } = Vector2.Zero;
    private const float SpeedPxPerSec = 80f; // movement speed
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

    public void Update(GameTime gameTime, int axisX, int axisY)
    {
        // Determine if moving, and which way we face
        bool isMoving = (!_seenByTrainer) && (axisX != 0 || axisY != 0);

        // If both non-zero ever slip through, pick a priority (down/up over left/right)
        if (axisX != 0 && axisY != 0)
        {
            // Choose one; here we prefer vertical
            axisX = 0;
        }
        UpdateFacing(axisX, axisY);

        if (isMoving)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Movement
            // Normalize axis (already unit in each axis), move in pixels/sec
            Position += new Vector2(axisX, axisY) * SpeedPxPerSec * dt;

            // Animation
            _sprite.UpdateMovAnim(_facing, dt);
        }
        else
        {
            // Idle frame for the facing direction
            _sprite.IdleReset(_facing);
        }
    }

    private void UpdateFacing(int axisX, int axisY)
    {
        if (axisY < 0) _facing = Facing.Up;
        else if (axisY > 0) _facing = Facing.Down;
        else if (axisX < 0) _facing = Facing.Left;
        else if (axisX > 0) _facing = Facing.Right;
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
