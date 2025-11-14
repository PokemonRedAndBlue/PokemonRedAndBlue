using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using PokemonGame;
using Microsoft.Xna.Framework.Graphics;
using System;
using Enter.Classes.Animations;

public class BattleUIHelper
{
    static private float _scale = 2.0f;
    static private Vector2 uiBasePosition = new Vector2(350, 75);
    private static Vector2 _pokeballStartPosition;
    static private Vector2 playerPokeballStartPosition = new Vector2(uiBasePosition.X + (88 * _scale), uiBasePosition.Y + (80 * _scale));
    static private Vector2 enemyPokeballStartPosition = new Vector2(uiBasePosition.X + (24 * _scale), uiBasePosition.Y + (16 * _scale));
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

    static public void drawPokeballSprites(Pokemon[] team, TextureAtlas battleUIAtlas, SpriteBatch spriteBatch, Boolean isPlayersTeam)
    {
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
            Sprite pokeballSprite = GetPokeballSprite(team[i].StateMachine.CurrentStateName, battleUIAtlas);
            pokeballSprite.Draw(spriteBatch, Color.White, _pokeballStartPosition + new Vector2(i * 8, 0), _scale);
        }
    }
}