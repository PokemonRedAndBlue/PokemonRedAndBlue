using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Enter.Classes.Characters;
using Enter.Classes.Sprites;
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

        // Draw trainer back and wild pokemon
        _trainerSpriteBack.Draw(spriteBatch, Color.White, playerPosition, 8f);
        _wildPokemonSpriteFront.Draw(spriteBatch, Color.White, wildPokemonPosition, 4f);

        // Draw health bars using current HP so they reflect changes
        battleUI.drawHealthBar(playerCurrentHP, playerMaxHP, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(enemyCurrentHP, enemyMaxHP, greenBar, yellowBar, redBar, spriteBatch, false);

        // Check for Tab key to reset battle (escape from item selection)
        KeyboardState currentState = Keyboard.GetState();
        if (currentState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }

        // Enter triggers bag confirm hook for throw/catch handling
        if (currentState.IsKeyDown(Keys.Enter))
        {
            BagConfirmRequested = true; // external battle logic can process catch/animation here
        }

        if (BagConfirmRequested){
            // throw pokeball

            
            // trigger pokemon being sucked up


            // play ball wobble animation and sfx


            // give catch result (for now, always succeed)


            // add pokemon to player's party/storage


            // finally set flag to indicate run/catch occurred
            didRunOrCatch = true;
        }

        _prevBagKeyState = currentState;
    }
}
