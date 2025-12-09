using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PokemonGame;
using Enter.Classes.Sprites;

public partial class TrainerBattleUI
{
    private void DrawState_Item(SpriteBatch spriteBatch, Pokemon currentPokemon, Pokemon enemyPokemon)
    {
        // Use the ITEM UI base for item scene
        UIBaseSprites[3].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);

        // Draw a fixed arrow next to the Pokeball word (same slot as menu top-left)
        Sprite arrowSprite = new Sprite(_TrainerUIAtlas.GetRegion("horizzontal-arrow"));
        // Align arrow with the same x/y as the Fight slot (more left than before)
        Vector2 arrowPos = BattleUIHelper.MenuTopLeftArrowPosition + new Vector2(-262, 0);
        arrowSprite.Draw(spriteBatch, Color.White, arrowPos, 4f);

        // Draw trainer back and enemy pokemon for context
        Sprite currentMon = PokemonBackFactory.Instance.CreateStaticSprite(currentPokemon.Name.ToLower() + "-back");
        currentMon.Draw(spriteBatch, Color.White, new Vector2(playerPosition.X, maxDrawPos.Y + (-currentMon.Height * _scale)), 4f);
        enemyPokemon.AnimatedSprite?.Draw(spriteBatch, Color.White, enemysPokemonPosition, _enemyTrainerString == "trainer-painter" ? 0.2f : 4f);

        // Draw health bars
        battleUI.drawHealthBar(currentPokemon, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(enemyPokemon, greenBar, yellowBar, redBar, spriteBatch, false);

        // Input: Tab exits, Enter requests pokeball use (hook for animation/catch)
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }

        if (keyState.IsKeyDown(Keys.Enter) && _prevItemKeyState.IsKeyUp(Keys.Enter))
        {
            ItemConfirmRequested = true; // external code can hook to throw/catch from this flag
        }
        else if (_currentState != "Item")
        {
            ItemConfirmRequested = false;
        }

        _prevItemKeyState = keyState;
    }
}
