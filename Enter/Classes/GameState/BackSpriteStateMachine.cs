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

    public static void UpdateBackState(GameTime gameTime, BackSpriteState currentBackState)
    {
        switch (currentBackState)
        {
            case BackSpriteState.Idle:
                // Handle Idle state logic
                break;
            case BackSpriteState.Attack:
                // Handle Attack state logic
                break;
            case BackSpriteState.Hurt:
                // Handle Hurt state logic
                break;
            case BackSpriteState.Death:
                // Handle Death state logic
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void ChangeState(BackSpriteState currentState, String keyboardInput)
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