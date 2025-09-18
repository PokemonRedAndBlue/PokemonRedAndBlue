using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace MonoGameLibrary.Graphics;

public class PokemonFrontFactory
{
    private static PokemonFrontFactory _instance;
    private ContentManager _content;

    // Singleton Instance
    public static PokemonFrontFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PokemonFrontFactory();
            }
            return _instance;
        }
    }

    private PokemonFrontFactory() { }

    public void LoadAllTextures(ContentManager content)
    {
        _content = content;

        // Example: Load your textures here
        // ExampleTexture = _content.Load<Texture2D>("Sprites/ExampleSprite");
    }

    // Example factory methods

    public Sprite CreateStaticSprite()
    {
        // return new StaticSprite(ExampleTexture);
        return null;
    }
}