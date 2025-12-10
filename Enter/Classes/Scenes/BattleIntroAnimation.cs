using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using System;

namespace Enter.Classes.Scenes.IntroAnimations
{
    /// <summary>
    /// The battle animation component between Gengar and Jigglypuff for the title screen sequence
    /// </summary>
    public class BattleIntroAnimation
    {
        private enum State
        {
            SlideIn,
            JigglypuffHopTwice,
            GengarMoveLeftArmRaised,
            GengarLunge,
            JigglypuffHopAwayRight,
            JigglypuffIdle,
            JigglypuffHopLeftRight,
            JigglypuffPrepareAttack,
            GengarReturn,
            JigglypuffJumpAttack,
            Complete
        }

        // ============================================================
        // DISPLAY CONSTANTS
        // ============================================================
        private const float GAMEBOY_WIDTH = 160f;
        private const float GAMEBOY_HEIGHT = 144f;
        private const int CONTENT_CENTER_Y = 84;
        private const float WINDOW_WIDTH = 1280f;

        // ============================================================
        // TIMING CONSTANTS - ADJUST THESE TO CHANGE ANIMATION SPEED (seconds)
        // ============================================================
        private const float SLIDE_DURATION = 3.0f;
        private const float HOP_TWICE_DURATION = 4.0f;
        private const float HOP_DURATION = 2.0f;
        private const float ARM_RAISED_MOVE_DURATION = 1.0f;
        private const float LUNGE_DURATION = 1.5f;
        private const float IDLE_DURATION = 0.5f;
        private const float HOP_LEFT_RIGHT_DURATION = 4.0f;
        private const float PREPARE_ATTACK_DURATION = 2.57f;
        private const float RETURN_DURATION = 1.0f;
        private const float JUMP_ATTACK_DURATION = 2.0f;

        // ============================================================
        // POSITION CONSTANTS (in Game Boy coordinates)
        // ============================================================
        private const float GENGAR_IDLE_X = 135f;
        private const float GENGAR_IDLE_Y_OFFSET = 28f;
        private const float GENGAR_LUNGE_X_OFFSET = 20f;
        private const float GENGAR_LUNGE_FORWARD_OFFSET = 40f;
        private const float GENGAR_SLIDE_START_OFFSET = 64f;
        private const float GENGAR_SLIDE_START_Y_OFFSET = 27f;

        private const float JIGGLYPUFF_IDLE_X = 85f;
        private const float JIGGLYPUFF_IDLE_Y_OFFSET = 23f;
        private const float JIGGLYPUFF_SLIDE_START_OFFSET = 56f;

        // ============================================================
        // ANIMATION CONSTANTS
        // ============================================================
        private const int TOTAL_HOPS = 2;
        private const float HOP_FIRST_HORIZONTAL = 30f;
        private const float HOP_FIRST_VERTICAL = 20f;
        private const float HOP_AWAY_HORIZONTAL = 50f;
        private const float HOP_AWAY_VERTICAL = 15f;
        private const float HOP_LEFT_RIGHT_HORIZONTAL = 20f;
        private const float HOP_LEFT_RIGHT_VERTICAL = 20f;
        private const float JUMP_ATTACK_HORIZONTAL = 160f;
        private const float JUMP_ATTACK_VERTICAL = 65f;

        private State _currentState = State.SlideIn;
        private float _stateTimer = 0f;

        private Sprite _gengarIdle;
        private Sprite _gengarArmRaised;
        private Sprite _gengarLunge;

        private Sprite _jigglypuffIdle;
        private Sprite _jigglypuffHop;
        private Sprite _jigglypuffJumpAttack;

        private Vector2 _gengarPosition;
        private Vector2 _gengarIdlePosition;
        private Vector2 _gengarLungePosition;
        private Vector2 _gengarLungeForwardPosition;

        private Vector2 _jigglypuffPosition;
        private Vector2 _jigglypuffIdlePosition;

