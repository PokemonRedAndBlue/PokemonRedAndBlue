using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class HurtAnimation : AnimationKernel
    {
        private bool isHurt = false;
        private int hurtAnimationTime = AnimationDuration;
        private const int HurtDuration = 20;

        private Color color;

        // Update the animation state; returns the current sprite state
        public PokemonState.SpriteState UpdateHurtAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
        {
            if (!isHurt)
                return PokemonState.SpriteState.Idle;

            Console.Write(hurtAnimationTime);

            // Hurt Animation goes here
            if (CurrentPosition.X % 2 == 0)
            {
                color = Color.Red;
                CurrentPosition = startPosition + new Vector2(2, 0);
                Console.Write("we made it");
            }
            else
            {
                color = Color.White;
                CurrentPosition = startPosition - new Vector2(2, 0);
            }

            hurtAnimationTime++;

            // End animation if duration exceeded
            if (hurtAnimationTime >= HurtDuration)
            {
                EndAnimation();
                return PokemonState.SpriteState.Hurt;
            }

            Draw(sprite, spriteBatch, color);
            return PokemonState.SpriteState.Attack;
        }
    }
}
