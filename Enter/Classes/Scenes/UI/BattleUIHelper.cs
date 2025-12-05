// ...using statements...

using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using PokemonGame;
using Microsoft.Xna.Framework.Graphics;
using System;
using Enter.Classes.Animations;
using Enter.Classes.Input;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Runtime.ConstrainedExecution;

public class BattleUIHelper
{
    // Overload: draw health bar with explicit HP/maxHP values
    public void drawHealthBar(int currentHp, int maxHp, Sprite greenMax, Sprite midYellow, Sprite redLow, SpriteBatch spriteBatch, bool isPlayersTeam)
    {
        double topThird = maxHp * (2.0 / 3.0);
        double midThird = maxHp * (1.0 / 3.0);
        Vector2 barPos = isPlayersTeam
            ? new Vector2(uiBasePosition.X + (_scale * 95) - 4, uiBasePosition.Y + (_scale * 74) - 4)
            : new Vector2(uiBasePosition.X + (31 * _scale) - 4, uiBasePosition.Y + (18 * _scale) - 4);

        if (currentHp > topThird)
        {
            greenMax.Draw(spriteBatch, Color.White, barPos, _scale);
        }
        else if (currentHp > midThird)
        {
            midYellow.Draw(spriteBatch, Color.White, barPos, _scale);
        }
        else
        {
            redLow.Draw(spriteBatch, Color.White, barPos, _scale);
        }
    }
    static private float _scale = 4.0f;
    private double _stateTimer;
    private String currentBattleState = "Initial";
    KeyboardController keyBrd = new KeyboardController();
    static private Vector2 uiBasePosition = new Vector2(340, 75);
    private KeyboardState previousKeyState;

    // handle arrow locations in menu
    private int[][] arrowLocation =
    {
        new int[] {1,0}, // Top left
        new int[] {0,0}  // Top right
    };
    static private Vector2 topLeft = new Vector2(uiBasePosition.X + (72 * _scale) - 5, uiBasePosition.Y + (112 * _scale) - 5);
    static private Vector2 topRight = new Vector2(uiBasePosition.X + (120 * _scale) - 5, uiBasePosition.Y + (112 * _scale) - 5);
    static private Vector2 botLeft = new Vector2(uiBasePosition.X + (72 * _scale) - 5, uiBasePosition.Y + (128 * _scale) - 5);
    static private Vector2 botRight = new Vector2(uiBasePosition.X + (120 * _scale) - 5, uiBasePosition.Y + (128 * _scale) - 5);
    static private Dictionary<(int, int), Vector2> numsToArrowLocals = new Dictionary<(int, int), Vector2>
    {
        // The key is just (0, 0) and the entry is { key, value }
        { (0, 0), topLeft },
        { (1, 0), topRight },
        { (0, 1), botLeft },
        { (1, 1), botRight }
    };
    private static Vector2 _pokeballStartPosition;
    private static Vector2 _drawPostionArrow;
    static private Vector2 playerPokeballStartPosition = new Vector2(uiBasePosition.X + (88 * _scale) - 4, uiBasePosition.Y + (80 * _scale) - 4);
    static private Vector2 enemyPokeballStartPosition = new Vector2(uiBasePosition.X + (24 * _scale) - 4, uiBasePosition.Y + (16 * _scale) - 4);
    static private Vector2 healthBarPosition = new Vector2(uiBasePosition.X + (31 * _scale) - 4, uiBasePosition.Y + (18 * _scale) - 4);
    static public Sprite GetPokeballSprite(string status, TextureAtlas battleUIAtlas)
    {
        switch (status)
        {
            case "idle":
                return new Sprite(battleUIAtlas.GetRegion("pokeball-present"));
            case "dead":
                return new Sprite(battleUIAtlas.GetRegion("pokeball-dead"));
            default:
                return new Sprite(battleUIAtlas.GetRegion("pokeball-missing"));
        }
    }

