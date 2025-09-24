using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Player
{

    public Vector2 Position { get; set; }

    public Player(GameWindow Window)
    { 
        Position = new Vector2(Window.ClientBounds.X, Window.ClientBounds.Y) * 0.5f;
    }

    public Player(Vector2 position)
    {
        Position = position;
    }

    public void Stop()
    {
        // TODO: Clear command state
    }
    
}
