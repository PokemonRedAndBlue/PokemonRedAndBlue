using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For example input

namespace PokemonGame.Scenes
{
    /// <summary>
    /// The scene for a wild Pokemon battle.
    /// Handles the battle logic, Pokemon state machines, and UI.
    /// </summary>
    public class BattleScene : IGameScene
    {
        private SceneManager _sceneManager;
        private SpriteFont _font;
        // private Pokemon _playerPokemon;
        // private Pokemon _enemyPokemon;
        // private BattleUI _battleUI;

        public BattleScene(SceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }

        public void LoadContent(ContentManager content)
        {
            // Load Pokemon sprites, battle UI, health bars, etc.
            // _font = content.Load<SpriteFont>("fonts/DebugFont");
            // _enemyPokemon = PokemonFactory.CreateWildPokemon("Pikachu", 5);
            // _playerPokemon = GetPlayer'sFirstPokemon();
            // _battleUI = new BattleUI(content);
        }

        public void UnloadContent() 
        {
            // Unload scene-specific content if necessary
        }

        public void Update(GameTime gameTime)
        {
            // --- Battle Logic ---
            // This is where your battle state machine would live.
            // e.g., PlayerChooseMove -> PlayerAttack -> EnemyAttack -> EndTurn
            // _playerPokemon.StateMachine.Update(gameTime);
            // _enemyPokemon.StateMachine.Update(gameTime);
            // _battleUI.Update(gameTime);

            // --- Transition Logic ---
            // Simulate ending the battle
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                _sceneManager.TransitionTo("overworld");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateGray); // Battle color
            
            // Draw your Pokemon, health bars, menus
            // _enemyPokemon.Draw(spriteBatch);
            // _playerPokemon.Draw(spriteBatch);
            // _battleUI.Draw(spriteBatch);
            
            // _font.DrawString(spriteBatch, "WILD BATTLE!", ...);
        }
    }
}

