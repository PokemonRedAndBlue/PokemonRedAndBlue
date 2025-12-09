using System;
using Microsoft.Xna.Framework.Content;
using Enter.Classes.Animations;
using Enter.Classes.Textures;

namespace Enter.Classes.Sprites;

public class PokemonFrontFactory
{
    private static PokemonFrontFactory _instance;
    private ContentManager _content;
    private TextureAtlas _PokemonFrontAtlas;
    private TextureAtlas _creatureAtlas;
    public TextureAtlas Atlas => _PokemonFrontAtlas;


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
        // Optional extra atlas for special cases (e.g., painter's creature)
        _creatureAtlas = TextureAtlas.FromFile(_content, "creature.xml");
    }
    public AnimatedSprite CreateAnimatedSprite(String spriteName)
    {
        string key = spriteName.ToLower();
        if (_PokemonFrontAtlas != null && _PokemonFrontAtlas._animations.ContainsKey(key))
        {
            return _PokemonFrontAtlas.CreateAnimatedSprite(key);
        }

        if (_creatureAtlas != null && _creatureAtlas._animations.ContainsKey(key))
        {
            return _creatureAtlas.CreateAnimatedSprite(key);
        }

        // Fallback: try default atlas to surface an exception with context
        return _PokemonFrontAtlas.CreateAnimatedSprite(spriteName);
    }
}