    static public void drawPokeballSprites(Team team, TextureAtlas battleUIAtlas, SpriteBatch spriteBatch, Boolean isPlayersTeam)
    {
        Sprite pokeballSprite;
        if(isPlayersTeam)
        {
            _pokeballStartPosition = playerPokeballStartPosition;
        }
        else
        {
            _pokeballStartPosition = enemyPokeballStartPosition;
        }
        for(int i = 0; i < 6; i++)
        {
            Pokemon currentPokemon = team.Pokemons[i];
            if (currentPokemon.Name.Equals("I am just a placeholder"))
            {
                pokeballSprite = GetPokeballSprite("default", battleUIAtlas);
            } else
            {
                pokeballSprite = GetPokeballSprite(currentPokemon.StateMachine.CurrentStateName, battleUIAtlas);
            }
            
            pokeballSprite.Draw(spriteBatch, Color.White, _pokeballStartPosition + new Vector2(i * 8 * _scale, 0), _scale);
        }
    }

public void moveArrow()
{
    KeyboardState currentState = Keyboard.GetState();

    // 1. Find the current position of the arrow
    int currentRow = 0;
    int currentCol = 0;
    
    // This loop is fine, but can be made more efficient
    for (int i = 0; i < 2; i++)
    {
        for (int j = 0; j < 2; j++)
        {
            if (arrowLocation[i][j] == 1)
            {
                currentRow = i;
                currentCol = j;
                i = 2; // A better way to break both loops
                break;
            }
        }
    }

    int newRow = currentRow;
    int newCol = currentCol;

    // 2. Check for a *single key press*
    //    (Is down NOW, but was UP last frame)
    if (currentState.IsKeyDown(Keys.Left) && previousKeyState.IsKeyUp(Keys.Left))
    {
        newRow = (currentRow - 1 + 2) % 2; 
        SoundEffectPlayer.Play(SfxId.SFX_PRESS_AB); 
    }
    else if (currentState.IsKeyDown(Keys.Right) && previousKeyState.IsKeyUp(Keys.Right))
    {
        newRow = (currentRow + 1) % 2;
        SoundEffectPlayer.Play(SfxId.SFX_PRESS_AB); 
    }
    else if (currentState.IsKeyDown(Keys.Up) && previousKeyState.IsKeyUp(Keys.Up))
    {
        newCol = (currentCol - 1 + 2) % 2;
        SoundEffectPlayer.Play(SfxId.SFX_PRESS_AB); 
    }
    else if (currentState.IsKeyDown(Keys.Down) && previousKeyState.IsKeyUp(Keys.Down))
    {
        newCol = (currentCol + 1) % 2;
        SoundEffectPlayer.Play(SfxId.SFX_PRESS_AB); 
    }

    // handle arrow selection
        if (currentState.IsKeyDown(Keys.Enter))
        {
            currentBattleState = handleArrowEvent(newCol, newRow);
            return;
        }

    // 3. If the position changed, update the array
    if (newRow != currentRow || newCol != currentCol)
    {
        arrowLocation[currentRow][currentCol] = 0; // Clear the old spot
        arrowLocation[newRow][newCol] = 1;       // Set the new spot
    }

    // 4. VERY IMPORTANT: Save the current state for the next frame
    previousKeyState = currentState;
}

public void DrawArrow(TextureAtlas battleUIAtlas, SpriteBatch spriteBatch)
{ 
    // This creates a new sprite object every frame. See optimization tip below.
    Sprite arrowSprite = new Sprite(battleUIAtlas.GetRegion("horizzontal-arrow"));

    for(int i = 0; i < 2; i++)
    {
        for(int j = 0; j < 2; j++)
        {
            // Check the grid spot at [i][j]
            if(arrowLocation[i][j] == 1) 
            {
                // Use (i, j) directly as the key
                _drawPostionArrow = numsToArrowLocals[(i, j)];
                arrowSprite.Draw(spriteBatch, Color.White, _drawPostionArrow, _scale);
            }
        }
    }
}

public String handleArrowEvent(int currentCol, int currentRow)
    {
        if(currentCol == 0 && currentRow == 0)
        {
            return "Fight";
        } else if(currentCol == 1 && currentRow == 0)
        {
            return "Item";
        } else if(currentCol == 0 && currentRow == 1){
            return "PkMn";
        } else if(currentCol == 1 && currentRow == 1)
        {
            return "Run";
        }
        return "";
    }

    public String getBattleState()
    {
        return this.currentBattleState;
    }

    public void setBattleState(String newState)
    {
        this.currentBattleState = newState;
    }

    public void Update(GameTime gameTime)
    {
        double TIME_TO_WAIT_MS = 4000;

        // Check the current state
        if (currentBattleState == "Initial")
        {
            // We are in the "Initial" state, so run the timer
            _stateTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check if 5 seconds (5000ms) have passed
            if (_stateTimer >= TIME_TO_WAIT_MS)
            {
                currentBattleState = "Menu"; // Transition the state
                _stateTimer = 0.0;           // Reset timer for future use
            }
        }
    }

    public void drawHealthBar(Pokemon poke, Sprite greenMax, Sprite midYellow, Sprite redLow, SpriteBatch spriteBatch, Boolean isPlayersTeam)
    {
        // split max health into 3 sections
        double topThird = poke.MaxHp * (2.0 / 3.0);
        double midThird = poke.MaxHp * (1.0 / 3.0);
        double currentHp = poke.Hp;

        if(isPlayersTeam)
        {
            healthBarPosition = new Vector2( uiBasePosition.X + (_scale * 95) - 4, uiBasePosition.Y + (_scale * 74) - 4);
        }

        if(currentHp > topThird)
        {
            // draw green
            greenMax.Draw(spriteBatch, Color.White, healthBarPosition, _scale);
        } else if(currentHp > midThird)
        {
            // draw yellow
            midYellow.Draw(spriteBatch, Color.White, healthBarPosition, _scale);
        } else if(midThird > currentHp)
        {
            // draw red
            redLow.Draw(spriteBatch, Color.White, healthBarPosition, _scale);
        }
    }
}