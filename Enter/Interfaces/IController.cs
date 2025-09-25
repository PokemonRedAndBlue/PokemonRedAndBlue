using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFile;
using KeyboardController;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IController
{
    public interface IController
    {
        void Update(Game1 game);
        int inputValue(int userInput);
    }
}