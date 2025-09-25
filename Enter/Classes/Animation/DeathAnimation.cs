using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class DeathAnimation : AnimationKernel
    {
        public const int DeathDuration = 30; // ~half second at 60fps

        public PokemonState.SpriteState UpdateDeathAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
        {
            if (!stateIsTrue)
                return PokemonState.SpriteState.Idle;

            // How far we are (0 → 1)
            float progress = (float)AnimationTime / DeathDuration;

            // Shrink vertically
            float scaleY = MathHelper.Lerp(4f, 0f, progress);

            // Optional: slide down a little as it faints
            CurrentPosition = startPosition + new Vector2(0, progress * 10f);

            sprite.Draw(spriteBatch, CurrentPosition, Color.White, sprite.Rotation,
                        sprite.Origin, scaleY,
                        SpriteEffects.None, sprite.LayerDepth);

            AnimationTime++;

            if (AnimationTime >= DeathDuration)
            {
                EndAnimation();
                return PokemonState.SpriteState.Death;
            }

            return PokemonState.SpriteState.Death;
        }
    }
}
