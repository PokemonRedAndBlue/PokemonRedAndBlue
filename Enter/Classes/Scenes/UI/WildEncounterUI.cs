using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using System;
using System.Data;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.GameState;
using System.Threading;
using System.Runtime.Intrinsics;
using Enter.Classes.Characters;
using System.Net;
using Enter.Classes.Scenes;
using PokemonGame;
using Enter.Classes.Input;
using System.Globalization;
// duplicate using removed
using System.Collections;
using System.Runtime.ConstrainedExecution;

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
    private Sprite wildUIAppeared;
    private String _wildPokemonID;
    private AnimatedSprite _wildPokemonSpriteFront;
    // Animation attack state tracking
    private bool shouldPlayPlayerAttackAnimation = false;
    private bool shouldPlayEnemyAttackAnimation = false;
    private bool playerAttackAnimationPlaying = false;
    private bool enemyAttackAnimationPlaying = false;
    private double playerAttackAnimationTimer = 0.0;
    private double enemyAttackAnimationTimer = 0.0;
    private const double AttackAnimationDurationMs = 600.0;
    private FrontStateAction frontState = new FrontStateAction();
    private BackStateAction backState = new BackStateAction();

    // Pre defined regions within UI ADD TO A DICT LATER
    static private Vector2 uiBasePosition = new Vector2(340, 75);
    static private Vector2 pokemonHealthBarPosition = new Vector2(uiBasePosition.X + 150, uiBasePosition.Y + 200);
    static private Vector2 pokemonLevelPosition = new Vector2(uiBasePosition.X + 250, uiBasePosition.Y + 280);
    static private Vector2 arrowPosition = new Vector2(uiBasePosition.X + 50, uiBasePosition.Y + 400);
    static private Vector2 enemyPokemonPosition = new Vector2(uiBasePosition.X, uiBasePosition.Y);
    static private Vector2 playerPosition = new Vector2(uiBasePosition.X + (8 * _scale) - 5, uiBasePosition.Y + (40 * _scale) - 5);
    static private Vector2 wildPokemonPosition = new Vector2(uiBasePosition.X + (96 * _scale), uiBasePosition.Y + 20);
    static private Vector2 _wildPokemonMessagePos1 = new Vector2(uiBasePosition.X + (8 * _scale), uiBasePosition.Y + (110 * _scale) + 1);
    static private Vector2 _wildPokemonMessagePos2 = new Vector2(uiBasePosition.X + (8 * _scale), uiBasePosition.Y + (125 * _scale) + 1);
    static private Vector2 _borderPostion = new Vector2(uiBasePosition.X - (48 * _scale), uiBasePosition.Y - (40 * _scale) + 1);
    static private Vector2 maxDrawPos = new Vector2(0, uiBasePosition.Y + (103 * _scale));
    public BattleUIHelper battleUI = new BattleUIHelper();
    private Sprite greenBar, yellowBar, redBar;
    static private Player _Player;
    static private Sprite _border;
    public Boolean resetBattle = false;
    public Boolean didRunOrCatch = false;

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
        _wildPokemonID = PokemonGenerator.GenerateWildPokemon().Species.Name.ToLower(); // Example: "bulbasaur"
        _font = content.Load<SpriteFont>("PokemonFont");
        _Player = ourPlayer;
    }

    public void Update(GameTime gameTime)
    {
        // Update UI state machine
        battleUI.Update(gameTime);

        // advance animated sprite frames only while attacking (prevents constant looping)
        if (enemyAttackAnimationPlaying)
        {
            _wildPokemonSpriteFront?.Update(gameTime);
        }

        // Input: pressing A triggers both player's back attack motion and wild front attack motion (demo)
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            shouldPlayEnemyAttackAnimation = true;
            enemyAttackAnimationPlaying = false; // will start in Update/Draw
            shouldPlayPlayerAttackAnimation = true;
            playerAttackAnimationPlaying = false;
        }

        // Start animations when flagged
        if (shouldPlayEnemyAttackAnimation && !enemyAttackAnimationPlaying)
        {
            enemyAttackAnimationPlaying = true;
            enemyAttackAnimationTimer = 0.0;
            // ensure front animation is active
            TrySetAnimation(_wildPokemonSpriteFront, new string[] { _wildPokemonID + "-front" });
        }

        if (shouldPlayPlayerAttackAnimation && !playerAttackAnimationPlaying)
        {
            playerAttackAnimationPlaying = true;
            playerAttackAnimationTimer = 0.0;
        }

        // Update timers for playing animations
        if (enemyAttackAnimationPlaying)
        {
            enemyAttackAnimationTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (enemyAttackAnimationTimer >= AttackAnimationDurationMs)
            {
                enemyAttackAnimationPlaying = false;
                shouldPlayEnemyAttackAnimation = false;
                enemyAttackAnimationTimer = 0.0;
                // optionally reset sprite animation to idle by setting front animation again
                TrySetAnimation(_wildPokemonSpriteFront, new string[] { _wildPokemonID + "-front" });
            }
        }

        if (playerAttackAnimationPlaying)
        {
            playerAttackAnimationTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (playerAttackAnimationTimer >= AttackAnimationDurationMs)
            {
                playerAttackAnimationPlaying = false;
                shouldPlayPlayerAttackAnimation = false;
                playerAttackAnimationTimer = 0.0;
            }
        }
    }

    public void LoadContent(ContentManager content)
    {
        // Load UI Textures
        UIFactory.Instance.LoadAllTextures(content, "BattleInterface.xml");
        _WildUIAtlas = TextureAtlas.FromFile(content, "BattleInterface.xml");
        TextureAtlas borders = TextureAtlas.FromFile(content, "Borders.xml");
        _border = new Sprite(borders.GetRegion("blue-border"));


        // load trainer sprite
        TextureAtlas trainerAtlas = TextureAtlas.FromFile(content, "BattleChars.xml");
        _trainerSpriteBack = new Sprite(trainerAtlas.GetRegion("player-back"));

        // load wild pokemon sprite
        PokemonFrontFactory.Instance.LoadAllTextures(content);
        PokemonBackFactory.Instance.LoadAllTextures(content);
        _wildPokemonSpriteFront = PokemonFrontFactory.Instance.CreateAnimatedSprite(_wildPokemonID + "-front"); // Example: Bulbasaur

        // create UI elements
        greenBar = new Sprite(_WildUIAtlas.GetRegion("green-health"));
        yellowBar = new Sprite(_WildUIAtlas.GetRegion("yellow-health"));
        redBar = new Sprite(_WildUIAtlas.GetRegion("red-health"));

        // Populate UIsprites array in order matching stateMapping: Initial=0, Menu=1, Fight=2, Bag=4, Pokemon=3
        UIsprites = new Sprite[5];
        UIsprites[0] = _WildUIAtlas.CreateSprite("battle-wild");   // Initial
        UIsprites[1] = _WildUIAtlas.CreateSprite("battle-menu");     // Menu
        UIsprites[2] = _WildUIAtlas.CreateSprite("battle-attack");   // Fight
        UIsprites[3] = _WildUIAtlas.CreateSprite("battle-pokemon");  // Pokemon
        UIsprites[4] = _WildUIAtlas.CreateSprite("battle-item");     // Bag

        // wildUIAppeared is same as Initial state UI sprite
        wildUIAppeared = UIsprites[0];

        // _____ has appeared text sprite
        _wildPokemonMessage1 = new TextSprite("Wild    " + _wildPokemonID.ToUpper(), _font, Color.Black);
        _wildPokemonMessage2 = new TextSprite("appeared!", _font, Color.Black);
    }

    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the base UI
        WildEncounterStateBasedDraw(UIsprites, spriteBatch);
    }

    public void WildEncounterStateBasedDraw(Sprite[] UI_BaseSprites, SpriteBatch spriteBatch)
    {
        // always draw border
        _border.Draw(spriteBatch, Color.White, _borderPostion, 4f);

        // always get players pokemon
         Pokemon currentPokemon = _Player.thePlayersTeam.Pokemons[0];

        // draw the UI elements for wild encounter (state based)
            switch (battleUI.getBattleState())
        {
            case "Initial": // Initial
                // draw base UI
                wildUIAppeared.Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                // draw _____ appeared messages
                _wildPokemonMessage1.DrawTextSpriteWithScale(spriteBatch, _wildPokemonMessagePos1, 2f);
                _wildPokemonMessage2.DrawTextSpriteWithScale(spriteBatch, _wildPokemonMessagePos2, 2f);
                // draw player trainer sprite
                _trainerSpriteBack.Draw(spriteBatch, Color.White, playerPosition, 8f);
                // draw wild pokemon sprite (apply attack offset if active)
                Vector2 wildOffsetInit = Vector2.Zero;
                if (enemyAttackAnimationPlaying)
                {
                    wildOffsetInit = frontState.AttackFrontAction(_wildPokemonSpriteFront, enemyAttackAnimationTimer, AttackAnimationDurationMs);
                }
                _wildPokemonSpriteFront.Draw(spriteBatch, Color.White, wildPokemonPosition + wildOffsetInit, 4f);
                // draw player trainer party bar
                BattleUIHelper.drawPokeballSprites(_Player.thePlayersTeam, _WildUIAtlas, spriteBatch, true);
                break;
            case "Menu": 
                // draw base UI
                UIsprites[1].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                // draw both health
                battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, true);
                battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, false);
                // arrow handling logic
                battleUI.DrawArrow(_WildUIAtlas, spriteBatch);
                battleUI.moveArrow();
                // Draw pokémon last (on top) with attack offsets
                String playersPokemon = currentPokemon.Name.ToString();
                Sprite currentMon = PokemonBackFactory.Instance.CreateStaticSprite(playersPokemon.ToLower() + "-back");
                Vector2 playerOffsetMenu = Vector2.Zero;
                if (playerAttackAnimationPlaying)
                {
                    playerOffsetMenu = backState.AttackBackAction(currentMon, playerAttackAnimationTimer, AttackAnimationDurationMs);
                }
                currentMon.Draw(spriteBatch, Color.White, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)) + playerOffsetMenu, 4f);
                // draw wild pokemon sprite with potential attack offset
                Vector2 wildOffset = Vector2.Zero;
                if (enemyAttackAnimationPlaying)
                {
                    wildOffset = frontState.AttackFrontAction(_wildPokemonSpriteFront, enemyAttackAnimationTimer, AttackAnimationDurationMs);
                }
                _wildPokemonSpriteFront.Draw(spriteBatch, Color.White, wildPokemonPosition + wildOffset, 4f);
                break;
            case "Fight":
                // draw base UI
                UIsprites[2].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                // draw both health
                battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, true);
                battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, false);
                KeyboardState keysState = Keyboard.GetState();
                if (keysState.IsKeyDown(Keys.Tab))
                {
                    resetBattle = true;
                }
                break;
            case "Item": // Bag
                // draw base UI
                UIsprites[4].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                // only draw opponent health
                battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, false);
                KeyboardState currentState = Keyboard.GetState();
                if (currentState.IsKeyDown(Keys.Tab))
                {
                    resetBattle = true;
                }
                break;
            case "Ball": // ball item selected TODO MOVE THIS WITHIN BAG SCENE
                // need to be able to process catch event
                break;
            case "PkMn":
            // draw base UI
                KeyboardState keyState = Keyboard.GetState();
                UIsprites[3].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                if (keyState.IsKeyDown(Keys.Tab))
                {
                    resetBattle = true;
                }
                break;
            case "Run":
                didRunOrCatch = true;
                break;
            default:
                break;
        }
    }

    private void TrySetAnimation(AnimatedSprite animSprite, string[] candidates)
    {
        if (animSprite == null) return;
        var atlas = PokemonFrontFactory.Instance.Atlas;
        if (atlas == null) return;

        foreach (var c in candidates)
        {
            if (string.IsNullOrEmpty(c)) continue;
            try
            {
                if (atlas._animations.ContainsKey(c))
                {
                    animSprite.Animation = atlas.GetAnimation(c);
                    return;
                }
                var lower = c.ToLower();
                if (atlas._animations.ContainsKey(lower))
                {
                    animSprite.Animation = atlas.GetAnimation(lower);
                    return;
                }
            }
            catch { }
        }
    }
}