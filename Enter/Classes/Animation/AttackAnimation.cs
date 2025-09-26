using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class AttackAnimation : AnimationKernel
    {
        public PokemonState.SpriteState UpdateAttackAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
        {
            if (!stateIsTrue)
                return PokemonState.SpriteState.Idle;

            // Forward for first 8 frames, then back
            if (AnimationTime <= 8)
                CurrentPosition = startPosition + new Vector2(15, 0);
            else
                CurrentPosition = startPosition - new Vector2(15, 0);

            // clamp to x >= 100
            if (CurrentPosition.X < 100)
                CurrentPosition = new Vector2(100, CurrentPosition.Y);

            AnimationTime++;

            if (AnimationTime >= AnimationDuration)
            {
                EndAnimation();
                return PokemonState.SpriteState.Idle;
            }

            Draw(sprite, spriteBatch, Color.White, 4f);
            return PokemonState.SpriteState.Attack;
        }
    }
}
