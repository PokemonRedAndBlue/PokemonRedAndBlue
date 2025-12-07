using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PokemonGame;

public partial class TrainerBattleUI
{
    private void DrawState_Item(SpriteBatch spriteBatch, Pokemon currentPokemon, Pokemon enemyPokemon)
    {
        UIBaseSprites[4].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
        // only draw opponent health
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, false);
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }
    }
}
