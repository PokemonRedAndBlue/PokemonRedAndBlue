using Enter.Classes.GameState;
using Enter.Classes.Textures;
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
        SuccessPause,
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

    // Absorb effect
    private FaintStateAction faintEffect = new FaintStateAction();
    private double absorbTimer = 0;
    private const double ABSORB_DURATION = 800; // milliseconds
    private double successPauseTimer = 0;
    private const double SUCCESS_PAUSE_MS = 4000; // pause after shakes end

    // Shake timing
    private double shakeTimer = 0;
    private int shakesLeft = 3;
    private const double SHAKE_DURATION = 700; // milliseconds per shake

    // Pokemon state
    private Vector2 pokemonPosition;
    private Vector2 pokemonInitialPosition;
    private TextureRegion _pokemonRegion;
    private bool pokemonVisible = true;
    private float pokemonScale = 1f;
    private Color pokemonColor = Color.White;
    private Vector2 targetPosition;
    private SpriteFont _font;

    private Vector2 absorbTargetPos;
    private System.Random rng = new System.Random();

    // Capture success rate test (can be modified based on Pokemon health or the Pokemon_Stats.xml data)
    private double captureRate = 0.5;

    public bool CaptureComplete =>
        _state == CaptureState.Success || _state == CaptureState.Fail;

    public bool CaptureSuccessful => _state == CaptureState.Success;

    // When true, caller should hide the normal wild sprite (capture anim will render it)
    public bool HideWildForCapture =>
        _state == CaptureState.Absorbing ||
        _state == CaptureState.InBall ||
        _state == CaptureState.Shaking ||
        _state == CaptureState.Success;

    public PokeballCaptureAnimation(
        Vector2 pokemonPos,
        TextureRegion pokemonRegion,
        Vector2 pokeballStartPos)
    {
        pokemonPosition = pokemonPos;
        pokemonInitialPosition = pokemonPos;
        _pokemonRegion = pokemonRegion;
        // Aim slightly inset near the upper-left quadrant of the wild sprite (avoid border)
        targetPosition = pokemonPos + new Vector2(12f, -12f);
        _pokeballPosition = pokeballStartPos;

        throwAnim = new PokeballthrowAnimation(
            (int)pokeballStartPos.X,
            (int)pokeballStartPos.Y,
            targetPosition
        );

        absorbTargetPos = targetPosition;
        activeFrame = 1;
        frameCount = 0;
    }

    public void SetCaptureChance(double chance)
    {
        captureRate = MathHelper.Clamp((float)chance, 0.05f, 0.95f);
    }

    public void StartCapture()
    {
        _state = CaptureState.Throwing;
    }

    public void LoadContent(ContentManager content)
    {
        _pokeballTexture = content.Load<Texture2D>("images/Pokeball");
        _font = content.Load<SpriteFont>("PokemonFont");
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
                    _pokeballPosition = targetPosition;
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
                _pokeballPosition = targetPosition; // Ball is now at target position
                _state = CaptureState.Absorbing;
                break;

            case CaptureState.Absorbing:
                absorbTimer += dt;

                // Combine faint fade/scale with an upward arc into the ball
                var (faintColor, faintScale) = faintEffect.GetFaintEffect(absorbTimer, ABSORB_DURATION);
                pokemonColor = faintColor;
                pokemonScale = faintScale;

                // Pull the sprite toward the ball while easing upward
                pokemonPosition = Vector2.Lerp(pokemonPosition, absorbTargetPos, 0.12f);
                pokemonPosition.Y -= 0.4f * (float)(dt / 16.0); // small lift while being absorbed

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
                            successPauseTimer = 0;
                            _state = CaptureState.SuccessPause;
                            activeFrame = 1; // Reset to neutral frame
                        }
                        else
                        {
                            _state = CaptureState.Fail;
                        }
                    }
                }
                break;

            case CaptureState.SuccessPause:
                successPauseTimer += dt;
                if (successPauseTimer >= SUCCESS_PAUSE_MS)
                {
                    _state = CaptureState.Success;
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
        // Draw the pokemon being absorbed with faint effect
        if (pokemonVisible && (_state == CaptureState.Absorbing))
        {
            Rectangle source = _pokemonRegion.SourceRectangle;
            Vector2 origin = Vector2.Zero;
            float scale = 4f * pokemonScale;
            spriteBatch.Draw(
                _pokemonRegion.Texture,
                pokemonPosition,
                source,
                pokemonColor,
                0f,
                origin,
                scale,
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
            _state == CaptureState.SuccessPause ||
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

            if (_state == CaptureState.SuccessPause && _font != null)
            {
                string msg = "POKEMON CAUGHT";
                Vector2 textSize = _font.MeasureString(msg);
                // Position text centered under the ball
                Vector2 textPos = _pokeballPosition + new Vector2(-textSize.X / 2f, sourceRect.Height * scale / 2f + 8f);
                // Drop shadow then main text for visibility
                spriteBatch.DrawString(_font, msg, textPos + new Vector2(2f, 2f), Color.Black);
                spriteBatch.DrawString(_font, msg, textPos, Color.White);
            }
        }
    }

}

