using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class FrontStateAction: PokemonStateActions
{
    public void IdleFrontAction(AnimatedSprite sprite)
    {
        // Implement idle front action logic here
        // essentially do not update the sprite animation still draw
    }

    public void AttackFrontAction(AnimatedSprite sprite)
    {
        // Implement attack front action logic here
        // update sprite animation
    }
}