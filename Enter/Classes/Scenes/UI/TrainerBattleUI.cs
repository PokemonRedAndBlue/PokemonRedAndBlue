using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using System;
using System.Data;
using System.Collections.Generic;
using Enter.Classes.Animations;
using PokemonGame;

public class TrainerBattleUI
{
    private Color pokemonBackgroundColor = new Color(246, 232, 248);
    private Sprite[] UIBaseSprites;
    private TextureAtlas _TrainerUIAtlas;
    private TextSprite _trainerText;
    private SpriteFont _font;
    private BattleUIHelper battleUI = new BattleUIHelper();
    private String _enemyTrainerString;
    static private float _scale = 4.0f;
    private Sprite _trainerSpriteBack;
    private AnimatedSprite _enemyPokemonSpriteFront;
    private Sprite _enemyTrainerSpriteFront;
    private TextSprite _enemyTrainerIDSprite;
    private Sprite greenBar, yellowBar, redBar;
    private string _currentState = "Initial";

    // Pre defined regions within UI ADD TO A DICT LATER
    static private Vector2 uiBasePosition = new Vector2(340, 75);
    static private Vector2 pokemonHealthBarPosition = new Vector2(uiBasePosition.X + 150, uiBasePosition.Y + 200);
    static private Vector2 pokemonLevelPosition = new Vector2(uiBasePosition.X + 250, uiBasePosition.Y + 280);
    static private Vector2 arrowPosition = new Vector2(uiBasePosition.X + 50, uiBasePosition.Y + 400);
    static private Vector2 playerPosition = new Vector2(uiBasePosition.X + (8 * _scale) - 5, uiBasePosition.Y + (40 * _scale) - 5);
    static private Vector2 enemyPokemonPosition = new Vector2(uiBasePosition.X, uiBasePosition.Y);
    static private Vector2 playerPokemonPosition = new Vector2(uiBasePosition.X + 450, uiBasePosition.Y + 200);
    static private Vector2 playerTrainerPosition = new Vector2(uiBasePosition.X + (8 * _scale) - 5, uiBasePosition.Y + (40 * _scale) - 5);
    static private Vector2 enemyTrainerPosition = new Vector2(uiBasePosition.X + (96 * _scale) - 4, uiBasePosition.Y);
    static private Vector2 enemyTrainerIDPosition = new Vector2(uiBasePosition.X + (8 * _scale), uiBasePosition.Y + (110 * _scale) + 1);
    static private Vector2 _borderPostion = new Vector2(uiBasePosition.X - (48 * _scale), uiBasePosition.Y - (40 * _scale) + 1);
    static private Vector2 enemysPokemonPosition = new Vector2(uiBasePosition.X + (96 * _scale), uiBasePosition.Y);
    static private Vector2 maxDrawPos = new Vector2(0, uiBasePosition.Y + (103 * _scale));
    public Boolean resetBattle = false;
    public Boolean didRunOrCatch = false;
    static private Team _playerTeam;
    static private Team _enemyTeam;
    static private Sprite _border;
    private Dictionary<string, int> stateMapping = new Dictionary<string, int>
    {
        { "Initial", 0 },
        { "Fight", 1 },
        { "Bag", 2 },
        { "Pokemon", 3 },
        { "Run", 4 }
    };

    public TrainerBattleUI(TextureAtlas trainerUIAtlas, ContentManager content, String enemyTrainerID, Team playerTeam, Team enemyTeam)
    {
        // init class vars
        _TrainerUIAtlas = trainerUIAtlas;
        _enemyTrainerString = enemyTrainerID.ToLower();
        _font = content.Load<SpriteFont>("PokemonFont");
        _playerTeam = playerTeam;
        _enemyTeam = enemyTeam;
    }

    public void Update(GameTime gameTime)
    {
        battleUI.Update(gameTime);
        _currentState = battleUI.getBattleState();
    }

