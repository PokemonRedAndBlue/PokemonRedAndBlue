using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace MonoGameLibrary.Graphics;

public class PokemonBackFactory
{
    private static PokemonBackFactory _instance;
    private ContentManager _content;

    // Singleton Instance
    public static PokemonBackFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PokemonBackFactory();
            }
            return _instance;
        }
    }

    private PokemonBackFactory() { }

    public void LoadAllTextures(ContentManager content)
    {
        _content = content;

        // Example: Load your textures here
        // ExampleTexture = _content.Load<Texture2D>("Sprites/ExampleSprite");
    }

    // Example factory methods

    public Sprite CreateAnimatedSprite()
    {
        // return new AnimatedSprite(ExampleTexture);
        return null;
    }
}