using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameLibrary.Graphics
{
    public class HurtAnimation : AnimationKernel
    {
        private Color color = Color.White;

        public PokemonState.SpriteState UpdateHurtAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
        {
            if (!stateIsTrue)
                return PokemonState.SpriteState.Idle;

            // Flicker red/white
            if (AnimationTime % 2 == 0)
            {
                CurrentPosition = startPosition + new Vector2(2, 0);
            }
            else
            {
                CurrentPosition = startPosition - new Vector2(2, 0);
            }

            AnimationTime++;

            if (AnimationTime >= AnimationDuration)
            {
                EndAnimation();
                return PokemonState.SpriteState.Idle;
            }

            Draw(sprite, spriteBatch, color, 4f);
            return PokemonState.SpriteState.Hurt;
        }
    }
}
