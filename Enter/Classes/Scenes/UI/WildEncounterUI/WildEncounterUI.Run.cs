using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Enter.Classes.Characters;
using Enter.Classes.Sprites;
using PokemonGame;

public partial class WildEncounterUI
{
    private void DrawState_Run(SpriteBatch spriteBatch, Pokemon currentPokemon)
    {
        // Draw base UI for run state
        UIBaseSprites[5].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);

        // Set flag to end the wild encounter
        didRunOrCatch = true;
    }
}
