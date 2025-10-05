using Microsoft.Xna.Framework;

namespace Enter.Classes.Behavior;

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
