using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Sprites;

public class TextSprite : Sprite
{
    private string _text;
    private SpriteFont _font;
    private Color _color;
    private Vector2 _textSize;

    public TextSprite(string text, SpriteFont font, Color color)
    {
        _text = text;
        _font = font;
        _color = color;
        _textSize = _font.MeasureString(_text);
    }

    public void DrawTextSprite(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.DrawString(_font, _text, position, _color);
    }
}
