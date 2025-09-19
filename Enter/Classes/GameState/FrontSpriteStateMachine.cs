using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class FrontSpriteStateMachine
{
    public FrontSpriteState _currentFrontState = FrontSpriteState.Idle;
    public enum FrontSpriteState
    {
        Idle,
        Attack,
        Hurt,
        Death
    }
    
    public static void ChangeFrontState(FrontSpriteState currentState, KeyboardState keyboardInput)
    {
        switch (keyboardInput)
        {
            case var _ when keyboardInput.IsKeyDown(Keys.D0):
                currentState = FrontSpriteState.Idle;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D1):
                currentState = FrontSpriteState.Attack;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D2):
                currentState = FrontSpriteState.Hurt;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D3):
                currentState = FrontSpriteState.Death;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}