    public void LoadContent(ContentManager content)
    {
        // Load UI Textures
        UIFactory.Instance.LoadAllTextures(content, "BattleInterface.xml");
        _TrainerUIAtlas = TextureAtlas.FromFile(content, "BattleInterface.xml");
        TextureAtlas borders = TextureAtlas.FromFile(content, "Borders.xml");
        _border = new Sprite(borders.GetRegion("blue-border"));

        // load trainer sprite
        TextureAtlas trainerAtlas = TextureAtlas.FromFile(content, "BattleChars.xml");
        _trainerSpriteBack = new Sprite(trainerAtlas.GetRegion("player-back"));

        // load enemy trainer sprite
        _enemyTrainerSpriteFront = new Sprite(trainerAtlas.GetRegion(_enemyTrainerString.ToLower()));

        // Initialize health bar sprites
        greenBar = new Sprite(_TrainerUIAtlas.GetRegion("green-health"));
        yellowBar = new Sprite(_TrainerUIAtlas.GetRegion("yellow-health"));
        redBar = new Sprite(_TrainerUIAtlas.GetRegion("red-health"));

        // create UI elements
        UIBaseSprites = new Sprite[_TrainerUIAtlas._regions.Count];
        int index = 0;
        foreach (var sprite in _TrainerUIAtlas._regions)
        {
            // Example: Create UI sprites as needed
            var uiSprite = _TrainerUIAtlas.CreateSprite(sprite.Key);
            UIBaseSprites[index++] = uiSprite;
        }

        // Example text sprite
        _enemyTrainerIDSprite = new TextSprite(formatTrainerName(_enemyTrainerString).ToUpper(), _font, Color.Black);
        _trainerText = new TextSprite("A wild Pokémon appeared!", _font, Color.White);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the base UI elements based on current state
        WildEncounterStateBasedDraw(UIBaseSprites, spriteBatch);
    }

    public void WildEncounterStateBasedDraw(Sprite[] UI_BaseSprites, SpriteBatch spriteBatch)
    {
        // always draw border
        _border.Draw(spriteBatch, Color.White, _borderPostion, 4f);

        // always get players pokemon
        Pokemon currentPokemon = _playerTeam.Pokemons[0];
        Pokemon enemyPokemon = _enemyTeam.Pokemons[0];
        _enemyPokemonSpriteFront = PokemonFrontFactory.Instance.CreateAnimatedSprite(enemyPokemon.Name.ToString().ToLower() + "-front");

        // draw the UI elements for wild encounter (state based)
        switch (_currentState)
        {
            case "Initial": // Initial
                // draw base UI
                UIBaseSprites[0].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                // draw player trainer sprite
                _trainerSpriteBack.Draw(spriteBatch, Color.White, playerTrainerPosition, 8f);
                // draw enemy trainer sprite
                _enemyTrainerSpriteFront.Draw(spriteBatch, Color.White, enemyTrainerPosition, 4f);
                // draw enemy trainer name
                _enemyTrainerIDSprite.DrawTextSpriteWithScale(spriteBatch, enemyTrainerIDPosition, 2f);
                // draw player trainer party bar
                BattleUIHelper.drawPokeballSprites(_playerTeam, _TrainerUIAtlas, spriteBatch, true);
                // draw enemy trainer party bar
                BattleUIHelper.drawPokeballSprites(_enemyTeam, _TrainerUIAtlas, spriteBatch, false);
                break;
            case "Menu": 
                // draw base UI
                UIBaseSprites[1].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                // draw players pokemon sprite, get first alive pokemon
                String playersPokemon = currentPokemon.Name.ToString();
                Sprite currentMon = PokemonBackFactory.Instance.CreateStaticSprite( playersPokemon.ToLower()+ "-back");
                currentMon.Draw(spriteBatch, Color.White, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)), 4f);
                // draw opponent pokemon sprite
                _enemyPokemonSpriteFront.Draw(spriteBatch, Color.White, enemysPokemonPosition, 4f);
                // draw both health
                battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, true);
                battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, false);
                // arrow handling logic
                battleUI.DrawArrow(_TrainerUIAtlas, spriteBatch);
                battleUI.moveArrow();
                break;
            case "Fight": // Fight
                UIBaseSprites[2].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                break;
            case "Item": // Bag
                UIBaseSprites[4].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                break;
            case "PkMn": // Pokemon
                UIBaseSprites[3].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                break;
            case "Run": // Run
                didRunOrCatch = true;
                break;
            default:
                UIBaseSprites[0].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                break;
        }
    }
    
    public string formatTrainerName(string trainerID)
    {
        // add .'s for any remaing characters available up to 11
        while (trainerID.Length < 14)
        {
            trainerID += " .";
        }
        return trainerID;
    }
}