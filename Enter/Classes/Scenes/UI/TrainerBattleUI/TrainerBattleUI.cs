using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Enter.Classes.Animations;
using Enter.Classes.GameState;
using PokemonGame;
using Microsoft.Xna.Framework.Input;

public partial class TrainerBattleUI
{
    private Color pokemonBackgroundColor = new Color(246, 232, 248);
    private Sprite[] UIBaseSprites;
    private TextureAtlas _TrainerUIAtlas;
    private TextureAtlas _BattleCharactersAtlas;
    private TextureAtlas _BordersAtlas;
    private TextureAtlas _creatureAtlas;
    private ContentManager _content;
    private bool _creatureSpriteMissing;
    private static readonly Random _rng = new Random();
    private Move _playerMove = SafeDefaultMove();
    private Move _enemyMove = SafeDefaultMove();
    public bool ItemConfirmRequested { get; private set; }
    private KeyboardState _prevItemKeyState;
    private KeyboardState _prevFightKeyState;
    private int _playerMoveIndex = 0;
    private SpriteFont _font;
    private BattleUIHelper battleUI = new BattleUIHelper();
    private String _enemyTrainerString;
    static private float _scale = 4.0f;
    private Sprite _trainerSpriteBack;
    // removed unused field _enemyPokemonSpriteFront (we use Pokemon.AnimatedSprite instead)
    private Sprite _enemyTrainerSpriteFront;
    private float _enemyTrainerScale = 4.0f;
    private TextSprite _enemyTrainerIDSprite;
    private Sprite greenBar, yellowBar, redBar;
    private string _currentState = "Initial";
    private string _prevState = "Initial";

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
    static private Vector2 enemysPokemonPosition = new Vector2(uiBasePosition.X + (96 * _scale), uiBasePosition.Y + 20);
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
    private const double AttackAnimationDurationMs = 350.0;  // Faster player movement (reduced from 600ms)
    // State action instances used to compute positional offsets for attack motions
    private FrontStateAction frontState = new FrontStateAction();
    private BackStateAction backState = new BackStateAction();
    
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
    private enum BattleTurn { Player, Cpu, Waiting, End }
    private BattleTurn currentTurn = BattleTurn.Player;
    private double turnTimer = 0.0;
    private const double CpuAttackDelayMs = 2000.0;

    // Deploy animation for player's pokemon
    private PokeballthrowAnimation _playerDeployThrow;
    private bool _playerDeploying = true;
    private bool _playerDeploySetup = false;
    private Vector2 _playerDeployTarget;

    public TrainerBattleUI(TextureAtlas trainerUIAtlas, TextureAtlas battleCharactersAtlas, TextureAtlas bordersAtlas, ContentManager content, String enemyTrainerID, Team playerTeam, Team enemyTeam)
    {
        // init class vars
        _TrainerUIAtlas = trainerUIAtlas;
        _BattleCharactersAtlas = battleCharactersAtlas;
        _BordersAtlas = bordersAtlas;
        _enemyTrainerString = enemyTrainerID?.ToLower() ?? string.Empty;
        _font = content.Load<SpriteFont>("PokemonFont");
        _playerTeam = playerTeam;
        _enemyTeam = enemyTeam;
    }

