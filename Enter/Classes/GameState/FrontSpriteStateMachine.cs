using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
    
    public static void ChangeFrontState(FrontSpriteState currentState, String keyboardInput)
    {
        switch (keyboardInput)
        {
            case "I":
                currentState = FrontSpriteState.Idle;
                break;
            case "A":
                currentState = FrontSpriteState.Attack;
                break;
            case "H":
                currentState = FrontSpriteState.Hurt;
                break;
            case "D":
                currentState = FrontSpriteState.Death;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}