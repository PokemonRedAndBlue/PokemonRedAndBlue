using Microsoft.Xna.Framework;
using Enter.Classes.Characters;

namespace Enter.Classes.Cameras;

public class Camera
{

    public Vector2 DiffPos
    {
        get => _difPos;
        set => _difPos = value;
    }
    // The visible world rectangle covered by the current camera view
    public Rectangle VisibleWorldRect => new(
        (int)_viewOrigin.X,
        (int)_viewOrigin.Y,
        _window.ClientBounds.Width,
        _window.ClientBounds.Height
    );
    private readonly GameWindow _window;
    private Vector2 _difPos = new(0, 0);
    private Vector2 _viewOrigin = new(0, 0);

    public Camera(GameWindow Window)
    {
        _window = Window;
    }

    public void Update()
    {
        _viewOrigin += DiffPos;
        DiffPos = Vector2.Zero;
    }
    
    // if called with a player, just recenter
    public void Update(Player player)
    {
        CenterOn(player.Position);
    }

    public void CenterOn(Vector2 Pos)
    {
        Vector2 viewport = new(_window.ClientBounds.Width, _window.ClientBounds.Height);
        _viewOrigin = Pos - 0.5f * viewport;
    }

    // Matrix used by SpriteBatch to convert world -> screen, as camera offset
    public Matrix GetViewMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-_viewOrigin, 0f));
    }

}
