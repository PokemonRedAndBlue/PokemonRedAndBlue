using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For example input

namespace PokemonGame.Scenes
{
    /// <summary>
    /// The scene for when the player is walking around in the world.
    /// Handles player movement, tilemap rendering, and collision.
    /// </summary>
    public class OverworldScene : IGameScene
    {
        private SceneManager _sceneManager;
        private SpriteFont _font; // Placeholder for UI/debug text
        // private Player _player;
        // private Tilemap _tilemap;

        // We must pass in the SceneManager so this scene can request transitions
        public OverworldScene(SceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }

        public void LoadContent(ContentManager content)
        {
            // Load your tilemap, player sprites, NPCs, etc.
            // _font = content.Load<SpriteFont>("fonts/DebugFont");
            // _player = new Player(content.Load<Texture2D>("sprites/player"));
            // _tilemap = new Tilemap(content.Load<Texture2D>("tiles/overworld_tiles"));
        }

        public void UnloadContent()
        {
            // Unload scene-specific content if necessary.
        }

        public void Update(GameTime gameTime)
        {
            // --- Player Movement Logic ---
            // _player.Update(gameTime, _tilemap); // Example of player update

            // --- Collision & Event Logic ---
            // Check for tile collisions (e.g., tall grass)
            // if (_tilemap.OnGrassTile(_player.Position) && _player.IsMoving)
            // {
            //     if (new System.Random().Next(0, 100) < 5) // 5% chance
            //     {
            //         _sceneManager.TransitionTo("wildBattle");
            //     }
            // }

            // --- Example: Force a battle with a key press ---
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                // This call switches the entire game state!
                _sceneManager.TransitionTo("wildBattle");
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                // Example of starting a specific trainer battle
                _sceneManager.TransitionTo("trainerBattle_Brock");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue); // Overworld color
            
            // Draw order matters: tilemap first, then player/NPCs
            // _tilemap.Draw(spriteBatch);
            // _player.Draw(spriteBatch);
            
            // _font.DrawString(spriteBatch, "OVERWORLD", ...);
        }
    }
}

