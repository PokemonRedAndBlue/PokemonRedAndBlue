using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class PokemonState
{
    public SpriteState _currentFrontState;
    public SpriteState _currentBackState;
    public enum SpriteState
    {
        Idle,
        Attack,
        Hurt,
        Death
    }

    public static void ChangeBackState(SpriteState currentState, KeyboardState keyboardInput)
    {
        switch (keyboardInput)
        {
            case var _ when keyboardInput.IsKeyDown(Keys.Q):
                currentState = SpriteState.Idle;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.W):
                currentState = SpriteState.Attack;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.E):
                currentState = SpriteState.Hurt;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.R):
                currentState = SpriteState.Death;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void ChangeFrontState(SpriteState currentState, KeyboardState keyboardInput)
    {
        switch (keyboardInput)
        {
            case var _ when keyboardInput.IsKeyDown(Keys.Q):
                currentState = SpriteState.Idle;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.W):
                currentState = SpriteState.Attack;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.E):
                currentState = SpriteState.Hurt;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.R):
                currentState = SpriteState.Death;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}