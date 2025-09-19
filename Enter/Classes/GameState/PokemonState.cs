using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class PokemonState
{
    public SpriteState _currentFrontState = SpriteState.Idle;
    public SpriteState _currentBackState = SpriteState.Idle;
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
            case var _ when keyboardInput.IsKeyDown(Keys.D0):
                currentState = SpriteState.Idle;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D1):
                currentState = SpriteState.Attack;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D2):
                currentState = SpriteState.Hurt;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D3):
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
            case var _ when keyboardInput.IsKeyDown(Keys.D0):
                currentState = SpriteState.Idle;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D1):
                currentState = SpriteState.Attack;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D2):
                currentState = SpriteState.Hurt;
                break;
            case var _ when keyboardInput.IsKeyDown(Keys.D3):
                currentState = SpriteState.Death;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}