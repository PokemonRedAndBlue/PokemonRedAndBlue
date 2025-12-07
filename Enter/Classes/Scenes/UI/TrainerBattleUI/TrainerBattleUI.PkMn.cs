using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PokemonGame;

public partial class TrainerBattleUI
{
    private void DrawState_PkMn(SpriteBatch spriteBatch, Pokemon currentPokemon, Pokemon enemyPokemon)
    {
        KeyboardState keyStates = Keyboard.GetState();
        UIBaseSprites[4].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
        if (keyStates.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }
    }
}
