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

    private int playerCurrentHP = 0;
    private int enemyCurrentHP = 0;
    private int playerMaxHP = 0;
    private int enemyMaxHP = 0;
    private bool battleInitialized = false;
    private string battleMessage = "";
    private bool endMessageActive = false;
    private double endMessageTimer = 0.0;
    private const double EndMessagePauseMs = 4000.0;

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
                int enemyDmg = 8;
                playerCurrentHP -= enemyDmg;
                if (playerCurrentHP < 0) playerCurrentHP = 0;
                battleMessage = $"Enemy attacks! Player loses {enemyDmg} HP.";
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
        spriteBatch.DrawString(_font, message, msgPos, color);
    }

    private void DrawHealthBarSprite(SpriteBatch spriteBatch, Vector2 pos, int currentHP, int maxHP, Sprite greenBar, Sprite yellowBar, Sprite redBar)
    {
        float percent = (float)currentHP / maxHP;
        Sprite bar = greenBar;
        if (percent <= 0.5f && percent > 0.2f)
            bar = yellowBar;
        else if (percent <= 0.2f)
            bar = redBar;
        // Use scale to represent HP percentage (assuming 1f is full bar)
        bar.Draw(spriteBatch, Color.White, pos, percent);
        // Draw border (full length, semi-transparent black)
        greenBar.Draw(spriteBatch, Color.Black * 0.5f, pos, 1f);
    }

    public void WildEncounterStateBasedDraw(Sprite[] UI_BaseSprites, SpriteBatch spriteBatch)
    {
        // always draw border
        _border.Draw(spriteBatch, Color.White, _borderPostion, 4f);

        // always get players pokemon
        Pokemon currentPokemon = _playerTeam.Pokemons[0];
        Pokemon enemyPokemon = _enemyTeam.Pokemons[0];
        _enemyPokemonSpriteFront = PokemonFrontFactory.Instance.CreateAnimatedSprite(enemyPokemon.Name.ToString().ToLower() + "-front");

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
                currentMon.Draw(spriteBatch, Color.White, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)), 4f);
                _enemyPokemonSpriteFront.Draw(spriteBatch, Color.White, enemysPokemonPosition, 4f);
                // Use updated HP for health bars
                DrawHealthBarSprite(spriteBatch, new Vector2(340, 210), playerCurrentHP, playerMaxHP, greenBar, yellowBar, redBar);
                DrawHealthBarSprite(spriteBatch, new Vector2(700, 90), enemyCurrentHP, enemyMaxHP, greenBar, yellowBar, redBar);
                DrawHP(spriteBatch, playerCurrentHP, playerMaxHP, new Vector2(340, 220), "Player HP");
                DrawHP(spriteBatch, enemyCurrentHP, enemyMaxHP, new Vector2(700, 100), "Enemy HP");
                if (!string.IsNullOrEmpty(battleMessage))
                {
                    DrawMessage(spriteBatch, battleMessage);
                }
                // Enable menu navigation
                battleUI.moveArrow();
                battleUI.DrawArrow(_TrainerUIAtlas, spriteBatch);
                break;
            case "Fight":
                UIBaseSprites[1].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                playersPokemon = currentPokemon.Name.ToString();
                currentMon = PokemonBackFactory.Instance.CreateStaticSprite(playersPokemon.ToLower() + "-back");
                currentMon.Draw(spriteBatch, Color.White, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)), 4f);
                _enemyPokemonSpriteFront.Draw(spriteBatch, Color.White, enemysPokemonPosition, 4f);
                if (!endMessageActive && currentTurn == BattleTurn.Player && Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    // Player attacks enemy (reduced damage)
                    int playerDmg = 4; // reduced damage
                    enemyCurrentHP -= playerDmg;
                    if (enemyCurrentHP < 0) enemyCurrentHP = 0;
                    battleMessage = $"Player attacks! Enemy loses {playerDmg} HP.";
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
                DrawHealthBarSprite(spriteBatch, new Vector2(340, 210), playerCurrentHP, playerMaxHP, greenBar, yellowBar, redBar);
                DrawHealthBarSprite(spriteBatch, new Vector2(700, 90), enemyCurrentHP, enemyMaxHP, greenBar, yellowBar, redBar);
                DrawHP(spriteBatch, playerCurrentHP, playerMaxHP, new Vector2(340, 220), "Player HP");
                DrawHP(spriteBatch, enemyCurrentHP, enemyMaxHP, new Vector2(700, 100), "Enemy HP");
                if (!string.IsNullOrEmpty(battleMessage))
                {
                    DrawMessage(spriteBatch, battleMessage);
                }
                break;
            case "Item": // Bag
                UIBaseSprites[4].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
                break;
            case "Run": // Run (blocked in trainer battles)
                // Show a message or just ignore; here, just ignore and return to menu
                resetBattle = true;
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