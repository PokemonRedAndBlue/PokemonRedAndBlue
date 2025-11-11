using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;

public class WildEncounterUI
{
    private Sprite[] sprites;
    private TextureAtlas _WildUIAtlas;
    private TextSprite trainerText;
    private SpriteFont _font;

    public WildEncounterUI(TextureAtlas wildUIAtlas, ContentManager content)
    {
        _WildUIAtlas = wildUIAtlas;
        _font = content.Load<SpriteFont>("PokemonFont");

        // create UI elements
        sprites = new Sprite[_WildUIAtlas._regions.Count];
        int index = 0;
        foreach (var region in _WildUIAtlas._regions.Values)
        {
            sprites[index++] = _WildUIAtlas.CreateSprite(region.Name);
        }

        // Example text sprite
        trainerText = new TextSprite("A wild Pokémon appeared!", PokemonFont, Color.W);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw UI elements
        trainerText.DrawTextSprite(spriteBatch, new Vector2(100, 100));
        sprites[0].Draw(spriteBatch, Color.White, new Vector2(350, 75), 4f); // Example UI sprite
    }
}