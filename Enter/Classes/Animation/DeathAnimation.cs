using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class DeathAnimation : AnimationKernel
    {
        private bool isDying = false;
        private int deathAnimationTime = 0;
        private const int DeathDuration = 20;
        private Color color = Color.White;

        // Update the animation state; returns the current sprite state
        public PokemonState.SpriteState UpdateDeathAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
        {
            if (!isDying)
                return PokemonState.SpriteState.Idle;

            // Death Animation Code Here
            if (CurrentPosition.X % 2 == 0)
            {
                color = Color.Red;
                CurrentPosition = startPosition + new Vector2(2, 0);
            }
            else
            {
                color = Color.White;
                CurrentPosition = startPosition - new Vector2 (2, 0);
            }

            deathAnimationTime++;

            // End animation if duration exceeded
            if (deathAnimationTime >= DeathDuration)
            {
                EndAnimation();
                return PokemonState.SpriteState.Idle;
            }

            Draw(sprite, spriteBatch, color);
            return PokemonState.SpriteState.Death;
        }
    }
}
