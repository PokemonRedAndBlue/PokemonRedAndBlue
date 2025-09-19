using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class Time : GameTime
{
    public void Delay(int seconds)
    {
        double start = TotalGameTime.TotalSeconds;
        while (TotalGameTime.TotalSeconds - start < seconds)
        {
            // Wait
        }
    }
}