using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using Enter.Classes.Animations;
using System.Collections.Generic;

namespace Enter.Classes.Scenes.IntroAnimations
{
    /// <summary>
    /// The GAMEFREAK animation component for the title screen sequence
    /// </summary>
    public class GameFreakAnimation
    {
        private enum State
        {
            BlackBlack,
            StarSweep,
            YellowBlack,
            DarkYellowYellow,
            BlackDarkYellow,
            FallingStars,
            Complete
        }
        
        // ============================================================
        // DISPLAY CONSTANTS
        // ============================================================
        private const float WINDOW_WIDTH = 1280f;
        private const int GAMEBOY_WIDTH = 160;
        private const int GAMEBOY_HEIGHT = 144;
        
        // ============================================================
        // TIMING CONSTANTS - ADJUST THESE TO CHANGE ANIMATION SPEED (seconds)
        // ============================================================
        private const float BLACK_BLACK_DURATION = 0.5f;           // Initial black logo
        private const float STAR_SWEEP_DURATION = 1.5f;            // Star moving across screen
        private const float YELLOW_BLACK_DURATION = 0.3f;          // Text turns yellow
        private const float DARKYELLOW_YELLOW_DURATION = 0.3f;     // Text dark yellow, symbol yellow
        private const float BLACK_DARKYELLOW_DURATION = 0.3f;      // Text black, symbol dark yellow
        private const float FALLING_STARS_DURATION = 3.5f;         // Stars falling animation
        
        // ============================================================
        // STAR ANIMATION CONSTANTS
        // ============================================================
        private const float STAR_SWEEP_START_OFFSET = 32f;
        private const float FALLING_STAR_START_Y = 90f;            // Game Boy Y coordinate
        private const float FALLING_STAR_VELOCITY = 25f;           // Pixels per second
        private const float BLACK_BAR_Y = 105f;                    // Game Boy Y coordinate where stars hide
        private const float STAR_BLINK_INTERVAL = 0.2f;            // Seconds between blinks
        private const int GAMEBOY_SCALE = 5;                       // Scale multiplier for Game Boy coordinates
        
        private State _currentState = State.BlackBlack;
        private float _stateTimer = 0f;
        
        // Full screen frame sprites 
        private Sprite _gameFreakBlackBlack;
        private Sprite _gameFreakYellowBlack;
        private Sprite _gameFreakDarkYellowYellow;
        private Sprite _gameFreakBlackDarkYellow;
        private AnimatedSprite _sweepStar;
        
        // Falling stars
        private List<FallingStar> _fallingStars = new List<FallingStar>();
        private TextureAtlas _atlas;
        
        // Star sweep
        private Vector2 _starPosition;
        private Vector2 _starStartPos;
        private Vector2 _starEndPos;
        
        // Screen dimensions
        private Rectangle _screenBounds;
        
        public bool IsComplete => _currentState == State.Complete;
        
        public GameFreakAnimation(TextureAtlas atlas, Rectangle screenBounds)
        {
            _atlas = atlas;
            _screenBounds = screenBounds;
            
            // Load full screen frame sprites
            _gameFreakBlackBlack = atlas.CreateSprite("gamefreak_black_black");
            _gameFreakYellowBlack = atlas.CreateSprite("gamefreak_yellow_black");
            _gameFreakDarkYellowYellow = atlas.CreateSprite("gamefreak_darkyellow_yellow");
            _gameFreakBlackDarkYellow = atlas.CreateSprite("gamefreak_black_darkyellow");
            
            _sweepStar = atlas.CreateAnimatedSprite("sweep_star_cycle");
            
            // Setup star sweep path (top-right to bottom-left)
            _starStartPos = new Vector2(
                screenBounds.Width + STAR_SWEEP_START_OFFSET, 
                -STAR_SWEEP_START_OFFSET
            );
            _starEndPos = new Vector2(
                -STAR_SWEEP_START_OFFSET, 
                screenBounds.Height + STAR_SWEEP_START_OFFSET
            );
            _starPosition = _starStartPos;
            
            InitializeFallingStars();
        }
        
        private void InitializeFallingStars()
        {
            string[] starColors = new[] { 
                "falling_star_red_orange",
                "falling_star_orange", 
                "falling_star_yellow", 
                "falling_star_yellow_darkyellow",
                "falling_star_lightblue",
                "falling_star_lightblue_darkblue",
                "falling_star_green",
                "falling_star_blue_green"
            };
            
            float[] xPositions = { 20, 40, 60, 80, 100, 120, 30, 50, 70, 90, 110, 130 };
            bool[] shouldBlink = { true, false, true, false, true, false, false, true, false, true, false, true };
            
            for (int i = 0; i < xPositions.Length; i++)
            {
                var star = new FallingStar
                {
                    Sprite = _atlas.CreateSprite(starColors[i % starColors.Length]),
                    Position = new Vector2(
                        _screenBounds.X + (xPositions[i] * GAMEBOY_SCALE),
                        _screenBounds.Y + (FALLING_STAR_START_Y * GAMEBOY_SCALE)
                    ),
                    Velocity = FALLING_STAR_VELOCITY,
                    ShouldBlink = shouldBlink[i],
                    BlinkInterval = STAR_BLINK_INTERVAL,
                    BlinkTimer = 0f,
                    IsVisible = true
                };
                _fallingStars.Add(star);
            }
        }
        
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _stateTimer += dt;
            
