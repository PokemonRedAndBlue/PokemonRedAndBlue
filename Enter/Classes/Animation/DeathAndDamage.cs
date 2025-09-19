using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Behavior.Time;

namespace MonoGameLibrary.Graphics;

public class DeathAndDamage : AnimatedSprite
{
    GraphicsDevice GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, new PresentationParameters());
    public void HurtAnimation(Sprite sprite, GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteBatch.Begin();
        var timer = new Time();

        // Draw sprite with red tint and make it flash
        for (int i = 0; i < 3; i++)
        {
            sprite.Draw(spriteBatch, sprite.Origin, Color.Red, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            timer.Delay(0.25);
            sprite.Draw(spriteBatch, sprite.Origin, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            timer.Delay(0.25);
        }

        sprite.

        spriteBatch.End();
    }

    public void HurtAnimation(AnimatedSprite sprite, GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteBatch.Begin();



        spriteBatch.End();
    }

    public void DeathAnimation(Sprite sprite, GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteBatch.Begin();



        spriteBatch.End();
    }

    public void DeathAnimation(AnimatedSprite sprite, GameTime gameTime)
    {
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteBatch.Begin();



        spriteBatch.End();
    }
}