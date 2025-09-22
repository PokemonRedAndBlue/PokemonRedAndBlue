using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KeyboardController
{
    public class KeyboardController
    {
        public void Update(Game1 game)  
        {
            // Get the current state of keyboard input.
            KeyboardState keyboardState = Keyboard.GetState();
        
        }
    }
}