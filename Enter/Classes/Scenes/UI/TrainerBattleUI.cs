using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using System;
using System.Data;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.GameState;
using PokemonGame;
using Microsoft.Xna.Framework.Input;

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
    // removed unused field _enemyPokemonSpriteFront (we use Pokemon.AnimatedSprite instead)
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

    private int playerCurrentHP = 0;
    private int enemyCurrentHP = 0;
    private int playerMaxHP = 0;
    private int enemyMaxHP = 0;
    private bool battleInitialized = false;
    private string battleMessage = "";
    private bool endMessageActive = false;
    private double endMessageTimer = 0.0;
    private const double EndMessagePauseMs = 4000.0;

    // Animation state tracking
    private bool shouldPlayPlayerAttackAnimation = false;
    private bool shouldPlayEnemyAttackAnimation = false;
    private bool playerAttackAnimationPlaying = false;
    private bool enemyAttackAnimationPlaying = false;
    private double playerAttackAnimationTimer = 0.0;
    private double enemyAttackAnimationTimer = 0.0;
    private const double AttackAnimationDurationMs = 600.0;
    // State action instances used to compute positional offsets for attack motions
    private FrontStateAction frontState = new FrontStateAction();
    private BackStateAction backState = new BackStateAction();

    // Turn-based system
    private enum BattleTurn { Player, Cpu, Waiting, End }
    private BattleTurn currentTurn = BattleTurn.Player;
    private double turnTimer = 0.0;
    private const double CpuAttackDelayMs = 2000.0;

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
        // Ensure battleUI state machine and timer are updated
        battleUI.Update(gameTime);
        _currentState = battleUI.getBattleState();
        
        // Update any animated sprites attached to the current pokémon
        try
        {
            Pokemon currentPokemon = _playerTeam?.Pokemons?[0];
            Pokemon enemyPokemon = _enemyTeam?.Pokemons?[0];
            // Only advance animated sprite frames while the pokemon is performing an attack animation
            if (enemyAttackAnimationPlaying)
            {
                enemyPokemon?.AnimatedSprite?.Update(gameTime);
            }
            if (playerAttackAnimationPlaying)
            {
                currentPokemon?.AnimatedSprite?.Update(gameTime);
            }
            
            // Trigger attack animation on flag transition (only set it once)
            if (shouldPlayEnemyAttackAnimation && !enemyAttackAnimationPlaying && enemyPokemon != null)
            {
                TrySetPokemonAnimation(enemyPokemon, new string[] {
                    enemyPokemon.Name.ToString().ToLower() + "-front",
                });
                enemyAttackAnimationPlaying = true;
                enemyAttackAnimationTimer = 0.0;
            }
            
            if (shouldPlayPlayerAttackAnimation && !playerAttackAnimationPlaying && currentPokemon != null)
            {
                TrySetPokemonAnimation(currentPokemon, new string[] {
                    currentPokemon.Name.ToString().ToLower() + "-back",
                    currentPokemon.Name.ToString().ToLower() + "-front"
                });
                playerAttackAnimationPlaying = true;
                playerAttackAnimationTimer = 0.0;
            }
        }
        catch { }

        // Handle end message pause
        if (endMessageActive)
        {
            endMessageTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (endMessageTimer >= EndMessagePauseMs)
            {
                resetBattle = true;
                endMessageActive = false;
            }
        }

        // Handle CPU turn timer
        if (currentTurn == BattleTurn.Waiting)
        {
            turnTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (turnTimer >= CpuAttackDelayMs)
            {
                // CPU attacks
                int enemyDmg = new Random().Next(0, 11);
                playerCurrentHP -= enemyDmg;
                if (playerCurrentHP < 0) playerCurrentHP = 0;
                string enemyMsg = "Enemy attacks! ";
                if (enemyDmg == 10)
                    enemyMsg += "Critical hit! ";
                else if (enemyDmg == 0)
                    enemyMsg += "Enemy missed! ";
                else
                    enemyMsg += $"Player loses {enemyDmg} HP.";
                battleMessage = enemyMsg;
                
                // Trigger enemy attack animation only when damage occurs
                shouldPlayEnemyAttackAnimation = true;
                enemyAttackAnimationPlaying = false; // Will be set true in Update when animation is triggered
                
                if (playerCurrentHP <= 0)
                {
                    battleMessage = "You lose!";
                    endMessageActive = true;
                    endMessageTimer = 0.0;
                    currentTurn = BattleTurn.End;
                }
                else
                {
                    currentTurn = BattleTurn.Player;
                }
                turnTimer = 0.0;
            }
        }

        // Update attack animation timers and revert to idle after duration
        if (enemyAttackAnimationPlaying)
        {
            enemyAttackAnimationTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (enemyAttackAnimationTimer >= AttackAnimationDurationMs)
            {
                enemyAttackAnimationPlaying = false;
                shouldPlayEnemyAttackAnimation = false;
                // Revert to idle animation
                Pokemon enemyPokemon = _enemyTeam?.Pokemons?[0];
                if (enemyPokemon?.AnimatedSprite != null)
                {
                    TrySetPokemonAnimation(enemyPokemon, new string[] {
                        enemyPokemon.Name.ToString().ToLower() + "-front",
                    });
                }
            }
        }

        if (playerAttackAnimationPlaying)
        {
            playerAttackAnimationTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (playerAttackAnimationTimer >= AttackAnimationDurationMs)
            {
                playerAttackAnimationPlaying = false;
                shouldPlayPlayerAttackAnimation = false;
                // Revert to idle animation
                Pokemon currentPokemon = _playerTeam?.Pokemons?[0];
                if (currentPokemon?.AnimatedSprite != null)
                {
                    TrySetPokemonAnimation(currentPokemon, new string[] {
                        currentPokemon.Name.ToString().ToLower() + "-back",
                        currentPokemon.Name.ToString().ToLower() + "-front"
                    });
                }
            }
        }
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

    private void DrawHP(SpriteBatch spriteBatch, int hp, int maxHp, Vector2 pos, string label)
    {
        // Draw both HP in black
        spriteBatch.DrawString(_font, $"{label}: {hp}/{maxHp}", pos, Color.Black);
    }

    private void DrawMessage(SpriteBatch spriteBatch, string message)
    {
        // Draw all messages at a slightly higher Y position
        Vector2 msgPos = new Vector2(340, 320);
        Color color = Color.Yellow;
        if (message.Contains("You win!")) color = Color.LawnGreen;
        else if (message.Contains("You lose!")) color = Color.Red;
        else if (message.StartsWith("Player attacks!")) color = Color.LawnGreen;
        else if (message.StartsWith("Enemy attacks!")) color = Color.Red;
        // Draw instructional messages in black and higher
        if (message == "Press A to use Tackle" || message == "Use arrow keys to navigate and Enter to select")
        {
            msgPos.Y -= 50;
            color = Color.Black;
        }
        spriteBatch.DrawString(_font, message, msgPos, color);
    }

    public void WildEncounterStateBasedDraw(Sprite[] UI_BaseSprites, SpriteBatch spriteBatch)
    {
        // always draw border
        _border.Draw(spriteBatch, Color.White, _borderPostion, 4f);

        // always get players pokemon
        Pokemon currentPokemon = _playerTeam.Pokemons[0];
        Pokemon enemyPokemon = _enemyTeam.Pokemons[0];
        // Ensure the pokémon have an AnimatedSprite instance created once (do not recreate every draw)
        if (enemyPokemon.AnimatedSprite == null)
        {
            try
            {
                var s = PokemonFrontFactory.Instance.CreateAnimatedSprite(enemyPokemon.Name.ToString().ToLower() + "-front");
                enemyPokemon.SetAnimatedSprite(s);
            }
            catch { }
        }
        if (currentPokemon.AnimatedSprite == null)
        {
            try
            {
                var s = PokemonFrontFactory.Instance.CreateAnimatedSprite(currentPokemon.Name.ToString().ToLower() + "-front");
                currentPokemon.SetAnimatedSprite(s);
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

        // draw the UI elements for wild encounter (state based)
        switch (_currentState)
        {
            case "Initial":
                UIBaseSprites[0].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                _trainerSpriteBack.Draw(spriteBatch, Color.White, playerTrainerPosition, 8f);
                _enemyTrainerSpriteFront.Draw(spriteBatch, Color.White, enemyTrainerPosition, 4f);
                _enemyTrainerIDSprite.DrawTextSpriteWithScale(spriteBatch, enemyTrainerIDPosition, 2f);
                BattleUIHelper.drawPokeballSprites(_playerTeam, _TrainerUIAtlas, spriteBatch, true);
                BattleUIHelper.drawPokeballSprites(_enemyTeam, _TrainerUIAtlas, spriteBatch, false);
                break;
            case "Menu":
                UIBaseSprites[1].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                String playersPokemon = currentPokemon.Name.ToString();
                Sprite currentMon = PokemonBackFactory.Instance.CreateStaticSprite(playersPokemon.ToLower() + "-back");
                Vector2 playerOffsetMenu = Vector2.Zero;
                if (playerAttackAnimationPlaying)
                {
                    playerOffsetMenu = backState.AttackBackAction(currentMon, playerAttackAnimationTimer, AttackAnimationDurationMs);
                }
                currentMon.Draw(spriteBatch, Color.White, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)) + playerOffsetMenu, 4f);
                Vector2 enemyOffsetMenu = Vector2.Zero;
                if (enemyAttackAnimationPlaying)
                {
                    enemyOffsetMenu = frontState.AttackFrontAction(enemyPokemon.AnimatedSprite, enemyAttackAnimationTimer, AttackAnimationDurationMs);
                }
                enemyPokemon.AnimatedSprite?.Draw(spriteBatch, Color.White, enemysPokemonPosition + enemyOffsetMenu, 4f);
                // Use updated HP for health bars
                battleUI.drawHealthBar(playerCurrentHP, playerMaxHP, greenBar, yellowBar, redBar, spriteBatch, true);
                battleUI.drawHealthBar(enemyCurrentHP, enemyMaxHP, greenBar, yellowBar, redBar, spriteBatch, false);
                DrawHP(spriteBatch, playerCurrentHP, playerMaxHP, new Vector2(340, 220), "Player HP");
                DrawHP(spriteBatch, enemyCurrentHP, enemyMaxHP, new Vector2(700, 100), "Enemy HP");
                if (!string.IsNullOrEmpty(battleMessage))
                {
                    DrawMessage(spriteBatch, battleMessage);
                }
                // Show menu navigation instructions
                DrawMessage(spriteBatch, "Use arrow keys to navigate and Enter to select");
                // Enable menu navigation
                battleUI.moveArrow();
                battleUI.DrawArrow(_TrainerUIAtlas, spriteBatch);
                break;
            case "Fight":
                UIBaseSprites[1].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                playersPokemon = currentPokemon.Name.ToString();
                currentMon = PokemonBackFactory.Instance.CreateStaticSprite(playersPokemon.ToLower() + "-back");
                if (!endMessageActive && currentTurn == BattleTurn.Player && Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    // Player attacks enemy (random damage 0-20)
                    int playerDmg = new Random().Next(0, 21);
                    string playerMsg = "Player attacks! ";
                    if (playerDmg == 20)
                        playerMsg += "Critical hit! ";
                    else if (playerDmg == 0)
                        playerMsg += "You missed! ";
                    else
                        playerMsg += $"Enemy loses {playerDmg} HP.";
                    enemyCurrentHP -= playerDmg;
                    if (enemyCurrentHP < 0) enemyCurrentHP = 0;
                    
                    // Trigger player attack animation when player attacks
                    shouldPlayPlayerAttackAnimation = true;
                    playerAttackAnimationPlaying = false; // Will be set true in Update when animation is triggered
                    
                    battleMessage = playerMsg;
                    if (enemyCurrentHP <= 0)
                    {
                        battleMessage = "You win!";
                        endMessageActive = true;
                        endMessageTimer = 0.0;
                        currentTurn = BattleTurn.End;
                    }
                    else
                    {
                        currentTurn = BattleTurn.Waiting;
                        turnTimer = 0.0;
                    }
                }
                // Use updated HP for health bars
                battleUI.drawHealthBar(playerCurrentHP, playerMaxHP, greenBar, yellowBar, redBar, spriteBatch, true);
                battleUI.drawHealthBar(enemyCurrentHP, enemyMaxHP, greenBar, yellowBar, redBar, spriteBatch, false);
                DrawHP(spriteBatch, playerCurrentHP, playerMaxHP, new Vector2(340, 220), "Player HP");
                DrawHP(spriteBatch, enemyCurrentHP, enemyMaxHP, new Vector2(700, 100), "Enemy HP");
                // draw after HP bars so its on top of gaint gray bars
                Vector2 playerOffsetFight = Vector2.Zero;
                if (playerAttackAnimationPlaying)
                {
                    playerOffsetFight = backState.AttackBackAction(currentMon, playerAttackAnimationTimer, AttackAnimationDurationMs);
                }
                currentMon.Draw(spriteBatch, Color.White, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)) + playerOffsetFight, 4f);
                Vector2 enemyOffsetFight = Vector2.Zero;
                if (enemyAttackAnimationPlaying)
                {
                    enemyOffsetFight = frontState.AttackFrontAction(enemyPokemon.AnimatedSprite, enemyAttackAnimationTimer, AttackAnimationDurationMs);
                }
                enemyPokemon.AnimatedSprite?.Draw(spriteBatch, Color.White, enemysPokemonPosition + enemyOffsetFight, 4f);
                if (!string.IsNullOrEmpty(battleMessage))
                {
                    DrawMessage(spriteBatch, battleMessage);
                }
                // Show fight instructions
                DrawMessage(spriteBatch, "Press A to use Tackle");
                break;
            case "Item": // Bag
                UIBaseSprites[4].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                KeyboardState keyState = Keyboard.GetState();
                if (keyState.IsKeyDown(Keys.Tab))
                {
                    resetBattle = true;
                }
                break;
            case "Run": // Run (blocked in trainer battles)
                // Show a message or just ignore; here, just ignore and return to menu
                resetBattle = true;
                break;
            case "PkMn": // Pokemon
                KeyboardState keyStates = Keyboard.GetState();
                UIBaseSprites[3].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                if (keyStates.IsKeyDown(Keys.Tab))
                {
                    resetBattle = true;
                }
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

    private void TrySetPokemonAnimation(Pokemon pokemon, string[] candidates)
    {
        if (pokemon == null || pokemon.AnimatedSprite == null) return;
        var atlas = PokemonFrontFactory.Instance.Atlas;
        if (atlas == null) return;

        foreach (var c in candidates)
        {
            if (string.IsNullOrEmpty(c)) continue;
            try
            {
                if (atlas._animations.ContainsKey(c))
                {
                    pokemon.AnimatedSprite.Animation = atlas.GetAnimation(c);
                    return;
                }
                var lower = c.ToLower();
                if (atlas._animations.ContainsKey(lower))
                {
                    pokemon.AnimatedSprite.Animation = atlas.GetAnimation(lower);
                    return;
                }
            }
            catch { }
        }
    }
}