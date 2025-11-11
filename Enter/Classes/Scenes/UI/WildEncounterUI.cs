using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using System;
using System.Data;
using System.Collections.Generic;

public class WildEncounterUI
{
    private Sprite[] UIsprites;
    private TextureAtlas _WildUIAtlas;
    private TextSprite trainerText;
    private SpriteFont _font;

    private Dictionary<string, int> stateMapping = new Dictionary<string, int>
    {
        { "Initial", 0 },
        { "Fight", 1 },
        { "Bag", 2 },
        { "Pokemon", 3 },
        { "Run", 4 }
    };

    public WildEncounterUI(TextureAtlas wildUIAtlas, ContentManager content)
    {
        _WildUIAtlas = wildUIAtlas;
        _font = content.Load<SpriteFont>("PokemonFont");

        // Load UI Textures
        UIFactory.Instance.LoadAllTextures(content, "BattleInterface.xml");
        _WildUIAtlas = TextureAtlas.FromFile(content, "BattleInterface.xml");

        // create UI elements
        UIsprites = new Sprite[_WildUIAtlas._regions.Count];
        int index = 0;
        foreach (var sprite in _WildUIAtlas._regions)
        {
            // Example: Create UI sprites as needed
            var uiSprite = _WildUIAtlas.CreateSprite(sprite.Key);
            UIsprites[index++] = uiSprite;
        }

        // Example text sprite
        trainerText = new TextSprite("A wild Pokémon appeared!", _font, Color.White);
    }

    public void Draw(SpriteBatch spriteBatch, string currentState)
    {
        // get index for current state
        int stateIndex = stateMapping.ContainsKey(currentState) ? stateMapping[currentState] : 0;

        // Draw UI elements
        trainerText.DrawTextSprite(spriteBatch, new Vector2(100, 100));
        UIsprites[stateIndex].Draw(spriteBatch, Color.White, new Vector2(350, 75), 4f); // Example UI sprite
    }
}