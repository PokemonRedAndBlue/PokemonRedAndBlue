using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using PokemonGame;
using Microsoft.Xna.Framework.Graphics;

public class BattleUIHelper
{
    static private float _scale = 2.0f;
    static public Sprite GetPokeballSprite(string status, TextureAtlas battleUIAtlas)
    {
        switch (status)
        {
            case "alive":
                return new Sprite(battleUIAtlas.GetRegion("pokeball-present"));
            case "dead":
                return new Sprite(battleUIAtlas.GetRegion("pokeball-dead"));
            default:
                return new Sprite(battleUIAtlas.GetRegion("pokeball-missing"));
        }
    }

    static public void drawPokeballSprites(Pokemon[] team, TextureAtlas battleUIAtlas, SpriteBatch spriteBatch)
    {
        Vector2 pokeballStartPosition = new Vector2(50, 400);
        for(int i = 0; i < 6; i++)
        {
            Sprite pokeballSprite = GetPokeballSprite(team[i].StateMachine.CurrentStateName, battleUIAtlas);
            pokeballSprite.Draw(spriteBatch, Color.White, pokeballStartPosition + new Vector2(i * 20, 0), _scale);
        }
    }
}