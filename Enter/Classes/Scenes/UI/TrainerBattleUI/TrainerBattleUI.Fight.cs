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
            var playerMove = _playerMove ?? SafeDefaultMove();
            string playerMsg;
            ApplyAttack(currentPokemon, playerMove, ref enemyCurrentHP, enemyPokemon?.Name.ToString() ?? "Enemy", out playerMsg);
            
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
        var playerHpPos = new Vector2(uiBasePosition.X + (_scale * 95) - 4, uiBasePosition.Y + (_scale * 74) - 4 - 25);
        DrawHP(spriteBatch, playerCurrentHP, playerMaxHP, currentPokemon?.Level ?? 1, playerHpPos, "Player");
        
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
        var enemyHpPos = new Vector2(uiBasePosition.X + (31 * _scale) - 4, uiBasePosition.Y + (18 * _scale) - 4 - 20);
        DrawHP(spriteBatch, enemyCurrentHP, enemyMaxHP, enemyPokemon?.Level ?? 1, enemyHpPos, "Enemy");
        if (!string.IsNullOrEmpty(battleMessage))
        {
            DrawMessage(spriteBatch, battleMessage);
        }
        // Show fight instructions
        var moves = _playerTeam?.Pokemons?[0]?.Moves;
        if (moves != null && moves.Count > 0)
        {
            _playerMoveIndex = Math.Clamp(_playerMoveIndex, 0, moves.Count - 1);
            _playerMove = moves[_playerMoveIndex];
        }

        string moveName = (_playerMove ?? SafeDefaultMove()).Name;
        DrawMessage(spriteBatch, "press A to use " + moveName);
    }
}
