using Enter.Classes.Animations;

namespace Enter.Classes.GameState;

public class FrontStateAction : PokemonStateActions
{
    public void IdleFrontAction(AnimatedSprite sprite)
    {
        // Implement idle front action logic here
        // essentially do not update the sprite animation still draw
    }

    public void AttackFrontAction(AnimatedSprite sprite)
    {
        // Implement attack front action logic here
        // update sprite animation
    }
}
