using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Enter.Classes.Characters;
using Enter.Classes.Sprites;
using PokemonGame;

public partial class WildEncounterUI
{
    private void DrawState_Pokemon(SpriteBatch spriteBatch, Pokemon currentPokemon)
    {
        // Draw base UI for pokemon switching state
        UIBaseSprites[4].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);

        // Check for Tab key to reset battle (escape from pokemon selection)
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }
    }
}
