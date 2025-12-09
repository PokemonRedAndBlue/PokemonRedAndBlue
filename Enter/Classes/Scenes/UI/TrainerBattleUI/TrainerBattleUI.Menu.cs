using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Enter.Classes.Sprites;
using PokemonGame;

public partial class TrainerBattleUI
{
    private void DrawState_Menu(SpriteBatch spriteBatch, Pokemon currentPokemon, Pokemon enemyPokemon)
    {
        UIBaseSprites[1].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
        String playersPokemon = currentPokemon.Name.ToString();
        Sprite currentMon = PokemonBackFactory.Instance.CreateStaticSprite(playersPokemon.ToLower() + "-back");
        // Use updated HP for health bars (use Pokemon overload like WildEncounterUI)
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(enemyPokemon, greenBar, yellowBar, redBar, spriteBatch, false);
        if (!string.IsNullOrEmpty(battleMessage))
        {
            DrawMessage(spriteBatch, battleMessage);
        }
        // Show menu navigation instructions
        DrawMessage(spriteBatch, "Use arrow keys to navigate and Enter to select");
        // Enable menu navigation
        battleUI.moveArrow();
        battleUI.DrawArrow(_TrainerUIAtlas, spriteBatch);
        // Draw pokémon last (on top) with attack offsets
        Vector2 playerOffsetMenu = Vector2.Zero;
        if (playerAttackAnimationPlaying)
        {
            playerOffsetMenu = backState.AttackBackAction(currentMon, playerAttackAnimationTimer, AttackAnimationDurationMs);
        }
        // Apply red tint if taking damage
        Color playerColorMenu = playerTakingDamage ? Color.Red : Color.White;
        float playerScaleMenu = 1.0f;
        if (playerFainting)
        {
            var (faintColor, faintScale) = faintState.GetFaintEffect(playerFaintTimer, FaintAnimationDurationMs);
            playerColorMenu = faintColor;
            playerScaleMenu = faintScale;
        }
        currentMon.Draw(spriteBatch, playerColorMenu, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)) + playerOffsetMenu, 4f * playerScaleMenu);
        // Draw HP just above player pokemon
        DrawHP(spriteBatch, playerCurrentHP, playerMaxHP, new Vector2(playerPosition.X + 20, maxDrawPos.Y - (currentMon.Height * _scale) - 20), "Player");
        
        Vector2 enemyOffsetMenu = Vector2.Zero;
        if (enemyAttackAnimationPlaying)
        {
            enemyOffsetMenu = frontState.AttackFrontAction(enemyPokemon.AnimatedSprite, enemyAttackAnimationTimer, AttackAnimationDurationMs);
        }
        // Apply red tint if taking damage
        Color enemyColorMenu = enemyTakingDamage ? Color.Red : Color.White;
        float enemyScaleMenu = 1.0f;
        if (enemyFainting)
        {
            var (faintColor, faintScale) = faintState.GetFaintEffect(enemyFaintTimer, FaintAnimationDurationMs);
            enemyColorMenu = faintColor;
            enemyScaleMenu = faintScale;
        }
        float enemyBaseScaleMenu = 4f;
        // Creature atlas art is huge; shrink it so it stays on-screen
        if (_enemyTrainerString == "trainer-painter")
        {
            enemyBaseScaleMenu = 0.2f;
        }
        enemyPokemon.AnimatedSprite?.Draw(spriteBatch, enemyColorMenu, enemysPokemonPosition + enemyOffsetMenu, enemyBaseScaleMenu * enemyScaleMenu);
        // Draw HP just above enemy pokemon (25px lower)
        DrawHP(spriteBatch, enemyCurrentHP, enemyMaxHP, new Vector2(enemysPokemonPosition.X + 20, enemysPokemonPosition.Y - 15), "Enemy");
    }
}
