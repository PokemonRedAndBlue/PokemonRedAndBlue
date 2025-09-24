using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class AttackAnimation : AnimatedSprite
    {
        private bool isAttacking = false;
        private int attackAnimationTime = 0;
        private const int AttackDuration = 20;

        public Vector2 CurrentPosition { get; private set; }

        public bool IsAttacking => isAttacking;

        // Start the attack animation
        public void StartAttack(Vector2 startPosition)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                attackAnimationTime = 0;
                CurrentPosition = startPosition;
            }
        }

        // Update the animation state; returns the current sprite state
        public PokemonState.SpriteState UpdateAttackAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
        {
            if (!isAttacking)
                return PokemonState.SpriteState.Idle;

            // Forward for first 8 frames, then back
            if (attackAnimationTime <= 8)
                CurrentPosition = startPosition + new Vector2(10, 0);
            else
                CurrentPosition = startPosition - new Vector2(10, 0);

                // clamp to x >= 100 at all times
                if (CurrentPosition.X < 100)
                {
                    CurrentPosition = new Vector2(100, CurrentPosition.Y);
                }

            attackAnimationTime++;

            // End animation if duration exceeded
            if (attackAnimationTime >= AttackDuration)
            {
                EndAttack();
                return PokemonState.SpriteState.Idle;
            }

            Draw(sprite, spriteBatch);
            return PokemonState.SpriteState.Attack;
        }

        // Draw the sprite at its current position
        public void Draw(Sprite sprite, SpriteBatch spriteBatch)
        {
            if (isAttacking)
            {
                sprite.Draw(spriteBatch, CurrentPosition, sprite.Color, sprite.Rotation,
                            sprite.Origin, 4f, SpriteEffects.None, sprite.LayerDepth);
            }
        }

        // Reset the animation
        private void EndAttack()
        {
            isAttacking = false;
            attackAnimationTime = 0;
        }
    }
}
