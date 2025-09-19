using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Player(GameWindow Window)
{

    public Vector2 Position { get; set; } = new Vector2(Window.ClientBounds.X, Window.ClientBounds.Y) * 0.5f;
    
}
