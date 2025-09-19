using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class FrontStateAction: StateActions
{
    public void IdleFrontAction()
    {
        // Implement idle front action logic here
        // essentially do not update the sprite animation still draw
    }

    public void AttackFrontAction()
    {
        // Implement attack front action logic here
        // update sprite animation
    }
}