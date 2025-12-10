using System;
using Enter.Classes.Sprites;
using Microsoft.Xna.Framework;

namespace Enter.Classes.GameState;

public class BackStateAction : PokemonStateActions
{
    // Return positional offset for back/ally attack motion
    public Vector2 AttackBackAction(Sprite sprite, double elapsedMs, double durationMs)
    {
        if (sprite == null) return Vector2.Zero;
        double t = System.Math.Min(System.Math.Max(elapsedMs / durationMs, 0.0), 1.0);
        double tri = t < 0.5 ? (t / 0.5) : (1.0 - (t - 0.5) / 0.5);
        float magnitude = 10f;
        float offsetX = (float)(magnitude * tri); // back/ally moves right when attacking
        float offsetY = (float)(-3f * tri);
        return new Vector2(offsetX, offsetY);
    }

    public Vector2 IdleBackAction(Sprite sprite)
    {
        return Vector2.Zero;
    }
}
