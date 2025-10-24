using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For example input
using System;
using Enter.Classes.Animations;
using Enter.Classes.Sprites;
using Enter.Classes.Textures;
using Enter.Interfaces;

namespace Enter.Classes.Scenes    
{
    /// <summary>
    /// The scene for a battle against a Trainer.
    /// </summary>
    public class TrainerBattleScene : IGameScene
    {
        private SceneManager _sceneManager;
        private String _trainerID;
        private Vector2 _enemyPokemonPosition = new Vector2(800, 200);
        private Vector2 _playerPokemonPosition = new Vector2(400, 400);
        private Game _game;
        private TextureAtlas _PokemonBackAtlas;
        private TextureAtlas _PokemonFrontAtlas;
        private TextSprite trainerText;
        private SpriteFont _font;
        private Sprite _playerPokemon;
        private AnimatedSprite _enemyPokemon;
        public TrainerBattleScene(SceneManager sceneManager, Game game1, string trainerID)
        {
            _sceneManager = sceneManager;
            _trainerID = trainerID; // e.g., "TRAINER_BROCK"
            _game = game1;
        }

        public void LoadContent(ContentManager content)
        {
            PokemonFrontFactory.Instance.LoadAllTextures(content);
            PokemonBackFactory.Instance.LoadAllTextures(content);
            _PokemonBackAtlas = TextureAtlas.FromFile(content, "Pokemon_BACK.xml");
            _PokemonFrontAtlas = TextureAtlas.FromFile(content, "Pokemon_FRONT.xml");

            // Load Trainer and their Pokemon
            _enemyPokemon = PokemonFrontFactory.Instance.CreateAnimatedSprite("bulbasaur-front");
            _playerPokemon = PokemonBackFactory.Instance.CreateStaticSprite("squirtle-back");

            // Load UI
            _font = content.Load<SpriteFont>("PokemonFont");
            trainerText = new TextSprite($"TRAINER BATTLE", _font, Color.Black);
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
            spriteBatch.Begin();
            // Draw Pokemon, health bars, menus
            spriteBatch.GraphicsDevice.Clear(Color.White); // Trainer battle color
            _enemyPokemon.Draw(spriteBatch, Color.White, _enemyPokemonPosition, 4f);
            _playerPokemon.Draw(spriteBatch, Color.White, _playerPokemonPosition, 4f);

            // Draw UI elements
            trainerText.DrawTextSprite(spriteBatch, new Vector2(100, 100));
            spriteBatch.End();
        }
    }
}
