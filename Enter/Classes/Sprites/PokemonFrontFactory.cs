using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace MonoGameLibrary.Graphics;

public class PokemonFrontFactory
{
    private static PokemonFrontFactory _instance;
    private ContentManager _content;
    private TextureAtlas _PokemonFrontAtlas;

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

    public void LoadAllTextures(ContentManager Content)
    {
        _content = Content;

        // Example: Load your textures here
        _PokemonFrontAtlas = TextureAtlas.FromFile(_content, "Pokemon_FRONT.xml");
    }
    public Sprite CreateAnimatedSprite(String spriteName)
    {
        return _PokemonFrontAtlas.CreateAnimatedSprite(spriteName);
    }
}