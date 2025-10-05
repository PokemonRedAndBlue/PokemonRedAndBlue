using Microsoft.Xna.Framework;

namespace Enter.Interfaces;

public interface IPlayer
{
    void Update(GameTime gameTime, int axisX, int axisY);
}
