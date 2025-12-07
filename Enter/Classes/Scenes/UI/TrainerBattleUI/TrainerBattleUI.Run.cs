using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public partial class TrainerBattleUI
{
    private void DrawState_Run(SpriteBatch spriteBatch, Pokemon currentPokemon, Pokemon enemyPokemon)
    {
        // Run state: simply set reset and return to menu
        resetBattle = true;
    }
}
