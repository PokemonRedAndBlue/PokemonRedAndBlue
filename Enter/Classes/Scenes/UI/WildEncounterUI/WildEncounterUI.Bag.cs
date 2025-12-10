using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Enter.Classes.Characters;
using Enter.Classes.Sprites;
using Enter.Classes.Animations;
using Enter.Classes.Textures;
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
        bool showWild = !ShouldHideWildSprite;
        if (showWild)
        {
            _wildPokemonSpriteFront.Draw(spriteBatch, Color.White, wildPokemonPosition, 4f);
        }

        // Draw health bars using current HP so they reflect changes
        battleUI.drawHealthBar(playerCurrentHP, playerMaxHP, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(enemyCurrentHP, enemyMaxHP, greenBar, yellowBar, redBar, spriteBatch, false);

        // Check for Tab key to reset battle (escape from item selection)
        KeyboardState currentState = Keyboard.GetState();

        // Draw capture animation overlay if active
        if (_captureAnimation != null)
        {
            _captureAnimation.Draw(spriteBatch);
        }
        if (currentState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }
    }
}
