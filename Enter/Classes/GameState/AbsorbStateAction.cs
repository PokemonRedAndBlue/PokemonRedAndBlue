using System;
using Microsoft.Xna.Framework;

namespace Enter.Classes.GameState
{
    public class AbsorbStateAction
    {
        public (Color color, float scale, float yOffset) GetAbsorbEffect(double elapsedMs, double durationMs)
        {
            // Normalized t in [0, 1]
            double t = Math.Clamp(elapsedMs / durationMs, 0.0, 1.0);

            // Pokémon fades out smoothly
            float alpha = (float)(1.0 - t); // 1 → 0
            Color absorbColor = new Color(1f, 1f, 1f, alpha);

            // Pokémon shrinks faster toward the end (quadratic)
            float scale = (float)(1.0 - (t * t * 0.9));

            // Pokémon drifts slightly upward as it's pulled into the ball, tweak as needed.
            float yOffset = (float)(-20f * t); // moves upward by up to ~20px

            return (absorbColor, scale, yOffset);
        }
    }
}
