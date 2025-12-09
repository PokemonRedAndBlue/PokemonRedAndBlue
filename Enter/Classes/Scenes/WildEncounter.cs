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
using Enter.Classes.Characters;

namespace Enter.Classes.Scenes    
{
    /// <summary>
    /// The scene for a battle against a wild Pokémon.
    /// </summary>
    public class WildEncounter : IGameScene
    {
        private Color pokemonBackgroundColor = new Color(246, 232, 248);
        private SceneManager _sceneManager;
        private String _wildPokemonID;
        private Game1 _game;
        private WildEncounterUI _wildUI;
        private TextureAtlas _UIAtlas;
        private TextureAtlas _BattleCharactersAtlas;
        private TextureAtlas _BordersAtlas;
        private TextSprite _trainerText;
        private SpriteFont _font;
        private Player _player;
        public WildEncounter(SceneManager sceneManager, Game1 game1, Player player)
        {
            _sceneManager = sceneManager;
            _game = game1;
            _wildPokemonID = PokemonGenerator.GenerateWildPokemon().Species.Name.ToLower(); // Example: "bulbasaur"
            _player = player;
        }

        public void LoadContent(ContentManager content)
        {
            // Load UI
            _font = content.Load<SpriteFont>("PokemonFont");
            _trainerText = new TextSprite($"WILD ENCOUNTER", _font, Color.Black);
            
            // Load Background Music 
            BackgroundMusicLibrary.Load(content);
            BackgroundMusicPlayer.Play(SongId.BattleWildPokemon, loop: true);

            // Load all required atlases
            _UIAtlas = TextureAtlas.FromFile(content, "BattleInterface.xml");
            _BattleCharactersAtlas = TextureAtlas.FromFile(content, "BattleChars.xml");
            _BordersAtlas = TextureAtlas.FromFile(content, "Borders.xml");
            
            // Create WildEncounterUI with all three atlases
            _wildUI = new WildEncounterUI(_UIAtlas, _BattleCharactersAtlas, _BordersAtlas, content, _player);
            _wildUI.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {

            // Update the UI first to get user input
            _wildUI.Update(gameTime);

            // Get current battle state decided by UI
            string state = _wildUI.battleUI.getBattleState();

            switch (state)
            {
                case "Initial":
                    // Wait for user to choose an action
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        // Transition to Menu state
                        _wildUI.battleUI.setBattleState("Menu");
                    }
                    break;
                case "Menu":
                    // UI handles menu navigation
                    break;
                case "Bag":
                    // Handle bag logic
                    break;
                case "Pokemon":
                    // Handle Pokemon switch logic
                    break;
                case "Run":
                    // Handle run logic
                    break;
                default:
                    break;
            }
            // --- Wild Encounter Logic ---
            // Similar to trainer battle, but with different rules:
            // - Can run
            // - Can catch Pokemon
            // - Trainer may switch Pokemon
            
            // --- Transition Logic ---
            // if (PlayerWon || Fainted)

                if(_wildUI.didRunOrCatch || _wildUI.resetBattle){
                    // Save the player's last position for the overworld
                    if (_game?.SavedPlayerPosition is Vector2 savedPosVec)
                    {
                        Point savedPos = new Point((int)savedPosVec.X, (int)savedPosVec.Y);
                        OverworldScene.SetNextSpawn(savedPos);
                    }
                    _sceneManager.TransitionTo("overworld");
                }

            // UI already updated above
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(pokemonBackgroundColor);
            spriteBatch.Begin();
            // Draw UI elements
            _wildUI.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
