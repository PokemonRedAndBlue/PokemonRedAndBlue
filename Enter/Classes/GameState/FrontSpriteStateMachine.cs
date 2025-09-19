using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Graphics;

public class FrontSpriteStateMachine
{
    private enum FrontSpriteState
    {
        Idle,
        Attack,
        Hurt,
        Death
    }

}