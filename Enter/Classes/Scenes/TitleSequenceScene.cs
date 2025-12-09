using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Enter.Interfaces;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using Enter.Classes.Scenes.IntroAnimations;
using PokemonGame;

namespace Enter.Classes.Scenes
{
    /// <summary>
    /// The title screen scene for Pokemon Red/Blue with full authentic intro sequence
    /// </summary>
    public class TitleSequenceScene : IGameScene
    {
        private enum TitleState
        {
            Copyright,
            GameFreakAnimation,
            BattleIntro,
            Complete
        }
        
        private SceneManager _sceneManager;
        private Game1 _game;
        
        // Assets
        private TextureAtlas _titleAtlas;
        
        // Sprites
        private Sprite _copyrightScreen;
        private Sprite _blankScreen;
        
        // Animation controllers
        private GameFreakAnimation _gameFreakAnim;
        private BattleIntroAnimation _battleIntroAnim;
        
        // Input handling
        private KeyboardState _previousKeyState;
        private KeyboardState _currentKeyState;
        
        // ============================================================
        // TIMING CONSTANTS - ADJUST THESE TO CHANGE ANIMATION SPEED
        // ============================================================
        // How long each screen lasts (in seconds):
        private const float COPYRIGHT_DURATION = 1.0f;    // Copyright screen
        private const float BLANK_DURATION = 0.3f;        // Blank screen between copyright and GAME FREAK
        // ============================================================
        
        // State management
        private TitleState _currentState = TitleState.Copyright;
        private float _stateTimer = 0f;
        
        // Screen scale - 5x fills a 1280x720 window nicely
        private const float SCREEN_SCALE = 5f; // Game Boy 160x144 scaled to 800x720
        private Vector2 _screenPosition;
        
        // Colors
        private Color _backgroundColor = Color.White;
        
        public TitleSequenceScene(SceneManager sceneManager, Game1 game)
        {
            _sceneManager = sceneManager;
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            // Load texture atlas
            _titleAtlas = TextureAtlas.FromFile(content, "TitleScreenSequence.xml");
            
            // Create static sprites
            _copyrightScreen = _titleAtlas.CreateSprite("copyright_screen");
            _blankScreen = _titleAtlas.CreateSprite("blank_screen");
            
            // Calculate screen position to center the Game Boy screen
            int screenWidth = _game.Window.ClientBounds.Width;
            int screenHeight = _game.Window.ClientBounds.Height;
            _screenPosition = new Vector2(
                (screenWidth - 160 * SCREEN_SCALE) / 2,
                (screenHeight - 144 * SCREEN_SCALE) / 2
            );
            
            // Create animation controllers
            Rectangle screenBounds = new Rectangle(
                (int)_screenPosition.X, 
                (int)_screenPosition.Y, 
                (int)(160 * SCREEN_SCALE), 
                (int)(144 * SCREEN_SCALE)
            );
            
            _gameFreakAnim = new GameFreakAnimation(_titleAtlas, screenBounds);
            _battleIntroAnim = new BattleIntroAnimation(_titleAtlas, screenBounds);
            
            // Start with copyright screen
            _currentState = TitleState.Copyright;
            _stateTimer = 0f;
            
            // Play title screen music (commented out if not loaded)
            // BackgroundMusicPlayer.Play(SongId.OpeningPart2);
            
            // Reset input states
            _previousKeyState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            _currentKeyState = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Check for restart key (R)
            if (IsKeyPressed(Keys.R))
            {
                RestartIntro();
            }
            
            switch (_currentState)
            {
                case TitleState.Copyright:
                    UpdateCopyright(deltaTime);
                    break;
                    
                case TitleState.GameFreakAnimation:
                    UpdateGameFreakAnimation(gameTime);
                    break;
                    
                case TitleState.BattleIntro:
                    UpdateBattleIntro(gameTime);
                    break;
            }
            
            _previousKeyState = _currentKeyState;
        }

        private void UpdateCopyright(float deltaTime)
        {
            _stateTimer += deltaTime;
            
            // Skip to next on any key press or after duration
            if (IsAnyKeyPressed() || _stateTimer >= COPYRIGHT_DURATION)
            {
                _currentState = TitleState.GameFreakAnimation;
                _stateTimer = 0f;
            }
        }

        private void UpdateBlankScreen(float deltaTime)
        {
            _stateTimer += deltaTime;
            
            if (IsAnyKeyPressed() || _stateTimer >= BLANK_DURATION)
            {
                _currentState = TitleState.GameFreakAnimation;
                _stateTimer = 0f;
            }
        }

        private void UpdateGameFreakAnimation(GameTime gameTime)
        {
            _gameFreakAnim.Update(gameTime);
            
            // Skip on any key press or when animation completes
            if (IsAnyKeyPressed() || _gameFreakAnim.IsComplete)
            {
                _currentState = TitleState.BattleIntro;
                _stateTimer = 0f;
            }
        }

        private void UpdateBattleIntro(GameTime gameTime)
        {
            _battleIntroAnim.Update(gameTime);
            
            // Skip on any key press or when animation completes
            if (IsAnyKeyPressed() || _battleIntroAnim.IsComplete)
            {
                _currentState = TitleState.Complete;
                _stateTimer = 0f;
                // Transition to actual title screen or game
                _sceneManager.TransitionTo("intro");
            }
        }

        private bool IsAnyKeyPressed()
        {
            var keys = _currentKeyState.GetPressedKeys();
            var prevKeys = _previousKeyState.GetPressedKeys();
            
            foreach (var key in keys)
            {
                // Don't count R key as "any key" since it's for restart
                if (key == Keys.R) continue;
                
                bool wasPressed = false;
                foreach (var prevKey in prevKeys)
                {
                    if (key == prevKey)
                    {
                        wasPressed = true;
                        break;
                    }
                }
                if (!wasPressed) return true;
            }
            return false;
        }
        
        private void RestartIntro()
        {
            _currentState = TitleState.Copyright;
            _stateTimer = 0f;
            
            // Recreate animation controllers to reset their state
            Rectangle screenBounds = new Rectangle(
                (int)_screenPosition.X, 
                (int)_screenPosition.Y, 
                (int)(160 * SCREEN_SCALE), 
                (int)(144 * SCREEN_SCALE)
            );
            
            _gameFreakAnim = new GameFreakAnimation(_titleAtlas, screenBounds);
            _battleIntroAnim = new BattleIntroAnimation(_titleAtlas, screenBounds);
        }

        private bool IsKeyPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(new Color(246, 232, 248));

            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            switch (_currentState)
            {
                case TitleState.Copyright:
                    _copyrightScreen.Draw(spriteBatch, Color.White, _screenPosition, SCREEN_SCALE);
                    break;
                    
                case TitleState.GameFreakAnimation:
                    _gameFreakAnim.Draw(spriteBatch, _screenPosition, SCREEN_SCALE);
                    break;
                    
                case TitleState.BattleIntro:
                    _battleIntroAnim.Draw(spriteBatch, _blankScreen, _screenPosition, SCREEN_SCALE);
                    break;
            }
            
            spriteBatch.End();
        }
    }
}