using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using PokemonGame;
using Microsoft.Xna.Framework.Graphics;
using System;
using Enter.Classes.Animations;
using Enter.Classes.Input;
using System.Collections.Generic;

public class BattleUIHelper
{
    // handle arrow locations in menu
    private int[][] arrowLocation =
    {
        new int[] {1,0}, // Top left
        new int[] {0,0}  // Top right
    };
    static private Vector2 topLeft = new Vector2(uiBasePosition.X + (72 * _scale), uiBasePosition.Y + (112 * _scale));
    static private Vector2 topRight = new Vector2(uiBasePosition.X + (120 * _scale), uiBasePosition.Y + (112 * _scale));
    static private Vector2 botLeft = new Vector2(uiBasePosition.X + (72 * _scale), uiBasePosition.Y + (138 * _scale));
    static private Vector2 botRight = new Vector2(uiBasePosition.X + (120 * _scale), uiBasePosition.Y + (138 * _scale));

    static private Dictionary<(int, int), Vector2> numsToArrowLocals = new Dictionary<(int, int), Vector2>
    {
        // The key is just (0, 0) and the entry is { key, value }
        { (0, 0), topLeft },
        { (1, 0), topRight },
        { (0, 1), botLeft },
        { (1, 1), botRight }
    };
    static private float _scale = 4.0f;
    KeyboardController keyBrd = new KeyboardController();
    static private Vector2 uiBasePosition = new Vector2(340, 75);
    private static Vector2 _pokeballStartPosition;
    static private Vector2 playerPokeballStartPosition = new Vector2(uiBasePosition.X + (88 * _scale) - 4, uiBasePosition.Y + (80 * _scale) - 4);
    static private Vector2 enemyPokeballStartPosition = new Vector2(uiBasePosition.X + (24 * _scale) - 4, uiBasePosition.Y + (16 * _scale) - 4);
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

    public void DrawArrow(TextureAtlas battleUIAtlas, SpriteBatch spriteBatch)
    {   
        Sprite arrowSprite = new Sprite(battleUIAtlas.GetRegion("horizzontal-arrow"));

        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                if(arrowLocation[i][j] == 1) // 1. Check if the arrow is at this grid spot
                {
                    // 2. Correctly call TryGetValue:
                    //    It returns a bool (true/false) and
                    //    populates 'drawPosition' with the Vector2 value.
                if (numsToArrowLocals.TryGetValue((i, j), out Vector2 drawPosition))
                {
                    // 3. Now 'drawPosition' holds the Vector2, so we use it here:
                    arrowSprite.Draw(spriteBatch, Color.White, drawPosition, _scale);
                }
            }
        }
    }
}

    public int[][] moveArrow(int[][] currentArrow)
    {
        return null;
    }
}