        // ============================================================
        // FLAGS - Make sure sound effects only play once per action
        // ============================================================
        private bool _hasPlayedSlideSound = false;
        private bool _hasPlayedHop1Sound = false;
        private bool _hasPlayedHop2Sound = false;
        private bool _hasPlayedRaiseSound = false;
        private bool _hasPlayedLungeSound = false;
        private bool _hasPlayedHopAwaySound = false;
        private bool _hasPlayedAttackSound = false;

        private int _hopCount = 0;
        private float _hopTimer = 0f;

        private Rectangle _screenBounds;
        private bool _jigglypuffIsPreparingAttack = false;
        
        public bool IsComplete => _currentState == State.Complete;

        public BattleIntroAnimation(TextureAtlas atlas, Rectangle screenBounds)
        {
            _screenBounds = screenBounds;

            _gengarIdle = atlas.CreateSprite("gengar_idle");
            _gengarArmRaised = atlas.CreateSprite("gengar_arm_raised");
            _gengarLunge = atlas.CreateSprite("gengar_lunge");

            _jigglypuffIdle = atlas.CreateSprite("jigglypuff_idle");
            _jigglypuffHop = atlas.CreateSprite("jigglypuff_hop");
            _jigglypuffJumpAttack = atlas.CreateSprite("jigglypuff_jump_attack");

            float scaleX = screenBounds.Width / GAMEBOY_WIDTH;
            float scaleY = screenBounds.Height / GAMEBOY_HEIGHT;
            int scaledCenterY = (int)(_screenBounds.Y + CONTENT_CENTER_Y * scaleY);

            _gengarIdlePosition = new Vector2(
                _screenBounds.Right - GENGAR_IDLE_X * scaleX,
                scaledCenterY - GENGAR_IDLE_Y_OFFSET * scaleY
            );
            _gengarLungePosition = new Vector2(
                _gengarIdlePosition.X - GENGAR_LUNGE_X_OFFSET * scaleX,
                scaledCenterY - GENGAR_IDLE_Y_OFFSET * scaleY
            );
            _gengarLungeForwardPosition = new Vector2(
                _gengarLungePosition.X + GENGAR_LUNGE_FORWARD_OFFSET * scaleX,
                scaledCenterY - GENGAR_IDLE_Y_OFFSET * scaleY
            );
            _gengarPosition = new Vector2(
                _screenBounds.Right + GENGAR_SLIDE_START_OFFSET, 
                scaledCenterY - GENGAR_SLIDE_START_Y_OFFSET * scaleY
            );

            _jigglypuffIdlePosition = new Vector2(
                _screenBounds.X + JIGGLYPUFF_IDLE_X * scaleX,
                scaledCenterY - JIGGLYPUFF_IDLE_Y_OFFSET * scaleY
            );
            _jigglypuffPosition = new Vector2(
                _screenBounds.X - JIGGLYPUFF_SLIDE_START_OFFSET, 
                scaledCenterY - JIGGLYPUFF_IDLE_Y_OFFSET * scaleY
            );
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _stateTimer += dt;

            switch (_currentState)
            {
                case State.SlideIn:
                    UpdateSlideIn(dt);
                    break;
                case State.JigglypuffHopTwice:
                    UpdateJigglypuffHopTwice(dt);
                    break;
                case State.GengarMoveLeftArmRaised:
                    UpdateGengarMoveLeftArmRaised(dt);
                    break;
                case State.GengarLunge:
                    UpdateGengarLunge(dt);
                    break;
                case State.JigglypuffHopAwayRight:
                    UpdateJigglypuffHopAwayRight(dt);
                    break;
                case State.GengarReturn:
                    UpdateGengarReturn(dt);
                    break;
                case State.JigglypuffIdle:
                    UpdateJigglypuffIdle(dt);
                    break;
                case State.JigglypuffHopLeftRight:
                    UpdateJigglypuffHopLeftRight(dt);
                    break;
                case State.JigglypuffPrepareAttack:
                    _jigglypuffIsPreparingAttack = true;
                    UpdateJigglypuffPrepareAttack(dt);
                    break;
                case State.JigglypuffJumpAttack:
                    _jigglypuffIsPreparingAttack = false;
                    UpdateJigglypuffJumpAttack(dt);
                    break;
            }
        }

