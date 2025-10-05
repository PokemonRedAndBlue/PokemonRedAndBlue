using Microsoft.Xna.Framework.Graphics;

namespace Enter.Interfaces;

public interface ISprite
{
    void Update();
    void Draw(int x, int y, SpriteEffects facing);
    int GetWidth();
    int GetHeight();
}
