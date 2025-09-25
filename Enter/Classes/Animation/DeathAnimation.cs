using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class DeathAnimation : AnimationKernel
    {
        private bool isDying = false;
        private int deathAnimationTime = AnimationDuration;
        private const int DeathDuration = 20;

        // Update the animation state; returns the current sprite state
        public PokemonState.SpriteState UpdateDeathAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
        {
            if (!isDying)
                return PokemonState.SpriteState.Idle;

            // Death Animation Code Here

            deathAnimationTime++;

            // End animation if duration exceeded
            if (deathAnimationTime >= DeathDuration)
            {
                EndAnimation();
                return PokemonState.SpriteState.Idle;
            }

            Draw(sprite, spriteBatch, Color.White);
            return PokemonState.SpriteState.Death;
        }
    }
}