        private void UpdateSlideIn(float dt)
        {
            if (!_hasPlayedSlideSound)
            {
                SoundEffectPlayer.Play(SfxId.SFX_INTRO_WHOOSH);
                _hasPlayedSlideSound = true;
            }

            float progress = _stateTimer / SLIDE_DURATION;

            _gengarPosition = Vector2.Lerp(
                new Vector2(_screenBounds.Width + GENGAR_SLIDE_START_OFFSET, _gengarIdlePosition.Y),
                _gengarIdlePosition,
                progress
            );

            _jigglypuffPosition = Vector2.Lerp(
                new Vector2(-JIGGLYPUFF_SLIDE_START_OFFSET, _jigglypuffIdlePosition.Y),
                _jigglypuffIdlePosition,
                progress
            );

            if (_stateTimer >= SLIDE_DURATION)
            {
                _currentState = State.JigglypuffHopTwice;
                _stateTimer = 0f;
                _hopCount = 0;
                _hopTimer = 0f;
            }
        }

        private void UpdateJigglypuffHopTwice(float dt)
        {
            _hopTimer += dt;

            if (_hopTimer >= HOP_DURATION && _hopCount < TOTAL_HOPS)
            {
                _hopCount++;
                _hopTimer = 0f;

                if (_hopCount == 1 && !_hasPlayedHop1Sound)
                {
                    SoundEffectPlayer.Play(SfxId.SFX_INTRO_HOP);
                    _hasPlayedHop1Sound = true;
                }
                else if (_hopCount == 2 && !_hasPlayedHop2Sound)
                {
                    SoundEffectPlayer.Play(SfxId.SFX_INTRO_HOP);
                    _hasPlayedHop2Sound = true;
                }
            }

            if (_hopCount < TOTAL_HOPS)
            {
                float hopProgress = _hopTimer / HOP_DURATION;
                float horizontalMove = (_hopCount == 0) 
                    ? HOP_FIRST_HORIZONTAL * hopProgress 
                    : HOP_FIRST_HORIZONTAL - (HOP_FIRST_HORIZONTAL * hopProgress);

                float verticalArc = (float)Math.Sin(hopProgress * Math.PI) * HOP_FIRST_VERTICAL;

                _jigglypuffPosition = new Vector2(
                    _jigglypuffIdlePosition.X + horizontalMove,
                    _jigglypuffIdlePosition.Y - verticalArc
                );
            }

            if (_stateTimer >= HOP_TWICE_DURATION)
            {
                _jigglypuffPosition = _jigglypuffIdlePosition;
                _currentState = State.GengarMoveLeftArmRaised;
                _stateTimer = 0f;
            }
        }

        private void UpdateGengarMoveLeftArmRaised(float dt)
        {
            if (!_hasPlayedRaiseSound)
            {
                SoundEffectPlayer.Play(SfxId.SFX_INTRO_RAISE);
                _hasPlayedRaiseSound = true;
            }

            float progress = _stateTimer / ARM_RAISED_MOVE_DURATION;
            _gengarPosition = Vector2.Lerp(_gengarIdlePosition, _gengarLungePosition, progress);

            if (_stateTimer >= ARM_RAISED_MOVE_DURATION)
            {
                _gengarPosition = _gengarLungePosition;
                _currentState = State.GengarLunge;
                _stateTimer = 0f;
            }
        }

