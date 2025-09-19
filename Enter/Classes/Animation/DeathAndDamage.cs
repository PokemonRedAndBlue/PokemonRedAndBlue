using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class DeathAndDamage : AnimatedSprite
{
    GraphicsDevice GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, new PresentationParameters());
    public void HurtAnimation(Sprite sprite, GameTime gameTime)
    {
        // Implement hurt animation logic here
        // Draw sprite with red tint and make it flash
    }

    public void HurtAnimation(AnimatedSprite sprite, GameTime gameTime)
    {
        // Implement hurt animation logic here
        // Draw sprite with red tint and make it flash
    }

    public void DeathAnimation(Sprite sprite, GameTime gameTime)
    {
        // Implement death animation logic here 
    }

    public void DeathAnimation(AnimatedSprite sprite, GameTime gameTime)
    {
        // Implement death animation logic here 
    }
}