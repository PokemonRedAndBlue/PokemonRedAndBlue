using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameFile;

namespace MouseController
{
    public class MouseController
    {
        public void Update(Game1 game)
        {
            // Get the current state of mouse input.
            MouseState mouseState = Mouse.GetState();

            // Get location of click
            Point mousePosition = new Point(mouseState.X, mouseState.Y);


            }
        }
    }
