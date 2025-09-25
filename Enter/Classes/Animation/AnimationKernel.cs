using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class AnimationKernel
    {
        public bool stateIsTrue = false;
        public int AnimationTime = 0;
        public const int AnimationDuration = 20;

        public Vector2 CurrentPosition { get; protected set; }

        // Start the animation
        public void StartAnimation(Vector2 startPosition)
        {
            if (!stateIsTrue)
            {
                stateIsTrue = true;
                AnimationTime = 0;
                CurrentPosition = startPosition;
            }
        }

        // Reset the animation
        public void EndAnimation()
        {
            stateIsTrue = false;
            AnimationTime = 0;
        }

        // Draw the sprite at its current position
        public void Draw(Sprite sprite, SpriteBatch spriteBatch, Color color, float scale)
        {
            if (stateIsTrue)
            {
                sprite.Draw(spriteBatch, CurrentPosition, color, sprite.Rotation,
                            sprite.Origin, scale, SpriteEffects.None, sprite.LayerDepth);
            }
        }
    }
}
