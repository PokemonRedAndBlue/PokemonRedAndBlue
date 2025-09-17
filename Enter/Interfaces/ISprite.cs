using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISprite
{
    public interface ISprite
    {
        void Update();
        void Draw(int x, int y, SpriteEffects facing);
        int GetWidth();
        int GetHeight();
    }
}