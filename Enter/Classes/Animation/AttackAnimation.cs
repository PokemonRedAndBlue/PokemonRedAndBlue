using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class AttackAnimation : AnimatedSprite
{
    GraphicsDevice GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, new PresentationParameters());
    public void FrontAttackAnimation(Sprite sprite, GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteBatch.Begin();
        sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X + 30, sprite.Origin.Y));
        sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X - 30, sprite.Origin.Y));
        sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X + 30, sprite.Origin.Y));

        spriteBatch.End();
    }

    public void BackAttackAnimation(AnimatedSprite sprite, GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteBatch.Begin();

        sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X, sprite.Origin.Y));
        sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X - 30, sprite.Origin.Y));
        sprite.Draw(spriteBatch, new Vector2(sprite.Origin.X, sprite.Origin.Y));

        spriteBatch.End();
    }
}