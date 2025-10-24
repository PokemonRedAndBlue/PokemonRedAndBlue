using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PokemonGame.Scenes
{
    /// <summary>
    /// Manages the active game scene, handling updates, drawing, and transitions.
    /// This is the main "state machine" for your game's high-level states.
    /// </summary>
    public class SceneManager
    {
        private IGameScene _currentScene;
        private readonly Dictionary<string, IGameScene> _scenes = new Dictionary<string, IGameScene>();
        private readonly ContentManager _content;
        private bool _isTransitioning = false;

        private SpriteBatch _spriteBatch;

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
                _currentScene.Update(gameTime);
            }
        }

        /// <summary>
        /// Called from Game1.Draw. Draws the active scene.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isTransitioning && _currentScene != null)
            {
                _currentScene.Draw(spriteBatch);
            }
        }
    }
}
