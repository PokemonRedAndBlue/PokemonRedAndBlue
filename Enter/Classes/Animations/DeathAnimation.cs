using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Enter.Classes.GameState;
using Enter.Classes.Sprites;

namespace Enter.Classes.Animations;

public class DeathAnimation : AnimationKernel
{
    public const int DeathDuration = 30; // ~0.5 seconds at 60fps

    private float scale = 4f;

    public PokemonState.SpriteState UpdateDeathAnimation(Vector2 startPosition, Sprite sprite, SpriteBatch spriteBatch)
    {
        if (!stateIsTrue)
            return PokemonState.SpriteState.Idle;

        // Death Animation Logic
        startPosition.Y += 2;
        CurrentPosition = startPosition;

        // shrink scale
        scale /= (float) 1.1;

        AnimationTime++;

        if (AnimationTime >= DeathDuration)
        {
            EndAnimation();
            return PokemonState.SpriteState.Idle; // TODO: add fainted state
        }

        Draw(sprite, spriteBatch, Color.White, scale);
        return PokemonState.SpriteState.Death;
    }
}
