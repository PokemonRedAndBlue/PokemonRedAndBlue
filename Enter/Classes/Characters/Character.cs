using System;
using Enter.Classes.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Character
{
    // Might use a scale for tile lengths later
    public Vector2 Position { get; private set; }
    public Point TilePos { get; private set; }

    private readonly Texture2D _texture;
    private Sprite _sprite;
    private Facing _facing = Facing.Down; // Facing direction
}
