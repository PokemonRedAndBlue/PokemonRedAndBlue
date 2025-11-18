using Microsoft.Xna.Framework;
using Enter.Classes.Characters;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Cameras;

public class Camera
{

    public Vector2 DiffPos { get; set; } = new(0, 0);
    public float Zoom { get; set; } = 1.0f;
    // The visible world rectangle covered by the current camera view
    public Rectangle VisibleWorldRect => new(
        (int)_viewOrigin.X,
        (int)_viewOrigin.Y,
        (int)(_viewport.Width/Zoom), //reflect zoom viewport
        (int)(_viewport.Height/Zoom) ////reflect zoom viewport
    );

    private readonly Viewport _viewport;
    private Vector2 _viewOrigin = new(0, 0);

    public Camera(Viewport viewport)
    {
        _viewport = viewport;
    }

    /// <summary>
    /// For general purpose of moving the camera
    /// </summary>
    public void Update()
    {
        _viewOrigin += DiffPos;
        DiffPos = Vector2.Zero;
    }
    
    /// <summary>
    /// Recenter on the center of player's sprite instead of the upper left corner.
    /// </summary>
    /// <param name="player">The player object</param>
    public void Update(Player player)
    {
        CenterOn(player.GetWorldCenterPosition());
    }

    /// <summary>
    /// Center on a given position
    /// </summary>
    /// <param name="Pos">the position to center on</param>
    public void CenterOn(Vector2 Pos)
    {
        _viewOrigin = Pos - 0.5f * new Vector2(_viewport.Width, _viewport.Height) / Zoom; //use viewport size expressed in WORLD units
    }

    // Matrix used by SpriteBatch to convert world -> screen, as camera offset
    public Matrix GetViewMatrix()
    {
        // moves the world opposite of the camera, zoom, and recenter after scaling
        return Matrix.CreateTranslation(new Vector3(-_viewOrigin, 0f))
        * Matrix.CreateScale(Zoom, Zoom, 1f); // Matrix used by SpriteBatch to convert world -> screen
    }

}
