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
        // Draw base UI for bag/item state
        UIBaseSprites[3].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);

        // Only draw opponent health
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, false);

        // Check for Tab key to reset battle (escape from item selection)
        KeyboardState currentState = Keyboard.GetState();
        if (currentState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }
    }
}
