using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Characters;
using Enter.Classes.Sprites;
using PokemonGame;

public partial class WildEncounterUI
{
    private void DrawState_Menu(SpriteBatch spriteBatch, Pokemon currentPokemon)
    {
        // Draw base UI for menu state
        UIBaseSprites[1].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);

        // Draw both health bars
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, false);

        // Arrow handling logic
        battleUI.DrawArrow(_WildUIAtlas, spriteBatch);
        battleUI.moveArrow();

        // Draw pokémon last (on top) with attack offsets
        string playersPokemon = currentPokemon.Name.ToString();
        Sprite currentMon = PokemonBackFactory.Instance.CreateStaticSprite(playersPokemon.ToLower() + "-back");
        Vector2 playerOffsetMenu = Vector2.Zero;
        if (playerAttackAnimationPlaying)
        {
            playerOffsetMenu = backState.AttackBackAction(currentMon, playerAttackAnimationTimer, AttackAnimationDurationMs);
        }
        currentMon.Draw(spriteBatch, Color.White, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)) + playerOffsetMenu, 4f);

        // Draw wild pokemon sprite with potential attack offset
        Vector2 wildOffset = Vector2.Zero;
        if (enemyAttackAnimationPlaying)
        {
            wildOffset = frontState.AttackFrontAction(_wildPokemonSpriteFront, enemyAttackAnimationTimer, AttackAnimationDurationMs);
        }
        _wildPokemonSpriteFront.Draw(spriteBatch, Color.White, wildPokemonPosition + wildOffset, 4f);
    }
}
