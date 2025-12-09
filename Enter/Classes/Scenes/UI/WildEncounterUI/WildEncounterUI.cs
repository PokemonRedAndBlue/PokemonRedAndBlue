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
    private Pokemon _enemyPokemon;
    private PokemonInstance _enemyInstance;
    private AnimatedSprite _wildPokemonSpriteFront;
    private static readonly Random _rng = new Random();
    private KeyboardState _prevKeyboardState;
    
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
    static private Vector2 enemysPokemonPosition = new Vector2(uiBasePosition.X + (96 * _scale), uiBasePosition.Y + 20);
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
    public bool BagConfirmRequested { get; set; }
    private KeyboardState _prevBagKeyState;
    
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

    // Battle state tracking
    private int playerCurrentHP = 0;
    private int enemyCurrentHP = 0;
    private int playerMaxHP = 0;
    private int enemyMaxHP = 0;
    private bool battleInitialized = false;
    private string battleMessage = "";
    private bool endMessageActive = false;
    private double endMessageTimer = 0.0;
    private const double EndMessagePauseMs = 4000.0;

    // Damage flash effect timers
    private double enemyDamageFlashTimer = 0.0;
    private bool enemyTakingDamage = false;
    private double playerDamageFlashTimer = 0.0;
    private bool playerTakingDamage = false;
    private const double DamageFlashDurationMs = 200.0; // How long the red flash lasts

    // Faint/death animation
    private FaintStateAction faintState = new FaintStateAction();
    private bool enemyFainting = false;
    private double enemyFaintTimer = 0.0;
    private bool playerFainting = false;
    private double playerFaintTimer = 0.0;
    private const double FaintAnimationDurationMs = 1000.0; // How long the faint animation lasts

    // Turn-based system
    private enum BattleTurn { Player, Wild, Waiting, End }
    private BattleTurn currentTurn = BattleTurn.Player;
    private double turnTimer = 0.0;
    private const double CpuAttackDelayMs = 2000.0;

    public WildEncounterUI(TextureAtlas wildUIAtlas, TextureAtlas battleCharactersAtlas, TextureAtlas bordersAtlas, ContentManager content, Player ourPlayer)
    {
        _WildUIAtlas = wildUIAtlas;
        _BattleCharactersAtlas = battleCharactersAtlas;
        _BordersAtlas = bordersAtlas;
        _enemyInstance = PokemonGenerator.GenerateWildPokemon();
        _wildPokemonID = _enemyInstance.Species.Name.ToLower();
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
        _enemyPokemon = new Pokemon(_wildPokemonID, _enemyInstance?.Level ?? 5, PokemonView.Front, _wildPokemonSpriteFront, wildPokemonPosition);

        // Load border from Borders atlas
        _border = new Sprite(_BordersAtlas.GetRegion("blue-border"));

        // Create text sprites for "Wild ___ appeared!" messages
        _wildPokemonMessage1 = new TextSprite("Wild    " + _wildPokemonID.ToUpper(), _font, Color.Black);
        _wildPokemonMessage2 = new TextSprite("appeared!", _font, Color.Black);
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        // Clear bag confirm flag when not in Bag state
        if (_currentState != "Bag")
        {
            BagConfirmRequested = false;
        }

        // Update UI state machine
        battleUI.Update(gameTime);
        _currentState = battleUI.getBattleState();

        // Advance animated sprite frames only while attacking
        if (enemyAttackAnimationPlaying)
        {
            _wildPokemonSpriteFront?.Update(gameTime);
        }

        // Player input only during fight state
        if (_currentState == "Fight" && currentTurn == BattleTurn.Player && !endMessageActive && keyboardState.IsKeyDown(Keys.A) && _prevKeyboardState.IsKeyUp(Keys.A))
        {
            ResolvePlayerAttack();
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

        // Update damage flash timers
        if (enemyTakingDamage)
        {
            enemyDamageFlashTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (enemyDamageFlashTimer >= DamageFlashDurationMs)
            {
                enemyTakingDamage = false;
            }
        }

        if (playerTakingDamage)
        {
            playerDamageFlashTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (playerDamageFlashTimer >= DamageFlashDurationMs)
            {
                playerTakingDamage = false;
            }
        }

        // Update faint timers
        if (enemyFainting)
        {
            enemyFaintTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        if (playerFainting)
        {
            playerFaintTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        // Update end message timer
        if (endMessageActive)
        {
            endMessageTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (endMessageTimer >= EndMessagePauseMs)
            {
                endMessageActive = false;
                resetBattle = true;
            }
        }

        // Handle turn transitions (wild attack after delay)
        if (currentTurn == BattleTurn.Waiting)
        {
            turnTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (turnTimer >= CpuAttackDelayMs)
            {
                ResolveEnemyAttack();
            }
        }

        _prevKeyboardState = keyboardState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw border first (at the very bottom layer)
        _border.Draw(spriteBatch, Color.White, _borderPostion, 4f);

        // Always get player's pokemon
        Pokemon currentPokemon = _Player.thePlayersTeam.Pokemons[0];
        Pokemon enemyPokemon = _enemyPokemon ?? new Pokemon(_wildPokemonID ?? "unknown", _enemyInstance?.Level ?? 5, PokemonView.Front, _wildPokemonSpriteFront, wildPokemonPosition);

        if (enemyPokemon.AnimatedSprite == null)
        {
            try
            {
                var s = PokemonFrontFactory.Instance.CreateAnimatedSprite(enemyPokemon.Name.ToString().ToLower() + "-front");
                enemyPokemon.SetAnimatedSprite(s);
            }
            catch { }
        }

        // Initialize battle HP on first draw only
        if (!battleInitialized)
        {
            playerCurrentHP = currentPokemon.Hp > 0 ? currentPokemon.Hp : 50;
            playerMaxHP = currentPokemon.MaxHp > 0 ? currentPokemon.MaxHp : 50;
            enemyCurrentHP = enemyPokemon.Hp > 0 ? enemyPokemon.Hp : 50;
            enemyMaxHP = enemyPokemon.MaxHp > 0 ? enemyPokemon.MaxHp : 50;
            battleInitialized = true;
            battleMessage = "";
        }

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
                DrawState_Fight(spriteBatch, currentPokemon, enemyPokemon);
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

    private void ResolvePlayerAttack()
    {
        int playerDmg = _rng.Next(0, 21);
        string playerMsg = "Player attacks! ";
        if (playerDmg == 20)
            playerMsg += "Critical hit! ";
        else if (playerDmg == 0)
            playerMsg += "You missed! ";
        else
            playerMsg += $"Enemy loses {playerDmg} HP.";

        enemyCurrentHP -= playerDmg;
        if (enemyCurrentHP < 0) enemyCurrentHP = 0;

        shouldPlayPlayerAttackAnimation = true;
        playerAttackAnimationPlaying = false;

        SoundEffectPlayer.Play(SfxId.SFX_CYMBAL_3);

        enemyTakingDamage = true;
        enemyDamageFlashTimer = 0.0;

        battleMessage = playerMsg;
        if (enemyCurrentHP <= 0)
        {
            battleMessage = "You win!";
            enemyFainting = true;
            enemyFaintTimer = 0.0;
            endMessageActive = true;
            endMessageTimer = 0.0;
            BackgroundMusicPlayer.Play(SongId.VictoryTrainer, loop: false);
            currentTurn = BattleTurn.End;
        }
        else
        {
            currentTurn = BattleTurn.Waiting;
            turnTimer = 0.0;
        }
    }

    private void ResolveEnemyAttack()
    {
        turnTimer = 0.0;
        currentTurn = BattleTurn.Wild;

        int cpuDmg = _rng.Next(0, 21);
        string cpuMsg = "Enemy attacks! ";
        if (cpuDmg == 20)
            cpuMsg += "Critical hit! ";
        else if (cpuDmg == 0)
            cpuMsg += "Enemy missed! ";
        else
            cpuMsg += $"Player loses {cpuDmg} HP.";

        playerCurrentHP -= cpuDmg;
        if (playerCurrentHP < 0) playerCurrentHP = 0;

        shouldPlayEnemyAttackAnimation = true;
        enemyAttackAnimationPlaying = false;

        SoundEffectPlayer.Play(SfxId.SFX_CYMBAL_3);

        playerTakingDamage = true;
        playerDamageFlashTimer = 0.0;

        battleMessage = cpuMsg;
        if (playerCurrentHP <= 0)
        {
            battleMessage = "You lose!";
            playerFainting = true;
            playerFaintTimer = 0.0;
            endMessageActive = true;
            endMessageTimer = 0.0;
            currentTurn = BattleTurn.End;
        }
        else
        {
            currentTurn = BattleTurn.Player;
        }
    }

    private void DrawHP(SpriteBatch spriteBatch, int hp, int maxHp, Vector2 pos, string label)
    {
        spriteBatch.DrawString(_font, $"{label}: {hp}/{maxHp}", pos, Color.Black);
    }

    private void DrawMessage(SpriteBatch spriteBatch, string message)
    {
        Vector2 msgPos = new Vector2(340, 320);
        Color color = Color.Yellow;
        if (message.Contains("You win!")) color = Color.LawnGreen;
        else if (message.Contains("You lose!")) color = Color.Red;
        else if (message.StartsWith("Player attacks!")) color = Color.LawnGreen;
        else if (message.StartsWith("Enemy attacks!")) color = Color.Red;

        if (message == "Press A to use Tackle" || message == "Use arrow keys to navigate and Enter to select")
        {
            msgPos.Y -= 50;
            color = Color.Black;
        }

        spriteBatch.DrawString(_font, message, msgPos, color);
    }
}
