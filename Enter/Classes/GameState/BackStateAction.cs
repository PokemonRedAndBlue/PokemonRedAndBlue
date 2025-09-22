using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class BackStateAction : PokemonStateActions
{
    public void IdleBackAction(Sprite sprite)
    {
        // Implement idle back action logic here
        // do nothing
    }

    public void AttackBackAction(Sprite sprite)
    {
        // Implement attack back action logic here
        // Make sprite move forward and back quickly
        // TODO: add attack animation
    }
}