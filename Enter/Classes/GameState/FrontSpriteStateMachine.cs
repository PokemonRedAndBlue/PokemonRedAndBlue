using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public class FrontSpriteStateMachine
{
    private FrontSpriteState _currentFrontState = FrontSpriteState.Idle;
    private enum FrontSpriteState
    {
        Idle,
        Attack,
        Hurt,
        Death
    }

    public static void UpdateFrontState(GameTime gameTime, FrontSpriteState currentFrontState)
    {
        switch (currentFrontState)
        {
            case FrontSpriteState.Idle:
                // Handle Idle state logic
                break;
            case FrontSpriteState.Attack:
                // Handle Attack state logic
                break;
            case FrontSpriteState.Hurt:
                // Handle Hurt state logic
                break;
            case FrontSpriteState.Death:
                // Handle Death state logic
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void ChangeState(FrontSpriteState currentState, String keyboardInput)
    {
        switch(keyboardInput)
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