using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

    public static void ChangeBackState(BackSpriteState currentState, String keyboardInput)
    {
        switch (keyboardInput)
        {
            case "I":
                currentState = BackSpriteState.Idle;
                break;
            case "A":
                currentState = BackSpriteState.Attack;
                break;
            case "H":
                currentState = BackSpriteState.Hurt;
                break;
            case "D":
                currentState = BackSpriteState.Death;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}