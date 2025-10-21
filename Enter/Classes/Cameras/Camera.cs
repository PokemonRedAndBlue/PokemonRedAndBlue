using Microsoft.Xna.Framework;
using Enter.Classes.Characters;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Cameras;

public class Camera
{

    public Vector2 DiffPos
    {
        get => _difPos;
        set => _difPos = value;
    }
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

    public void CenterOn(Vector2 Pos)
    {
        Vector2 viewport = new(_window.ClientBounds.X, _window.ClientBounds.Y);
        _viewOrigin += Pos - 0.5f * viewport;
    }
    
}
