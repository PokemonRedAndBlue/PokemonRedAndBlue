using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using System;
using System.Data;
using System.Collections.Generic;
using Enter.Classes.Animations;
using System.Threading;
using System.Runtime.Intrinsics;
using Enter.Classes.Characters;
using System.Net;

public class WildEncounterUI
{
    private Color pokemonBackgroundColor = new Color(246, 232, 248);
    private Sprite[] UIsprites;
    private TextureAtlas _WildUIAtlas;
    private TextSprite _wildPokemonMessage1;
    private TextSprite _wildPokemonMessage2;
    private SpriteFont _font;
    static private float _scale = 4.0f;
    private Sprite _trainerSpriteBack;
    private String _wildPokemonID;
    private AnimatedSprite _wildPokemonSpriteFront;
    private string _currentState = "Initial";

    // Pre defined regions within UI ADD TO A DICT LATER
    static private Vector2 uiBasePosition = new Vector2(350, 75);
    static private Vector2 pokemonHealthBarPosition = new Vector2(uiBasePosition.X + 150, uiBasePosition.Y + 200);
    static private Vector2 pokemonLevelPosition = new Vector2(uiBasePosition.X + 250, uiBasePosition.Y + 280);
    static private Vector2 arrowPosition = new Vector2(uiBasePosition.X + 50, uiBasePosition.Y + 400);
    static private Vector2 enemyPokemonPosition = new Vector2(uiBasePosition.X, uiBasePosition.Y);
    static private Vector2 playerPokemonPosition = new Vector2(uiBasePosition.X + 450, uiBasePosition.Y + 200);
    static private Vector2 playerTrainerPosition = new Vector2(uiBasePosition.X + (8 * _scale) - 5, uiBasePosition.Y + (40 * _scale) - 5);
    static private Vector2 wildPokemonPosition = new Vector2(uiBasePosition.X + (96 * _scale), uiBasePosition.Y);
    static private Vector2 _wildPokemonMessagePos1 = new Vector2(uiBasePosition.X + (8 * _scale), uiBasePosition.Y + (110 * _scale) + 1);
    static private Vector2 _wildPokemonMessagePos2 = new Vector2(uiBasePosition.X + (8 * _scale), uiBasePosition.Y + (125 * _scale) + 1);
    static private Player _Player;

    private Dictionary<string, int> stateMapping = new Dictionary<string, int>
    {
        { "Initial", 0 },
        { "Fight", 1 },
        { "Bag", 2 },
        { "Pokemon", 3 },
        { "Run", 4 }
    };

    public WildEncounterUI(TextureAtlas wildUIAtlas, ContentManager content, Player ourPlayer)
    {
        _WildUIAtlas = wildUIAtlas;
        _wildPokemonID = PokemonGenerator.GenerateRandom().Species.Name.ToLower(); // Example: "bulbasaur"
        _font = content.Load<SpriteFont>("PokemonFont");
        _Player = ourPlayer;
    }

    public void LoadContent(ContentManager content)
    {
        // Load UI Textures
        UIFactory.Instance.LoadAllTextures(content, "BattleInterface.xml");
        _WildUIAtlas = TextureAtlas.FromFile(content, "BattleInterface.xml");

        // load trainer sprite
        TextureAtlas trainerAtlas = TextureAtlas.FromFile(content, "BattleChars.xml");
        _trainerSpriteBack = new Sprite(trainerAtlas.GetRegion("player-back"));

        // load wild pokemon sprite
        PokemonFrontFactory.Instance.LoadAllTextures(content);
        _wildPokemonSpriteFront = PokemonFrontFactory.Instance.CreateAnimatedSprite(_wildPokemonID + "-front"); // Example: Bulbasaur

        // create UI elements
        UIsprites = new Sprite[_WildUIAtlas._regions.Count];
        int index = 0;
        foreach (var sprite in _WildUIAtlas._regions)
        {
            // Example: Create UI sprites as needed
            var uiSprite = _WildUIAtlas.CreateSprite(sprite.Key);
            UIsprites[index++] = uiSprite;
        }

        // _____ has appeared text sprite
        _wildPokemonMessage1 = new TextSprite("Wild    " + _wildPokemonID.ToUpper(), _font, Color.Black);
        _wildPokemonMessage2 = new TextSprite("appeared!", _font, Color.Black);
    }

    public void Draw(SpriteBatch spriteBatch, ContentManager content)
    {
        // Draw the base UI
        LoadContent(content);
        WildEncounterStateBasedDraw(UIsprites, spriteBatch);
        _wildPokemonMessage1.DrawTextSpriteWithScale(spriteBatch, _wildPokemonMessagePos1, 2f);
        _wildPokemonMessage2.DrawTextSpriteWithScale(spriteBatch, _wildPokemonMessagePos2, 2f);
    }

    public void WildEncounterStateBasedDraw(Sprite[] UI_BaseSprites, SpriteBatch spriteBatch)
    {

        // draw the UI elements for wild encounter (state based)
            switch (_currentState)
        {
            case "Initial": // Initial
                // draw base UI
                UIsprites[0].Draw(spriteBatch, Color.White, new Vector2(350, 75), 4f);
                // draw player trainer sprite
                _trainerSpriteBack.Draw(spriteBatch, Color.White, playerTrainerPosition, 8f);
                // draw wild pokemon sprite
                _wildPokemonSpriteFront.Draw(spriteBatch, Color.White, wildPokemonPosition, 4f);
                // draw player trainer party bar
                BattleUIHelper.drawPokeballSprites(_Player.thePlayersTeam, _WildUIAtlas, spriteBatch, true);
                break;
            case "Fight": // Fight
                UIsprites[1].Draw(spriteBatch, Color.White, new Vector2(350, 75), 4f);
                break;
            case "Bag": // Bag
                UIsprites[2].Draw(spriteBatch, Color.White, new Vector2(350, 75), 4f);
                break;
            case "Pokemon": // Pokemon
                UIsprites[3].Draw(spriteBatch, Color.White, new Vector2(350, 75), 4f);
                break;
            case "Run": // Run
                UIsprites[4].Draw(spriteBatch, Color.White, new Vector2(350, 75), 4f);
                break;
            default:
                UIsprites[0].Draw(spriteBatch, Color.White, new Vector2(350, 75), 4f);
                break;
        }
    }
}