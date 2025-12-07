using System;
using Microsoft.Xna.Framework;

namespace Enter.Classes.GameState;

public class FaintStateAction
{
    // Returns a fade and scale multiplier for a fainting pokemon
    // elapsedMs: how long the faint has been playing
    // durationMs: total duration of the faint animation
    public (Color color, float scale) GetFaintEffect(double elapsedMs, double durationMs)
    {
        // normalized t in [0,1]
        double t = Math.Min(Math.Max(elapsedMs / durationMs, 0.0), 1.0);
        
        // Create blinking effect: oscillate between visible and semi-transparent
        // Blink frequency increases as the sprite shrinks
        double blinkSpeed = 8.0; // Number of blinks during animation
        double blinkPhase = (t * blinkSpeed * Math.PI * 2);
        double blinkFactor = (Math.Sin(blinkPhase) + 1.0) / 2.0; // Oscillates 0 to 1
        
        // Alpha fades out while blinking: start at 255, end at 0
        float alpha = (float)(255 * (1.0 - t) * blinkFactor);
        Color faintColor = new Color(1.0f, 1.0f, 1.0f, alpha / 255.0f);
        
        // Scale down smoothly as it fades
        float scale = (float)(1.0 - (t * t * 1.0f)); // Quadratic shrink (faster at end)
        
        return (faintColor, scale);
    }
}
