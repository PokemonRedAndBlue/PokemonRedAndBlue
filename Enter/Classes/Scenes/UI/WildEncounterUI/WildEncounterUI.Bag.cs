using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Enter.Classes.Characters;
using Enter.Classes.Sprites;
using Enter.Classes.Animations;
using PokemonGame;

public partial class WildEncounterUI
{
    private void DrawState_Bag(SpriteBatch spriteBatch, Pokemon currentPokemon)
    {
        // Use the ITEM UI base for bag/item state
        UIBaseSprites[3].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);

        // Draw arrow aligned with menu top-left (Pokeball option)
        Sprite arrowSprite = new Sprite(_WildUIAtlas.GetRegion("horizzontal-arrow"));
        // Align arrow with the same x/y as the Fight slot (more left than before)
        Vector2 arrowPos = BattleUIHelper.MenuTopLeftArrowPosition + new Vector2(-262, 0);
        arrowSprite.Draw(spriteBatch, Color.White, arrowPos, 4f);

        // Draw trainer back and wild pokemon (unless absorbed in animation)
        _trainerSpriteBack.Draw(spriteBatch, Color.White, playerPosition, 8f);
        bool showWild = _captureAnimation == null || !_captureAnimation.CaptureComplete;
        if (showWild)
        {
            _wildPokemonSpriteFront.Draw(spriteBatch, Color.White, wildPokemonPosition, 4f);
        }

        // Draw health bars using current HP so they reflect changes
        battleUI.drawHealthBar(playerCurrentHP, playerMaxHP, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(enemyCurrentHP, enemyMaxHP, greenBar, yellowBar, redBar, spriteBatch, false);

        // Check for Tab key to reset battle (escape from item selection)
        KeyboardState currentState = Keyboard.GetState();

        // Trigger capture animation on Enter if not already running
        if (_captureAnimation == null && !captureInProgress && currentState.IsKeyDown(Keys.Enter))
        {
            BagConfirmRequested = true;
            var pokeTexture = _wildPokemonSpriteFront?.Region?.Texture;
            if (pokeTexture != null)
            {
                var startPos = playerPosition; // throw starts from player
                _captureAnimation = new PokeballCaptureAnimation(wildPokemonPosition, pokeTexture, startPos);
                _captureAnimation.LoadContent(_content);

                // Simple capture odds: higher at lower HP
                double hpRatio = enemyMaxHP > 0 ? (double)enemyCurrentHP / enemyMaxHP : 1.0;
                double captureChance = Math.Clamp(0.8 - (hpRatio * 0.5), 0.25, 0.9);
                _captureAnimation.SetCaptureChance(captureChance);

                _captureAnimation.StartCapture();
                captureInProgress = true;
            }
        }

        // Draw capture animation overlay if active
        if (_captureAnimation != null)
        {
            _captureAnimation.Draw(spriteBatch);
        }
        if (currentState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }

        // Enter triggers bag confirm hook for throw/catch handling
        if (currentState.IsKeyDown(Keys.Enter))
        {
            BagConfirmRequested = true; // external battle logic can process catch/animation here
        }

        _prevBagKeyState = currentState;
    }
}
