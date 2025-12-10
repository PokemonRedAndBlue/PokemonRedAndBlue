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
        if (_playerDeploying)
        {
            _playerDeployThrow?.Draw(spriteBatch);
        }
        else
        {
            currentMon.Draw(spriteBatch, Color.White, GetPlayerMonDrawPosWithOffset(currentMon), PlayerBackScaleDraw);
        }
        enemyPokemon.AnimatedSprite?.Draw(spriteBatch, Color.White, enemysPokemonPosition, _enemyTrainerString == "trainer-painter" ? 0.2f : 4f);

        // Draw health bars using tracked HP values
        battleUI.drawHealthBar(playerCurrentHP, playerMaxHP, greenBar, yellowBar, redBar, spriteBatch, true);
        battleUI.drawHealthBar(enemyCurrentHP, enemyMaxHP, greenBar, yellowBar, redBar, spriteBatch, false);

        // Input: Tab exits, Enter requests pokeball use (hook for animation/catch)
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.Tab))
        {
            resetBattle = true;
        }

        if (keyState.IsKeyDown(Keys.Enter))
        {
            ItemConfirmRequested = true; // external code can hook to throw/catch from this flag
        }

        if (ItemConfirmRequested){
            // throw pokeball

            
            // trigger pokemon being sucked up


            // play ball wobble animation and sfx


            // give catch result (for now, always succeed)


            // add pokemon to player's party/storage


            // finally set flag to indicate run/catch occurred
            didRunOrCatch = true;
        }

        _prevItemKeyState = keyState;
    }
}
