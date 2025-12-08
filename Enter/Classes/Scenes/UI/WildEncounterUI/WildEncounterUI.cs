using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using System;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.GameState;
using Enter.Classes.Characters;
using PokemonGame;

public partial class WildEncounterUI
{
    private Color pokemonBackgroundColor = new Color(246, 232, 248);
    private Sprite[] UIBaseSprites;
    private TextureAtlas _WildUIAtlas;
    private TextureAtlas _BattleCharactersAtlas;
    private TextureAtlas _BordersAtlas;
    private TextSprite _wildPokemonMessage1;
    private TextSprite _wildPokemonMessage2;
    private SpriteFont _font;
    static private float _scale = 4.0f;
    private Sprite _trainerSpriteBack;
    private String _wildPokemonID;
    private AnimatedSprite _wildPokemonSpriteFront;
    
    // Animation attack state tracking
    private bool shouldPlayPlayerAttackAnimation = false;
    private bool shouldPlayEnemyAttackAnimation = false;
    private bool playerAttackAnimationPlaying = false;
    private bool enemyAttackAnimationPlaying = false;
    private double playerAttackAnimationTimer = 0.0;
    private double enemyAttackAnimationTimer = 0.0;
    private const double AttackAnimationDurationMs = 350.0;  // Faster player movement (reduced from 600ms)
    private FrontStateAction frontState = new FrontStateAction();
    private BackStateAction backState = new BackStateAction();

    // Pre defined regions within UI
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
    
    private string _currentState = "Initial";
    private Dictionary<string, int> stateMapping = new Dictionary<string, int>
    {
        { "Initial", 0 },
        { "Menu", 1 },
        { "Fight", 2 },
        { "Bag", 3 },
        { "Pokemon", 4 },
        { "Run", 5 }
    };

    public WildEncounterUI(TextureAtlas wildUIAtlas, TextureAtlas battleCharactersAtlas, TextureAtlas bordersAtlas, ContentManager content, Player ourPlayer)
    {
        _WildUIAtlas = wildUIAtlas;
        _BattleCharactersAtlas = battleCharactersAtlas;
        _BordersAtlas = bordersAtlas;
        _wildPokemonID = PokemonGenerator.GenerateWildPokemon().Species.Name.ToLower();
        _font = content.Load<SpriteFont>("PokemonFont");
        _Player = ourPlayer;
    }

    public void LoadContent(ContentManager content)
    {
        // Load health bar sprites from the WildUIAtlas
        greenBar = new Sprite(_WildUIAtlas.GetRegion("green-health"));
        yellowBar = new Sprite(_WildUIAtlas.GetRegion("yellow-health"));
        redBar = new Sprite(_WildUIAtlas.GetRegion("red-health"));

        UIBaseSprites = new Sprite[6]; // Initialize with fixed size for the 6 UI states
        UIBaseSprites[0] = _WildUIAtlas.CreateSprite("battle-wild");     // Initial state
        UIBaseSprites[1] = _WildUIAtlas.CreateSprite("battle-menu");       // Menu state
        UIBaseSprites[2] = _WildUIAtlas.CreateSprite("battle-attack");     // Fight state
        UIBaseSprites[3] = _WildUIAtlas.CreateSprite("battle-item");       // Bag state
        UIBaseSprites[4] = _WildUIAtlas.CreateSprite("battle-pokemon");    // Pokemon state
        UIBaseSprites[5] = _WildUIAtlas.CreateSprite("battle-item");       // Run state

        // Load trainer sprites from the BattleCharacters atlas
        _trainerSpriteBack = new Sprite(_BattleCharactersAtlas.GetRegion("player-back"));

        // Load wild pokemon sprite
        PokemonFrontFactory.Instance.LoadAllTextures(content);
        PokemonBackFactory.Instance.LoadAllTextures(content);
        _wildPokemonSpriteFront = PokemonFrontFactory.Instance.CreateAnimatedSprite(_wildPokemonID + "-front");

        // Load border from Borders atlas
        _border = new Sprite(_BordersAtlas.GetRegion("blue-border"));

        // Create text sprites for "Wild ___ appeared!" messages
        _wildPokemonMessage1 = new TextSprite("Wild    " + _wildPokemonID.ToUpper(), _font, Color.Black);
        _wildPokemonMessage2 = new TextSprite("appeared!", _font, Color.Black);
    }

    public void Update(GameTime gameTime)
    {
        // Update UI state machine
        battleUI.Update(gameTime);
        _currentState = battleUI.getBattleState();

        // Advance animated sprite frames only while attacking
        if (enemyAttackAnimationPlaying)
        {
            _wildPokemonSpriteFront?.Update(gameTime);
        }

        // Input: pressing A triggers both player's back attack motion and wild front attack motion
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            shouldPlayEnemyAttackAnimation = true;
            enemyAttackAnimationPlaying = false;
            shouldPlayPlayerAttackAnimation = true;
            playerAttackAnimationPlaying = false;
        }

        // Start animations when flagged
        if (shouldPlayEnemyAttackAnimation && !enemyAttackAnimationPlaying)
        {
            enemyAttackAnimationPlaying = true;
            enemyAttackAnimationTimer = 0.0;
            TrySetAnimation(_wildPokemonSpriteFront, new string[] { _wildPokemonID + "-front" });
            // Slow down enemy animation playback to 0.2x speed (half of previous speed) for single play-through
            if (_wildPokemonSpriteFront != null)
            {
                _wildPokemonSpriteFront.AnimationSpeedMultiplier = 0.2;
                _wildPokemonSpriteFront.Loop = false;
            }
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

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw border first (at the very bottom layer)
        _border.Draw(spriteBatch, Color.White, _borderPostion, 4f);

        // Always get player's pokemon
        Pokemon currentPokemon = _Player.thePlayersTeam.Pokemons[0];

        // Draw the UI elements for wild encounter (state based)
        switch (_currentState)
        {
            case "Initial":
                DrawState_Initial(spriteBatch, currentPokemon);
                break;
            case "Menu":
                DrawState_Menu(spriteBatch, currentPokemon);
                break;
            case "Fight":
                DrawState_Fight(spriteBatch, currentPokemon);
                break;
            case "Item": // Bag
                DrawState_Bag(spriteBatch, currentPokemon);
                break;
            case "Ball": // ball item selected TODO MOVE THIS WITHIN BAG SCENE
                // need to be able to process catch event
                break;
            case "PkMn":
                DrawState_Pokemon(spriteBatch, currentPokemon);
                break;
            case "Run":
                DrawState_Run(spriteBatch, currentPokemon);
                break;
            default:
                UIBaseSprites[0].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
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