            switch (_currentState)
            {
                case State.BlackBlack:
                    if (_stateTimer >= BLACK_BLACK_DURATION)
                    {
                        _currentState = State.StarSweep;
                        _stateTimer = 0f;
                        _starPosition = _starStartPos;
                    }
                    break;
                    
                case State.StarSweep:
                    UpdateStarSweep(gameTime, dt);
                    break;
                    
                case State.YellowBlack:
                    if (_stateTimer >= YELLOW_BLACK_DURATION)
                    {
                        _currentState = State.DarkYellowYellow;
                        _stateTimer = 0f;
                    }
                    break;
                    
                case State.DarkYellowYellow:
                    if (_stateTimer >= DARKYELLOW_YELLOW_DURATION)
                    {
                        _currentState = State.BlackDarkYellow;
                        _stateTimer = 0f;
                    }
                    break;
                    
                case State.BlackDarkYellow:
                    if (_stateTimer >= BLACK_DARKYELLOW_DURATION)
                    {
                        _currentState = State.FallingStars;
                        _stateTimer = 0f;
                    }
                    break;
                    
                case State.FallingStars:
                    UpdateFallingStars(dt);
                    if (_stateTimer >= FALLING_STARS_DURATION)
                    {
                        _currentState = State.Complete;
                    }
                    break;
            }
        }
        
        private void UpdateStarSweep(GameTime gameTime, float dt)
        {
            _sweepStar.Update(gameTime);
            
            // Move star diagonally
            float progress = _stateTimer / STAR_SWEEP_DURATION;
            _starPosition = Vector2.Lerp(_starStartPos, _starEndPos, progress);
            
            if (_stateTimer >= STAR_SWEEP_DURATION)
            {
                _currentState = State.YellowBlack;
                _stateTimer = 0f;
            }
        }
        
        private void UpdateFallingStars(float dt)
        {
            float blackBarY = _screenBounds.Y + (BLACK_BAR_Y * GAMEBOY_SCALE);
            
            foreach (var star in _fallingStars)
            {
                // Move star down
                star.Position = new Vector2(
                    star.Position.X, 
                    star.Position.Y + star.Velocity * dt
                );
                
                // Hide stars when they enter the bottom black bar
                if (star.Position.Y >= blackBarY)
                {
                    star.IsVisible = false;
                    continue; 
                }
                
                if (star.ShouldBlink)
                {
                    star.BlinkTimer += dt;
                    if (star.BlinkTimer >= star.BlinkInterval)
                    {
                        star.IsVisible = !star.IsVisible;
                        star.BlinkTimer = 0f;
                    }
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, Vector2 screenPos, float scale)
        {
            Sprite currentFrame = null;
            
            switch (_currentState)
            {
                case State.BlackBlack:
                case State.StarSweep:
                    currentFrame = _gameFreakBlackBlack;
                    break;
                    
                case State.YellowBlack:
                    currentFrame = _gameFreakYellowBlack;
                    break;
                    
                case State.DarkYellowYellow:
                    currentFrame = _gameFreakDarkYellowYellow;
                    break;
                    
                case State.BlackDarkYellow:
                case State.FallingStars:
                    currentFrame = _gameFreakBlackDarkYellow;
                    break;
            }
            
            if (currentFrame != null)
            {
                float gameScreenWidth = GAMEBOY_WIDTH * scale; 
                float extraSpace = (WINDOW_WIDTH - gameScreenWidth) / 2; 
                
                // Draw left border
                spriteBatch.Draw(
                    currentFrame.Region.Texture,
                    new Rectangle(0, (int)screenPos.Y, (int)extraSpace, (int)(GAMEBOY_HEIGHT * scale)),
                    new Rectangle(currentFrame.Region.SourceRectangle.X, currentFrame.Region.SourceRectangle.Y, 1, GAMEBOY_HEIGHT),
                    Color.White
                );
                
                // Draw center (actual frame)
                currentFrame.Draw(spriteBatch, Color.White, screenPos, scale);
                
                // Draw right border
                spriteBatch.Draw(
                    currentFrame.Region.Texture,
                    new Rectangle((int)(screenPos.X + gameScreenWidth), (int)screenPos.Y, (int)extraSpace, (int)(GAMEBOY_HEIGHT * scale)),
                    new Rectangle(currentFrame.Region.SourceRectangle.Right - 1, currentFrame.Region.SourceRectangle.Y, 1, GAMEBOY_HEIGHT),
                    Color.White
                );
            }
            
            // Draw sweeping star on top
            if (_currentState == State.StarSweep)
            {
                _sweepStar.Draw(spriteBatch, Color.White, _starPosition, scale);
            }
            
            // Draw falling stars on top
            if (_currentState == State.FallingStars)
            {
                foreach (var star in _fallingStars)
                {
                    if (star.IsVisible)
                    {
                        star.Sprite.Draw(spriteBatch, Color.White, star.Position, scale);
                    }
                }
            }
        }
        
        private class FallingStar
        {
            public Sprite Sprite { get; set; }
            public Vector2 Position { get; set; }
            public float Velocity { get; set; }
            public bool ShouldBlink { get; set; }
            public float BlinkInterval { get; set; }
            public float BlinkTimer { get; set; }
            public bool IsVisible { get; set; }
        }
    }
}