using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using Enter.Interfaces;
using Enter.Classes.Characters;

namespace Enter.Classes.Scenes
{
    /// <summary>
    /// Manages the active game scene, handling updates, drawing, and transitions.
    /// This is the main "state machine" for your game's high-level states.
    /// </summary>
    public class SceneManager
    {
        private IGameScene _currentScene;
        public IGameScene CurrentScene => _currentScene;
        private readonly Dictionary<string, IGameScene> _scenes = new Dictionary<string, IGameScene>();
        private string _currentSceneName = null;
        public string PreviousSceneName { get; private set; } = null;
        private readonly ContentManager _content;
        private bool _isTransitioning = false;
        private SpriteBatch _spriteBatch;
        // profiling stopwatches
        private readonly Stopwatch _updateSw = new Stopwatch();
        private readonly Stopwatch _drawSw = new Stopwatch();

        /// <summary>
        /// Last measured time spent in scene Update (milliseconds).
        /// </summary>
        public double LastUpdateMs { get; private set; } = 0.0;

        /// <summary>
        /// Last measured time spent in scene Draw (milliseconds).
        /// </summary>
        public double LastDrawMs { get; private set; } = 0.0;

        public SceneManager(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Adds a new scene to the manager.
        /// </summary>
        public void AddScene(string name, IGameScene scene)
        {
            _scenes[name] = scene;
        }

        /// <summary>
        /// Unloads the current scene, loads the new one, and makes it active.
        /// </summary>
        public void TransitionTo(string sceneName)
        {
            // Prevent transitioning to a scene that doesn't exist or while already transitioning
            if (_isTransitioning || !_scenes.ContainsKey(sceneName))
                return;

            _isTransitioning = true;

            // Track previous scene name
            PreviousSceneName = _currentSceneName;
            _currentSceneName = sceneName;

            // Load and set the new scene
            _currentScene = _scenes[sceneName];
            _currentScene.LoadContent(_content);

            _isTransitioning = false;
        }

        /// <summary>
        /// Called from Game1.Update. Updates the active scene.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (!_isTransitioning && _currentScene != null)
            {
                _updateSw.Restart();
                _currentScene.Update(gameTime);
                _updateSw.Stop();
                LastUpdateMs = _updateSw.Elapsed.TotalMilliseconds;
            }
            else
            {
                LastUpdateMs = 0.0;
            }
        }

        /// <summary>
        /// Called from Game1.Draw. Draws the active scene.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isTransitioning && _currentScene != null)
            {
                _drawSw.Restart();
                _currentScene.Draw(spriteBatch);
                _drawSw.Stop();
                LastDrawMs = _drawSw.Elapsed.TotalMilliseconds;
            }
            else
            {
                LastDrawMs = 0.0;
            }
        }
    }
}
