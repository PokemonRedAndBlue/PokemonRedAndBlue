using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class AttackAnimation : AnimatedSprite
{
    GraphicsDevice GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, new PresentationParameters());
    public void BackAttackAnimation(Sprite sprite, GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteBatch.Begin();

        for (int i = 0; i < 30; i++)
        {
            sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X + i, sprite.Origin.Y));
        }

        for (int i = 30; i > 0; i--)
        {
            sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X + i, sprite.Origin.Y));
        }

        spriteBatch.End();
    }

    public void FrontAttackAnimation(AnimatedSprite sprite, GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteBatch.Begin();

        for (int i = 30; i > 0; i--)
        {
            sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X + i, sprite.Origin.Y));
        }

        for (int i = 0; i < 30; i++)
        {
            sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X + i, sprite.Origin.Y));
        }

        spriteBatch.End();
    }
}