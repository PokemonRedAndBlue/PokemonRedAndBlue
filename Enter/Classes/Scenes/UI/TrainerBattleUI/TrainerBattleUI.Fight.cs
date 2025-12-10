using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Enter.Classes.Sprites;
using PokemonGame;

public partial class TrainerBattleUI
{
    private void DrawState_Fight(SpriteBatch spriteBatch, Pokemon currentPokemon, Pokemon enemyPokemon)
    {
        UIBaseSprites[1].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
        String playersPokemon = currentPokemon.Name.ToString();
        Sprite currentMon = PokemonBackFactory.Instance.CreateStaticSprite(playersPokemon.ToLower() + "-back");
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
            
            //Attack SFX
            SoundEffectPlayer.Play(SfxId.SFX_CYMBAL_3);
            

            // Trigger enemy damage flash
            enemyTakingDamage = true;
            enemyDamageFlashTimer = 0.0;
            
            battleMessage = playerMsg;
            if (enemyCurrentHP <= 0)
            {
                battleMessage = "You win!";
                enemyFainting = true;
                enemyFaintTimer = 0.0;
                endMessageActive = true;
                endMessageTimer = 0.0;
                BackgroundMusicPlayer.Play(SongId.VictoryTrainer, loop: false);
                currentTurn = BattleTurn.End;
            }
            else
            {
                currentTurn = BattleTurn.Waiting;
                turnTimer = 0.0;
            }
        }
        // Use updated HP for health bars (use Pokemon overload like WildEncounterUI)
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(enemyPokemon, greenBar, yellowBar, redBar, spriteBatch, false);
        // draw after HP bars so its on top of gaint gray bars
        Vector2 playerOffsetFight = Vector2.Zero;
        if (playerAttackAnimationPlaying)
        {
            playerOffsetFight = backState.AttackBackAction(currentMon, playerAttackAnimationTimer, AttackAnimationDurationMs);
        }
        // Apply red tint if taking damage
        Color playerColorFight = playerTakingDamage ? Color.Red : Color.White;
        float playerScaleFight = 1.0f;
        if (playerFainting)
        {
            var (faintColor, faintScale) = faintState.GetFaintEffect(playerFaintTimer, FaintAnimationDurationMs);
            playerColorFight = faintColor;
            playerScaleFight = faintScale;
        }
        currentMon.Draw(spriteBatch, playerColorFight, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)) + playerOffsetFight, 4f * playerScaleFight);
        // Draw HP just above player pokemon
        DrawHP(spriteBatch, playerCurrentHP, playerMaxHP, new Vector2(playerPosition.X + 20, maxDrawPos.Y - (currentMon.Height * _scale) - 20), "Player");
        
        Vector2 enemyOffsetFight = Vector2.Zero;
        if (enemyAttackAnimationPlaying)
        {
            enemyOffsetFight = frontState.AttackFrontAction(enemyPokemon.AnimatedSprite, enemyAttackAnimationTimer, AttackAnimationDurationMs);
        }
        // Apply red tint if taking damage
        Color enemyColorFight = enemyTakingDamage ? Color.Red : Color.White;
        float enemyScaleFight = 1.0f;
        if (enemyFainting)
        {
            var (faintColor, faintScale) = faintState.GetFaintEffect(enemyFaintTimer, FaintAnimationDurationMs);
            enemyColorFight = faintColor;
            enemyScaleFight = faintScale;
        }
        float enemyBaseScaleFight = 4f;
        if (_enemyTrainerString == "trainer-painter")
        {
            enemyBaseScaleFight = 0.2f;
        }
        enemyPokemon.AnimatedSprite?.Draw(spriteBatch, enemyColorFight, enemysPokemonPosition + enemyOffsetFight, enemyBaseScaleFight * enemyScaleFight);
        // Draw HP just above enemy pokemon (25px lower)
        DrawHP(spriteBatch, enemyCurrentHP, enemyMaxHP, new Vector2(enemysPokemonPosition.X + 20, enemysPokemonPosition.Y - 15), "Enemy");
        if (!string.IsNullOrEmpty(battleMessage))
        {
            DrawMessage(spriteBatch, battleMessage);
        }
        // Show fight instructions
        DrawMessage(spriteBatch, "Press A to use Tackle");
    }
}
