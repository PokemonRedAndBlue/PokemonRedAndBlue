using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PokemonGame;

public partial class TrainerBattleUI
{
    private void DrawState_Run(SpriteBatch spriteBatch, Pokemon currentPokemon, Pokemon enemyPokemon)
    {
        UIBaseSprites[5].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
        // Run state: simply set reset and return to menu
        resetBattle = true;
    }
}
