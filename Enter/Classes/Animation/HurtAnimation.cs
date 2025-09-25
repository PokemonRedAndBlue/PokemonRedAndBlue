using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class HurtAnimation : AnimationKernel
    {
        private bool isHurt = false;
        private int hurtAnimationTime = 0;
        private const int HurtDuration = 20;

        // Update the animation state; returns the current sprite state
        public PokemonState.SpriteState UpdateHurtAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
        {
            if (!isHurt)
                return PokemonState.SpriteState.Idle;

            // TODO: Hurt Animation goes here
           
            hurtAnimationTime++;

            // End animation if duration exceeded
            if (hurtAnimationTime >= HurtDuration)
            {
                EndAnimation();
                return PokemonState.SpriteState.Hurt;
            }

            Draw(sprite, spriteBatch, Color.White);
            return PokemonState.SpriteState.Attack;
        }
    }
}
