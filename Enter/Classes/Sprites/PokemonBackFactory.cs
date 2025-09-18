using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace MonoGameLibrary.Graphics;

public class PokemonBackFactory
{
    private static PokemonBackFactory _instance;
    private ContentManager _content;
    private TextureAtlas _PokemonBackAtlas;

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

    public void LoadAllTextures(ContentManager Content)
    {
        _content = Content;

        // Example: Load your textures here
        _PokemonBackAtlas = TextureAtlas.FromFile(_content, "Pokemon_BACK.xml");
    }
    public Sprite CreateStaticSprite(String spriteName)
    {
        return _PokemonBackAtlas.CreateSprite(spriteName);
    }
}