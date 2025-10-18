using Microsoft.Xna.Framework;
using Enter.Classes.Characters;

namespace Enter.Classes.Camera;

public class Camera
{

    public Vector2 Position { get => _position; }
    private readonly GameWindow _window;
    private Vector2 _position = new(0, 0);

    public Camera(GameWindow Window)
    {
        _window = Window;
    }

    public void Update(Player player)
    {
        _position = player.Position - 0.5f * new Vector2(_window.ClientBounds.Width, _window.ClientBounds.Height);
    }
    
}
