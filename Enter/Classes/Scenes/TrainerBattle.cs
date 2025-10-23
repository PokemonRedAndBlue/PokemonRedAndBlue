using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For example input
using Enter.Classes.Animations;
using Enter.Classes.Characters;
using Enter.Classes.Sprites;
using Enter.Classes.GameState;

namespace PokemonGame.Scenes    
{
    /// <summary>
    /// The scene for a battle against a Trainer.
    /// </summary>
    public class TrainerBattleScene : IGameScene
    {
        private SceneManager _sceneManager;
        private string _trainerID;
        private Trainer _trainer;
        private Pokemon _playerPokemon;
        private Pokemon _enemyPokemon;
        private Vector2 _playerPokemonPosition = new Vector2(800, 400);
        private Vector2 _enemyPokemonPosition = new Vector2(400, 200);

        private TextSprite trainerText;
        private SpriteFont _font;
        public TrainerBattleScene(SceneManager sceneManager, string trainerID)
        {
            _sceneManager = sceneManager;
            _trainerID = trainerID; // e.g., "TRAINER_BROCK"
        }

        public void LoadContent(ContentManager content)
        {
            PokemonBackFactory.Instance.LoadAllTextures(content);
            PokemonFrontFactory.Instance.LoadAllTextures(content);

            // Load Trainer and their Pokemon
            Sprite enemySprite = PokemonFrontFactory.Instance.CreateAnimatedSprite("bulbasaur-front");
            Sprite playerSprite = PokemonBackFactory.Instance.CreateStaticSprite("charmander-back");
            _enemyPokemon = new Pokemon("bulbasaur", 1, PokemonView.Front, enemySprite, _enemyPokemonPosition);
            _playerPokemon = new Pokemon("charmander", 1, PokemonView.Back, playerSprite, _playerPokemonPosition);

            // Load UI
            _font = content.Load<SpriteFont>("fonts/PokemonFont");
            trainerText = new TextSprite($"TRAINER BATTLE: {_trainerID}", _font, Color.Black);
        }

        public void Update(GameTime gameTime)
        {
            // --- Battle Logic ---
            // Similar to wild battle, but with different rules:
            // - Cannot run
            // - Cannot catch Pokemon
            // - Trainer may switch Pokemon
            
            // --- Transition Logic ---
            // if (PlayerWon || TrainerWon)
            // {
                 if (Keyboard.GetState().IsKeyDown(Keys.Tab)) // Placeholder for battle end condition
                 {
                     _sceneManager.TransitionTo("overworld");
                 }
            // }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.White); // Trainer battle color
            
            // Draw Pokemon, health bars, menus
            _enemyPokemon.AnimatedSprite.Draw(spriteBatch, Color.White,_enemyPokemonPosition, 4f);
            _playerPokemon.AnimatedSprite.Draw(spriteBatch, Color.White,_playerPokemonPosition, 4f);

            // Draw UI elements
            trainerText.Draw(spriteBatch, Color.White, new Vector2(100, 100), 4f);
        }
    }
}
