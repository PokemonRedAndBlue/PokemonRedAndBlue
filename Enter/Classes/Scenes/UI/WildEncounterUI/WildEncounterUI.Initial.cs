using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Characters;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using Enter.Classes.Animations;
using PokemonGame;

public partial class WildEncounterUI
{
    private void DrawState_Initial(SpriteBatch spriteBatch, Pokemon currentPokemon)
    {
        // Draw base UI for initial state
        UIBaseSprites[0].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);

        // Draw "Wild ___ appeared!" messages
        _wildPokemonMessage1.DrawTextSpriteWithScale(spriteBatch, _wildPokemonMessagePos1, 2f);
        _wildPokemonMessage2.DrawTextSpriteWithScale(spriteBatch, _wildPokemonMessagePos2, 2f);

        // Draw player trainer sprite
        _trainerSpriteBack.Draw(spriteBatch, Color.White, playerPosition, 8f);

        // Draw wild pokemon sprite (apply attack offset if active)
        Vector2 wildOffsetInit = Vector2.Zero;
        if (enemyAttackAnimationPlaying)
        {
            wildOffsetInit = frontState.AttackFrontAction(_wildPokemonSpriteFront, enemyAttackAnimationTimer, AttackAnimationDurationMs);
        }
        if (!HideWildInState(_currentState))
        {
            _wildPokemonSpriteFront.Draw(spriteBatch, Color.White, wildPokemonPosition + wildOffsetInit, 4f);
        }

        // Draw player trainer party bar
        BattleUIHelper.drawPokeballSprites(_Player.thePlayersTeam, _WildUIAtlas, spriteBatch, true);
    }
}