        private void UpdateGengarLunge(float dt)
        {
            if (!_hasPlayedLungeSound)
            {
                SoundEffectPlayer.Play(SfxId.SFX_INTRO_LUNGE);
                _hasPlayedLungeSound = true;
            }
            
            float progress = _stateTimer / LUNGE_DURATION;
            _gengarPosition = Vector2.Lerp(_gengarLungePosition, _gengarLungeForwardPosition, progress);

            if (_stateTimer >= LUNGE_DURATION)
            {
                _gengarPosition = _gengarLungeForwardPosition;
                _currentState = State.JigglypuffHopAwayRight;
                _stateTimer = 0f;
                _hopTimer = 0f;
            }
        }

        private void UpdateJigglypuffHopAwayRight(float dt)
        {
            if (!_hasPlayedHopAwaySound)
            {
                SoundEffectPlayer.Play(SfxId.SFX_INTRO_HOP);
                _hasPlayedHopAwaySound = true;
            }

            _hopTimer += dt;
            float progress = _hopTimer / HOP_DURATION;

            if (progress < 1f)
            {
                float horizontalMove = HOP_AWAY_HORIZONTAL * progress;
                float verticalArc = (float)Math.Sin(progress * Math.PI) * HOP_AWAY_VERTICAL;

                _jigglypuffPosition = new Vector2(
                    _jigglypuffIdlePosition.X + horizontalMove,
                    _jigglypuffIdlePosition.Y - verticalArc
                );
            }
            else
            {
                _jigglypuffPosition = new Vector2(
                    _jigglypuffIdlePosition.X + HOP_AWAY_HORIZONTAL,
                    _jigglypuffIdlePosition.Y
                );
                _currentState = State.GengarReturn;
                _stateTimer = 0f;
            }
        }

        private void UpdateJigglypuffIdle(float dt)
        {
            if (_stateTimer >= IDLE_DURATION)
            {
                _currentState = State.JigglypuffHopLeftRight;
                _stateTimer = 0f;
                _hopCount = 0;
                _hopTimer = 0f;
            }
        }

        private void UpdateJigglypuffHopLeftRight(float dt)
        {
            _hopTimer += dt;

            if (_hopTimer >= HOP_DURATION && _hopCount < TOTAL_HOPS)
            {
                _hopCount++;
                _hopTimer = 0f;
                SoundEffectPlayer.Play(SfxId.SFX_INTRO_HOP);
            }

            if (_hopCount < TOTAL_HOPS)
            {
                float hopProgress = _hopTimer / HOP_DURATION;
                float horizontalMove = (_hopCount == 0) 
                    ? HOP_LEFT_RIGHT_HORIZONTAL - (HOP_LEFT_RIGHT_HORIZONTAL * hopProgress)
                    : HOP_LEFT_RIGHT_HORIZONTAL * hopProgress;

                float verticalArc = (float)Math.Sin(hopProgress * Math.PI) * HOP_LEFT_RIGHT_VERTICAL;

                _jigglypuffPosition = new Vector2(
                    _jigglypuffIdlePosition.X + horizontalMove,
                    _jigglypuffIdlePosition.Y - verticalArc
                );
            }

            if (_stateTimer >= HOP_LEFT_RIGHT_DURATION)
            {
                _jigglypuffPosition = new Vector2(
                    _jigglypuffIdlePosition.X + HOP_LEFT_RIGHT_HORIZONTAL,
                    _jigglypuffIdlePosition.Y
                );
                _currentState = State.JigglypuffPrepareAttack;
                _stateTimer = 0f;
            }
        }

        private void UpdateJigglypuffPrepareAttack(float dt)
        {
            if (_stateTimer >= PREPARE_ATTACK_DURATION)
            {
                _currentState = State.JigglypuffJumpAttack;
                _stateTimer = 0f;
            }
        }

