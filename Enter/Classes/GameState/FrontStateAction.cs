using System;
using Enter.Classes.Animations;
using Microsoft.Xna.Framework;

namespace Enter.Classes.GameState;

public class FrontStateAction : PokemonStateActions
{
    // Return a positional offset to apply when performing the front/foe attack
    // elapsedMs: how long the attack has been playing
    // durationMs: total duration of the attack animation
    public Vector2 AttackFrontAction(AnimatedSprite sprite, double elapsedMs, double durationMs)
    {
        // If nothing to do, return zero offset
        if (sprite == null) return Vector2.Zero;
        // Use a simple triangular motion: move forward then back along X axis
        // normalized t in [0,1]
        double t = Math.Min(Math.Max(elapsedMs / durationMs, 0.0), 1.0);
        // triangular shape: ramp up to 0.5 then ramp down
        double tri = t < 0.5 ? (t / 0.5) : (1.0 - (t - 0.5) / 0.5);
        // movement magnitude in pixels (scaled to UI scale elsewhere)
        float magnitude = 12f;
        float offsetX = (float)(magnitude * tri);
        // For front/foe, move slightly up/down for expressiveness
        float offsetY = (float)(-4f * tri);
        return new Vector2(offsetX, offsetY);
    }

    public Vector2 IdleFrontAction(AnimatedSprite sprite)
    {
        return Vector2.Zero;
    }
}
