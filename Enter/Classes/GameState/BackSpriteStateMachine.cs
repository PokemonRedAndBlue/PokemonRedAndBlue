using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class BackSpriteStateMachine
{
    public BackSpriteState _currentBackState = BackSpriteState.Idle;
    public enum BackSpriteState
    {
        Idle,
        Attack,
        Hurt,
        Death
    }

    public static void ChangeBackState(BackSpriteState currentState, KeyboardState keyboardInput)
    {
        switch (keyboardInput)
        {
            case var _ when keyboardInput.IsKeyDown(Keys.D0):
                currentState = BackSpriteState.Idle;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D1):
                currentState = BackSpriteState.Attack;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D2):
                currentState = BackSpriteState.Hurt;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D3):
                currentState = BackSpriteState.Death;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}