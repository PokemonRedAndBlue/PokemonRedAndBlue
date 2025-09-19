using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Behavior.Time;

public class Time : GameTime
{
    public void Delay(double seconds)
    {
        double start = TotalGameTime.TotalSeconds;
        while (TotalGameTime.TotalSeconds - start < seconds)
        {
            // Wait
        }
    }
}