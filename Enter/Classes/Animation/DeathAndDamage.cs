using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class DeathAndDamage : AnimatedSprite
{
    public void HurtAnimation(Sprite sprite)
    {
        // Implement hurt animation logic here
        // Draw sprite with red tint and make it flash
    }

    public void HurtAnimation(AnimatedSprite sprite)
    {
        // Implement hurt animation logic here
        // Draw sprite with red tint and make it flash
    }

    public void DeathAnimation(Sprite sprite)
    {
        // Implement death animation logic here 
    }

    public void DeathAnimation(AnimatedSprite sprite)
    {
        // Implement death animation logic here 
    }
}