        private void UpdateGengarReturn(float dt)
        {
            float progress = _stateTimer / RETURN_DURATION;
            _gengarPosition = Vector2.Lerp(_gengarLungeForwardPosition, _gengarIdlePosition, progress);

            if (_stateTimer >= RETURN_DURATION)
            {
                _gengarPosition = _gengarIdlePosition;
                _currentState = State.JigglypuffHopLeftRight;
                _stateTimer = 0f;
                _hopCount = 0;
                _hopTimer = 0f;
            }
        }

        private void UpdateJigglypuffJumpAttack(float dt)
        {
            if (!_hasPlayedAttackSound)
            {
                SoundEffectPlayer.Play(SfxId.SFX_INTRO_CRASH);
                _hasPlayedAttackSound = true;
            }

            float progress = _stateTimer / JUMP_ATTACK_DURATION;

            if (progress < 1f)
            {
                Vector2 startPos = new Vector2(
                    _jigglypuffIdlePosition.X + HOP_LEFT_RIGHT_HORIZONTAL,
                    _jigglypuffIdlePosition.Y
                );

                Vector2 endPos = new Vector2(
                    startPos.X - JUMP_ATTACK_HORIZONTAL,
                    startPos.Y - JUMP_ATTACK_VERTICAL
                );

                _jigglypuffPosition = Vector2.Lerp(startPos, endPos, progress);
            }
            else
            {
                _currentState = State.Complete;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Sprite blankScreen, Vector2 screenPos, float scale)
        {
            float gameScreenWidth = GAMEBOY_WIDTH * scale;
            float extraSpace = (WINDOW_WIDTH - gameScreenWidth) / 2;

            // Draw left border
            spriteBatch.Draw(
                blankScreen.Region.Texture,
                new Rectangle(0, (int)screenPos.Y, (int)extraSpace, (int)(GAMEBOY_HEIGHT * scale)),
                new Rectangle(blankScreen.Region.SourceRectangle.X, blankScreen.Region.SourceRectangle.Y, 1, (int)GAMEBOY_HEIGHT),
                Color.White
            );

            // Draw center
            blankScreen.Draw(spriteBatch, Color.White, screenPos, scale);

            // Draw right border
            spriteBatch.Draw(
                blankScreen.Region.Texture,
                new Rectangle((int)(screenPos.X + gameScreenWidth), (int)screenPos.Y, (int)extraSpace, (int)(GAMEBOY_HEIGHT * scale)),
                new Rectangle(blankScreen.Region.SourceRectangle.Right - 1, blankScreen.Region.SourceRectangle.Y, 1, (int)GAMEBOY_HEIGHT),
                Color.White
            );

            Sprite gengarSprite = _gengarIdle;
            Sprite jigglypuffSprite = _jigglypuffIdle;

            switch (_currentState)
            {
                case State.GengarMoveLeftArmRaised:
                    gengarSprite = _gengarArmRaised;
                    break;
                case State.GengarLunge:
                case State.JigglypuffHopAwayRight:
                case State.GengarReturn:
                    gengarSprite = _gengarLunge;
                    break;
            }

            if (_currentState == State.JigglypuffPrepareAttack || _jigglypuffIsPreparingAttack
                || _currentState == State.JigglypuffHopAwayRight || _currentState == State.JigglypuffHopLeftRight)
            {
                jigglypuffSprite = _jigglypuffHop;
            }

            if (_currentState == State.JigglypuffJumpAttack)
            {
                jigglypuffSprite = _jigglypuffJumpAttack;
            }

            bool gengarBehind = _gengarPosition.Y > _jigglypuffPosition.Y;

            if (gengarBehind)
            {
                gengarSprite.Draw(spriteBatch, Color.White, _gengarPosition, scale);
                jigglypuffSprite.Draw(spriteBatch, Color.White, _jigglypuffPosition, scale);
            }
            else
            {
                jigglypuffSprite.Draw(spriteBatch, Color.White, _jigglypuffPosition, scale);
                gengarSprite.Draw(spriteBatch, Color.White, _gengarPosition, scale);
            }
        }
    }
}