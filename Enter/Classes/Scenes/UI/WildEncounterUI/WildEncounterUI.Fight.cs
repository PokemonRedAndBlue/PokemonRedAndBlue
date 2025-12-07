using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Enter.Classes.Characters;
using Enter.Classes.Sprites;
using PokemonGame;

public partial class WildEncounterUI
{
    private void DrawState_Fight(SpriteBatch spriteBatch, Pokemon currentPokemon)
    {
        // Draw base UI for fight state
        UIBaseSprites[2].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);

        // Draw both health bars
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, false);

        // Check for Tab key to reset battle (escape from fight)
        KeyboardState keysState = Keyboard.GetState();
        if (keysState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }
    }
}
