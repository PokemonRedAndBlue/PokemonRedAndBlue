using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public partial class TrainerBattleUI
{
    private void DrawState_Initial(SpriteBatch spriteBatch, Pokemon currentPokemon, Pokemon enemyPokemon)
    {
        UIBaseSprites[0].Draw(spriteBatch, Color.White, new Vector2(340, 75), 4f);
        _trainerSpriteBack.Draw(spriteBatch, Color.White, playerTrainerPosition, 8f);
        _enemyTrainerSpriteFront.Draw(spriteBatch, Color.White, enemyTrainerPosition, 4f);
        _enemyTrainerIDSprite.DrawTextSpriteWithScale(spriteBatch, enemyTrainerIDPosition, 2f);
        BattleUIHelper.drawPokeballSprites(_playerTeam, _TrainerUIAtlas, spriteBatch, true);
        BattleUIHelper.drawPokeballSprites(_enemyTeam, _TrainerUIAtlas, spriteBatch, false);
    }
}
