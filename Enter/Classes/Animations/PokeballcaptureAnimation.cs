using Enter.Classes.GameState;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Animations;

public class PokeballCaptureAnimation
{


    private enum CaptureState
    {
        Idle,
        Throwing,
        Hit,
        Absorbing,
        InBall,
        Shaking,
        Success,
        Fail
    }

    private CaptureState _state = CaptureState.Idle;
    // Pokeball texture and animation
    private Texture2D _pokeballTexture;
    private Vector2 _pokeballPosition;
    private int activeFrame = 1;
    private int frameCount = 0;

    // Throw animation
    private PokeballthrowAnimation throwAnim;
    private bool throwAnimComplete = false;

    // Absorb effect
    private AbsorbStateAction absorbEffect = new AbsorbStateAction();
    private double absorbTimer = 0;
    private const double ABSORB_DURATION = 800; // milliseconds

    // Shake timing
    private double shakeTimer = 0;
    private int shakesLeft = 3;
    private const double SHAKE_DURATION = 700; // milliseconds per shake

    // Pokemon state
    private Vector2 pokemonPosition;
    private Vector2 pokemonInitialPosition;
    private Texture2D pokemonTexture;
    private bool pokemonVisible = true;
    private float pokemonScale = 1f;
    private Color pokemonColor = Color.White;

    private Vector2 absorbTargetPos;
    private System.Random rng = new System.Random();

    // Capture success rate test (can be modified based on Pokemon health or the Pokemon_Stats.xml data)
    private double captureRate = 0.5;

    public bool CaptureComplete =>
        _state == CaptureState.Success || _state == CaptureState.Fail;

    public bool CaptureSuccessful => _state == CaptureState.Success;

    public PokeballCaptureAnimation(
        Vector2 pokemonPos,
        Texture2D pokemonTex,
        Vector2 pokeballStartPos)
    {
        pokemonPosition = pokemonPos;
        pokemonTexture = pokemonTex;
        _pokeballPosition = pokemonPos;

        throwAnim = new PokeballthrowAnimation(
            (int)pokeballStartPos.X,
            (int)pokeballStartPos.Y
        );

        absorbTargetPos = pokemonPos;
        activeFrame = 1;
        frameCount = 0;
    }

    public void StartCapture()
    {
        _state = CaptureState.Throwing;
    }

    public void LoadContent(ContentManager content)
    {
        _pokeballTexture = content.Load<Texture2D>("images/Pokeball");
        throwAnim.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        double dt = gameTime.ElapsedGameTime.TotalMilliseconds;

        switch (_state)
        {
            case CaptureState.Idle:
                // Waiting to start
                break;

            case CaptureState.Throwing:
                throwAnim.Update(gameTime);
                
                //Check if throw animation has completed its arc
                if (throwAnim.IsComplete)
                {
                    _state = CaptureState.Hit;
                    _pokeballPosition = pokemonPosition;
                }

                // if (!throwAnimComplete)
                // {
                //     frameCount++;
                //     if (frameCount > 80) // ~1.3 seconds at 60fps
                //     {
                //         _state = CaptureState.Hit;
                //         throwAnimComplete = true;
                //         frameCount = 0;
                //     }
                // }
                break;

            case CaptureState.Hit:
                // Brief pause at impact, then start absorbing
                absorbTimer = 0;
                _pokeballPosition = pokemonPosition; // Ball is now at pokemon position
                _state = CaptureState.Absorbing;
                break;

            case CaptureState.Absorbing:
                absorbTimer += dt;

                // Get the absorption effect
                var (color, scale, yOffset) = absorbEffect.GetAbsorbEffect(absorbTimer, ABSORB_DURATION);

                pokemonColor = color;
                pokemonScale = scale;

                // Apply the vertical offset as pokemon is absorbed
                pokemonPosition = pokemonInitialPosition + new Vector2(0, yOffset);

                // Smoothly move Pokemon toward center of ball horizontally
                pokemonPosition = new Vector2(
                    MathHelper.Lerp(pokemonPosition.X, absorbTargetPos.X, 0.08f),
                    pokemonPosition.Y
                );

                if (absorbTimer >= ABSORB_DURATION)
                {
                    pokemonVisible = false;
                    _state = CaptureState.InBall;
                }
                break;

            case CaptureState.InBall:
                // Reset for shaking animation
                shakeTimer = 0;
                shakesLeft = 3;
                activeFrame = 1;
                frameCount = 0;
                _state = CaptureState.Shaking;
                break;

            case CaptureState.Shaking:
                shakeTimer += dt;

                // Animate pokeball shaking during capture
                frameCount++;
                if (frameCount > 10) // Faster frame cycling for more visible shake
                {
                    frameCount = 0;
                    activeFrame = (activeFrame + 1) % 4; // Cycle through frames 0, 1, 2, 3
                }

                if (shakeTimer > SHAKE_DURATION) // Time per shake
                {
                    shakeTimer = 0;
                    shakesLeft--;

                    if (shakesLeft <= 0)
                    {
                        // Decide success or fail based on capture rate
                        if (rng.NextDouble() < captureRate)
                        {
                            _state = CaptureState.Success;
                            activeFrame = 1; // Reset to neutral frame
                        }
                        else
                        {
                            _state = CaptureState.Fail;
                        }
                    }
                }
                break;

            case CaptureState.Success:
                // Pokemon successfully captured 
                // Pokemon added to team
                break;

            case CaptureState.Fail:
                // Pokemon escapes - show the pokemon again
                pokemonVisible = true;
                pokemonColor = Color.White;
                pokemonScale = 1f;
                pokemonPosition = pokemonInitialPosition + new Vector2(-40, -40); // Pop-out offset
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw Pokemon (only if visible)
        if (pokemonVisible)
        {
            spriteBatch.Draw(
                pokemonTexture,
                pokemonPosition,
                null,
                pokemonColor,
                0f,
                new Vector2(
                    pokemonTexture.Width / 2f,
                    pokemonTexture.Height / 2f),
                pokemonScale,
                SpriteEffects.None,
                0f
            );
        }

        // Draw throw animation during throwing
        if (_state == CaptureState.Throwing)
        {
            throwAnim.Draw(spriteBatch);
        }

        // Draw pokeball (shaking, at rest, or success)
        // Don't know what I did here :(
        if (_state == CaptureState.Shaking || 
            _state == CaptureState.Success || 
            _state == CaptureState.InBall ||
            _state == CaptureState.Absorbing)
        {
            Rectangle sourceRect = new Rectangle(0, 24 * activeFrame, 16, 24);
            Vector2 origin = new Vector2(sourceRect.Width / 2f, sourceRect.Height / 2f);
            float scale = 4f;

            spriteBatch.Draw(
                _pokeballTexture,
                _pokeballPosition,
                sourceRect,
                Color.White,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }

}