    public void LoadContent(ContentManager content)
    {
        _content = content;
        // Load UI sprites from the TrainerUIAtlas
        greenBar = new Sprite(_TrainerUIAtlas.GetRegion("green-health"));
        yellowBar = new Sprite(_TrainerUIAtlas.GetRegion("yellow-health"));
        redBar = new Sprite(_TrainerUIAtlas.GetRegion("red-health"));

        UIBaseSprites = new Sprite[6]; // Initialize with fixed size for the 6 UI states (Initial, Menu, Fight, Item, PkMn, Run)
        UIBaseSprites[0] = _TrainerUIAtlas.CreateSprite("battle-prompt");   // Initial state
        UIBaseSprites[1] = _TrainerUIAtlas.CreateSprite("battle-menu");     // Menu state
        UIBaseSprites[2] = _TrainerUIAtlas.CreateSprite("battle-attack");   // Fight state
        UIBaseSprites[3] = _TrainerUIAtlas.CreateSprite("battle-item");     // Item state
        UIBaseSprites[4] = _TrainerUIAtlas.CreateSprite("battle-pokemon");  // PkMn state
        UIBaseSprites[5] = _TrainerUIAtlas.CreateSprite("battle-run");      // Run state

        // Load trainer sprites from the BattleCharacters atlas (use player-back like WildEncounterUI)
        _trainerSpriteBack = new Sprite(_BattleCharactersAtlas.GetRegion("player-back"));

        if (_enemyTrainerString == "trainer-painter")
        {
            // Painter battle portrait comes from its own atlas; scale to roughly player trainer size
            TextureAtlas painterAtlas = TextureAtlas.FromFile(content, "PainterBattle.xml");
            var region = painterAtlas.GetRegion("trainer-painter");
            _enemyTrainerSpriteFront = new Sprite(region);

            // Match player trainer height (~player-back is ~30px tall, drawn at scale 8 -> ~240px onscreen)
            const float targetHeight = 240f;
            float h = region.Height;
            _enemyTrainerScale = h > 0 ? targetHeight / h : _enemyTrainerScale;
        }
        else
        {
            _enemyTrainerSpriteFront = _BattleCharactersAtlas.CreateSprite(_enemyTrainerString);
        }

        // Load enemy trainer ID sprite
        _enemyTrainerIDSprite = new TextSprite(formatTrainerName(_enemyTrainerString), _font, Color.Black);

        // Load border from Borders atlas
        _border = _BordersAtlas.CreateSprite("blue-border");
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var previousState = _currentState;
        // Update deploy throw animation
        if (_playerDeployThrow != null && _playerDeploying)
        {
            _playerDeployThrow.Update(gameTime);
            if (_playerDeployThrow.IsComplete)
            {
                _playerDeploying = false;
            }
        }
        // Ensure battleUI state machine and timer are updated
        battleUI.Update(gameTime);
        _currentState = battleUI.getBattleState();
        _prevState = previousState;

        // Initialize deploy throw when first entering Menu
        if (_currentState == "Menu" && _prevState != "Menu" && !_playerDeploySetup)
        {
            var currentPokemon = _playerTeam?.Pokemons?[0];
            if (currentPokemon != null)
            {
                EnsurePlayerDeploySetup(currentPokemon);
            }
        }

        if (_currentState != "Item")
        {
            ItemConfirmRequested = false;
        }

        if (_currentState == "Fight" && currentTurn == BattleTurn.Player && !endMessageActive)
        {
            UpdateMoveSelectionFight(keyboardState);
        }
        
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
                // Slow down enemy animation playback to 0.2x speed (half of previous speed) for single play-through
                enemyPokemon.AnimatedSprite.AnimationSpeedMultiplier = 0.2;
                enemyPokemon.AnimatedSprite.Loop = false;
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

        // Update attack animation timers
        if (playerAttackAnimationPlaying)
        {
            playerAttackAnimationTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (playerAttackAnimationTimer >= AttackAnimationDurationMs)
            {
                playerAttackAnimationPlaying = false;
                shouldPlayPlayerAttackAnimation = false;
            }
        }

        if (enemyAttackAnimationPlaying)
        {
            enemyAttackAnimationTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (enemyAttackAnimationTimer >= AttackAnimationDurationMs)
            {
                enemyAttackAnimationPlaying = false;
                shouldPlayEnemyAttackAnimation = false;
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

        // Handle turn transitions
        if (currentTurn == BattleTurn.Waiting)
        {
            turnTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (turnTimer >= CpuAttackDelayMs)
            {
                // Switch to CPU turn and trigger enemy attack
                currentTurn = BattleTurn.Cpu;
                
                Pokemon currentPokemon = _playerTeam?.Pokemons?[0];
                Pokemon enemyPokemon = _enemyTeam?.Pokemons?[0];
                
                string cpuMsg;
                var enemyMove = _enemyMove ?? SafeDefaultMove();
                ApplyAttack(enemyPokemon, enemyMove, ref playerCurrentHP, currentPokemon?.Name.ToString() ?? "Player", out cpuMsg);
                
                // Trigger enemy attack animation
                shouldPlayEnemyAttackAnimation = true;
                enemyAttackAnimationPlaying = false;

                // CPU Attack SFX
                SoundEffectPlayer.Play(SfxId.SFX_CYMBAL_3);
                
                // Trigger player damage flash
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
                    // Return to player's turn after CPU attack animation finishes
                    currentTurn = BattleTurn.Player;
                }
            }
        }

        _prevFightKeyState = keyboardState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw border first (at the very bottom layer)
        _border.Draw(spriteBatch, Color.White, _borderPostion, 4f);

        // always get players pokemon
        Pokemon currentPokemon = _playerTeam.Pokemons[0];
        Pokemon enemyPokemon = _enemyTeam.Pokemons[0];

        // Hard-code creature sprite hookup for painter battles even if atlas key resolution fails elsewhere
        EnsureEnemyCreatureSprite(enemyPokemon);

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

            // Seed default moves from species data
            _playerMoveIndex = 0;
            _playerMove = ResolveMoveForPokemonName(currentPokemon?.Name);
            _enemyMove = ResolveMoveForPokemonName(enemyPokemon?.Name);
        }

        // draw the UI elements for trainer battle (state based)
        switch (_currentState)
        {
            case "Initial":
                DrawState_Initial(spriteBatch, currentPokemon, enemyPokemon);
                break;
            case "Menu":
                DrawState_Menu(spriteBatch, currentPokemon, enemyPokemon);
                break;
            case "Fight":
                DrawState_Fight(spriteBatch, currentPokemon, enemyPokemon);
                break;
            case "Item": // Bag
                DrawState_Item(spriteBatch, currentPokemon, enemyPokemon);
                break;
            case "Ball": // ball item selected TODO MOVE THIS WITHIN BAG SCENE
                // need to be able to process catch event
                break;
            case "PkMn":
                DrawState_PkMn(spriteBatch, currentPokemon, enemyPokemon);
                break;
            case "Run":
                DrawState_Run(spriteBatch, currentPokemon, enemyPokemon);
                break;
            default:
                UIBaseSprites[0].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                break;
        }

        // Overlay a debug text if the creature sprite failed to resolve
        if (_creatureSpriteMissing)
        {
            spriteBatch.DrawString(_font, "Missing creature sprite", new Vector2(40, 40), Color.Red);
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

    private void EnsurePlayerDeploySetup(Pokemon currentPokemon)
    {
        if (_playerDeploySetup || currentPokemon == null)
            return;

        var backSprite = PokemonBackFactory.Instance.CreateStaticSprite(currentPokemon.Name.ToLower() + "-back");
        _playerDeployTarget = GetPlayerMonDrawPos(backSprite);
        var throwStart = playerTrainerPosition + new Vector2(-24f, -12f); // start slightly behind/above trainer
        if (Vector2.Distance(throwStart, _playerDeployTarget) < 1f)
        {
            throwStart += new Vector2(-32f, -16f); // ensure start and target differ so arc is visible
        }
        _playerDeployThrow = new PokeballthrowAnimation((int)throwStart.X, (int)throwStart.Y, _playerDeployTarget);
        battleMessage = "Trainer deployed " + (currentPokemon?.Name ?? "Pokemon") + "!";
        if (_content != null)
        {
            _playerDeployThrow.LoadContent(_content);
        }
        _playerDeploySetup = true;
        _playerDeploying = true;
    }

    private Vector2 GetPlayerMonDrawPos(Sprite backSprite)
    {
        return new Vector2(playerPosition.X, maxDrawPos.Y + (-backSprite.Height * _scale));
    }

    private static Move SafeDefaultMove()
    {
        try
        {
            return MoveDatabase.Get("Tackle");
        }
        catch
        {
            return new Move
            {
                Name = "Tackle",
                Type = "Normal",
                Power = 15,
                Category = MoveCategory.Physical
            };
        }
    }

    private static int ComputeDamage(Move move)
    {
        move ??= SafeDefaultMove();
        int basePower = move.Power > 0 ? move.Power : 10;
        int variance = _rng.Next(-5, 6); // small +/- variance
        int damage = Math.Max(1, basePower + variance);
        return damage;
    }

    private static int ApplyAttack(Pokemon attacker, Move move, ref int targetHp, string targetName, out string message)
    {
        string attackerName = attacker?.Name ?? "Pokemon";
        move ??= SafeDefaultMove();

        // Simple miss/crit logic to keep battles dynamic
        bool miss = _rng.Next(0, 100) < 5;          // 5% miss chance
        bool crit = _rng.Next(0, 100) > 90;         // 9% crit chance

        if (miss)
        {
            message = attackerName + " used " + move.Name + ", but missed!";
            return 0;
        }

        int damage = ComputeDamage(move);

        double typeMult = 1.0;
        try
        {
            var targetSpecies = PokemonGenerator.GenerateSpeciesByName(targetName);
            typeMult = DamageCalculator.GetTypeEffectiveness(move.Type, targetSpecies.Type1, targetSpecies.Type2);
        }
        catch { }

        if (typeMult == 0)
        {
            message = attackerName + " used " + move.Name + "! It doesn't affect " + targetName + ".";
            return 0;
        }

        damage = (int)Math.Round(damage * typeMult);
        if (crit)
        {
            damage = (int)Math.Round(damage * 1.5);
        }

        targetHp -= damage;
        if (targetHp < 0) targetHp = 0;

        if (crit)
        {
            message = attackerName + " used " + move.Name + "! Critical hit for " + damage + " damage.";
        }
        else
        {
            message = attackerName + " used " + move.Name + "! " + targetName + " lost " + damage + " HP.";
        }

        if (typeMult > 1.01)
        {
            message += " It's super effective!";
        }
        else if (typeMult < 0.99)
        {
            message += " It's not very effective.";
        }

        return damage;
    }

    private Move ResolveMoveForPokemonName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return SafeDefaultMove();
        try
        {
            var species = PokemonGenerator.GenerateSpeciesByName(name);
            var first = species?.Moves?.FirstOrDefault();
            return !string.IsNullOrWhiteSpace(first) ? MoveDatabase.Get(first) : SafeDefaultMove();
        }
        catch
        {
            return SafeDefaultMove();
        }
    }

    private void UpdateMoveSelectionFight(KeyboardState keyboardState)
    {
        var moves = _playerTeam?.Pokemons?[0]?.Moves;
        if (moves == null || moves.Count == 0) return;

        if (keyboardState.IsKeyDown(Keys.Left) && _prevFightKeyState.IsKeyUp(Keys.Left))
        {
            _playerMoveIndex = (_playerMoveIndex - 1 + moves.Count) % moves.Count;
        }
        else if (keyboardState.IsKeyDown(Keys.Right) && _prevFightKeyState.IsKeyUp(Keys.Right))
        {
            _playerMoveIndex = (_playerMoveIndex + 1) % moves.Count;
        }

        _playerMove = moves[Math.Clamp(_playerMoveIndex, 0, moves.Count - 1)];
    }

    private void DrawHP(SpriteBatch spriteBatch, int hp, int maxHp, int level, Vector2 pos, string label)
    {
        // Draw HP with level indicator and explicit HP label
        spriteBatch.DrawString(_font, $"{label} Lv{level} HP: {hp}/{maxHp}", pos, Color.Black);
    }

    private void DrawMessage(SpriteBatch spriteBatch, string message)
    {
        // Damage/attack messages sit above HP display; instruction sits at bottom of play area
        Vector2 dmgPos = new Vector2(345, 300); // shift right by 25 total
        Vector2 instrPos = new Vector2(365, 530); // shift right by 20
        float msgScale = 1.05f; // enlarge battle and instruction text
        Color color = Color.Black;
        Color effectColor = color;
        Color moveColor = Color.MediumPurple;

        // Instruction: split onto two lines (prompt + move name)
        if (message.StartsWith("press A to use"))
        {
            string moveName = message.Length > "press A to use ".Length
                ? message.Substring("press A to use ".Length)
                : "";
            spriteBatch.DrawString(_font, "press A to use", instrPos, color, 0f, Vector2.Zero, msgScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_font, moveName, instrPos + new Vector2(0, 24), moveColor, 0f, Vector2.Zero, msgScale, SpriteEffects.None, 0f);
            return;
        }

        // Effectiveness: split main line and effectiveness line
        string effectLine = null;
        string mainLine = message;
        bool looksLikeAttack = message.Contains(" used ") || message.Contains(" lost ") || message.Contains(" damage") || message.Contains("attack");
        if (message.Contains("It's super effective!"))
        {
            effectLine = "The attack was super effective!";
            mainLine = message.Replace(" It's super effective!", "");
            effectColor = Color.Green;
        }
        else if (message.Contains("It's not very effective."))
        {
            effectLine = "The attack was not very effective.";
            mainLine = message.Replace(" It's not very effective.", "");
            effectColor = Color.IndianRed;
        }
        else if (message.Contains("doesn't affect"))
        {
            effectLine = "The attack had no effect.";
            effectColor = Color.IndianRed;
        }
        else if (looksLikeAttack)
        {
            effectLine = "The attack was effective.";
            effectColor = Color.Green;
        }

        if (mainLine.Contains(" used "))
        {
            int usedIdx = mainLine.IndexOf(" used ", StringComparison.Ordinal);
            int afterIdx = mainLine.IndexOf("!", usedIdx >= 0 ? usedIdx : 0);
            if (afterIdx < 0) afterIdx = mainLine.Length;

            string before = mainLine.Substring(0, usedIdx + 6);
            int nameStart = usedIdx + 6;
            string moveName = mainLine.Substring(nameStart, Math.Max(0, afterIdx - nameStart));
            string after = mainLine.Substring(afterIdx);

            Vector2 pos = dmgPos;
            spriteBatch.DrawString(_font, before, pos, color, 0f, Vector2.Zero, msgScale, SpriteEffects.None, 0f);
            pos.X += _font.MeasureString(before).X * msgScale;

            spriteBatch.DrawString(_font, moveName, pos, moveColor, 0f, Vector2.Zero, msgScale, SpriteEffects.None, 0f);
            pos.X += _font.MeasureString(moveName).X * msgScale;

            spriteBatch.DrawString(_font, after, pos, color, 0f, Vector2.Zero, msgScale, SpriteEffects.None, 0f);
        }
        else
        {
            spriteBatch.DrawString(_font, mainLine, dmgPos, color, 0f, Vector2.Zero, msgScale, SpriteEffects.None, 0f);
        }
        if (!string.IsNullOrEmpty(effectLine))
        {
            spriteBatch.DrawString(_font, effectLine, dmgPos + new Vector2(0, 20), effectColor, 0f, Vector2.Zero, msgScale, SpriteEffects.None, 0f);
        }
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

    private void EnsureEnemyCreatureSprite(Pokemon enemyPokemon)
    {
        if (_enemyTrainerString != "trainer-painter") return;
        if (enemyPokemon == null) return;

        // Load creature atlas lazily
        if (_creatureAtlas == null && _content != null)
        {
            try
            {
                _creatureAtlas = TextureAtlas.FromFile(_content, "creature.xml");
            }
            catch { }
        }

        // Force-create animated sprite from creature atlas if missing
        if (enemyPokemon.AnimatedSprite == null && _creatureAtlas != null)
        {
            try
            {
                var anim = _creatureAtlas.GetAnimation("the creature-front");
                enemyPokemon.SetAnimatedSprite(new AnimatedSprite(anim));
            }
            catch { }
        }

        // Last resort: try the shared front factory again (will throw if also missing)
        if (enemyPokemon.AnimatedSprite == null)
        {
            try
            {
                var anim = PokemonFrontFactory.Instance.CreateAnimatedSprite("the creature-front");
                enemyPokemon.SetAnimatedSprite(anim);
            }
            catch { }
        }

        _creatureSpriteMissing = enemyPokemon.AnimatedSprite == null;
    }
}