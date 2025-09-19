using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Graphics;

public class BackSpriteStateMachine
{
    private enum BackSpriteState
    {
        Idle,
        Attack,
        Hurt,
        Death
    }

}