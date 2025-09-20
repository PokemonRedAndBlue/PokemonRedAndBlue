using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;    
using Behavior.Time;

namespace MonoGameLibrary.Graphics;

public class AttackAnimation : AnimatedSprite
{
    // Use the engine's shared SpriteBatch instead of creating a new GraphicsDevice
    public void BackAttackAnimation(Sprite sprite, SpriteBatch spriteBatch, GameTime gameTime)
    {
        for (int i = 0; i < 30; i++)
        {
            var pos = new Vector2(sprite.Origin.X + i, sprite.Origin.Y);
            sprite.Draw(spriteBatch, pos, sprite.Color, sprite.Rotation, sprite.Origin, 4f, SpriteEffects.None, sprite.LayerDepth);
        }

        for (int i = 30; i > 0; i--)
        {
            var pos = new Vector2(sprite.Origin.X + i, sprite.Origin.Y);
            sprite.Draw(spriteBatch, pos, sprite.Color, sprite.Rotation, sprite.Origin, 4f, SpriteEffects.None, sprite.LayerDepth);
        }
    }

    public void FrontAttackAnimation(AnimatedSprite sprite, GameTime gameTime, SpriteBatch spriteBatch, GameTime gameTime)
    {
        for (int i = 30; i > 0; i--)
        {
            var pos = new Vector2(sprite.Origin.X + i, sprite.Origin.Y);
            sprite.Draw(spriteBatch, pos, sprite.Color, sprite.Rotation, sprite.Origin, 4f, SpriteEffects.None, sprite.LayerDepth);
        }

        for (int i = 0; i < 30; i++)
        {
            var pos = new Vector2(sprite.Origin.X + i, sprite.Origin.Y);
            sprite.Draw(spriteBatch, pos, sprite.Color, sprite.Rotation, sprite.Origin, 4f, SpriteEffects.None, sprite.LayerDepth);
        }
    }
}