using System;
using Microsoft.Xna.Framework.Content;
using Enter.Classes.Textures;

namespace Enter.Classes.Sprites;

public class UIFactory
{
    private static UIFactory _instance;
    private ContentManager _content;
    private TextureAtlas _UIAtlas;

    // Singleton Instance
    public static UIFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIFactory();
            }
            return _instance;
        }
    }

    private UIFactory() { }

    public void LoadAllTextures(ContentManager Content, String path)
    {
        _content = Content;

        // Example: Load your textures here
        _UIAtlas = TextureAtlas.FromFile(_content, path);
    }
    public Sprite CreateStaticSprite(String spriteName)
    {
        return _UIAtlas.CreateSprite(spriteName);
    }
}