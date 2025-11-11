using System;
using Microsoft.Xna.Framework.Content;
using Enter.Classes.Textures;

namespace Enter.Classes.Sprites;

public class UIFactory
{
    private static UIFactory _instance;
    private ContentManager _content;
    private TextureAtlas _UIAtlas;
    private String path = "";

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
        this.path = path;

        // Example: Load your textures here
        _UIAtlas = TextureAtlas.FromFile(_content, "Content/" + path);
    }
    public Sprite CreateStaticSprite(String spriteName)
    {
        return _UIAtlas.CreateSprite(spriteName);
    }

    public TextureAtlas FromFile(ContentManager content)
    {
        return TextureAtlas.FromFile(content, "Content/" + this.path);
    }
}