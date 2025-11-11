using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For example input
using System;
using Enter.Classes.Animations;
using Enter.Classes.Sprites;
using Enter.Classes.Textures;
using Enter.Interfaces;
using System.CodeDom.Compiler;

namespace Enter.Classes.Scenes    
{
    /// <summary>
    /// The scene for a battle against a wild Pokémon.
    /// </summary>
    public class WildEncounter : IGameScene
    {
        private SceneManager _sceneManager;
        private String _wildPokemonID;
        private Vector2 _enemyPokemonPosition = new Vector2(800, 200);
        private Vector2 _playerPokemonPosition = new Vector2(400, 400);
        private Game _game;
        private TextureAtlas _PokemonBackAtlas;
        private TextureAtlas _PokemonFrontAtlas;
        private TextureAtlas _UIAtlas;
        private TextSprite trainerText;
        private SpriteFont _font;
        private Sprite _playerPokemon;
        private AnimatedSprite _enemyPokemon;
        public WildEncounter(SceneManager sceneManager, Game game1)
        {
            _sceneManager = sceneManager;
            _game = game1;
        }

        public void LoadContent(ContentManager content)
        {
            // Load Pokemon Textures
            PokemonFrontFactory.Instance.LoadAllTextures(content);
            PokemonBackFactory.Instance.LoadAllTextures(content);
            _PokemonBackAtlas = TextureAtlas.FromFile(content, "Pokemon_BACK.xml");
            _PokemonFrontAtlas = TextureAtlas.FromFile(content, "Pokemon_FRONT.xml");

            // Load UI Textures
            UIFactory.Instance.LoadAllTextures(content, "UI.xml");
            _UIAtlas = UIFactory.Instance.FromFile(content);

            // create UI elements
            Sprite[] sprites = new Sprite[_UIAtlas._regions.Count];
            int index = 0;
            foreach (var sprite in _UIAtlas._regions)
            {
                // Example: Create UI sprites as needed
                var uiSprite = _UIAtlas.CreateSprite(sprite.Key);
                sprites[index++] = uiSprite;
            }

            // Load Trainer and their Pokemon
            _wildPokemonID = PokemonGenerator.GenerateRandom().Species.Name.ToString();
            _enemyPokemon = PokemonFrontFactory.Instance.CreateAnimatedSprite(_wildPokemonID.ToLower() + "-front");
            _playerPokemon = PokemonBackFactory.Instance.CreateStaticSprite("squirtle-back");

            // Load UI
            _font = content.Load<SpriteFont>("PokemonFont");
            trainerText = new TextSprite($"WILD ENCOUNTER", _font, Color.Black);
        }

        public void Update(GameTime gameTime)
        {
            // --- Wild Encounter Logic ---
            // Similar to trainer battle, but with different rules:
            // - Can run
            // - Can catch Pokemon
            // - Trainer may switch Pokemon
            
            // --- Transition Logic ---
            // if (PlayerWon || Fainted)
            // {
                if (Keyboard.GetState().IsKeyDown(Keys.Tab)) // Placeholder for battle end condition
                {
                    // The saved position from before entering battle will be automatically
                    // restored by OverworldScene.LoadContent using Game1.SavedPlayerPosition
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
