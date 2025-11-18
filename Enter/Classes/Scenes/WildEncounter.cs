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
        private Game _game;
        private WildEncounterUI wildUI;
        private TextureAtlas _UIAtlas;
        private TextSprite trainerText;
        private SpriteFont _font;
        private Player _player;
        public WildEncounter(SceneManager sceneManager, Game game1, Player player)
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
            trainerText = new TextSprite($"WILD ENCOUNTER", _font, Color.Black);

            // 1. FIX: Load the atlas first, so _UIAtlas is not null
            _UIAtlas = TextureAtlas.FromFile(content, "BattleInterface.xml"); 
            
            // 2. FIX: Remove "WildEncounterUI" to assign to the class field
            wildUI = new WildEncounterUI(_UIAtlas, content, _player); 
            wildUI.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {

            // Update the UI first to get user input
            wildUI.Update(gameTime);

            // Get current battle state decided by UI
            string state = wildUI.battleUI.getBattleState();

            switch (state)
            {
                case "Initial":
                    // Wait for user to choose an action
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        // Transition to Menu state
                        wildUI.battleUI.setBattleState("Menu");
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

                if(wildUI.didRunOrCatch || wildUI.resetBattle){
                    // Save the player's last position for the overworld
                    if ((_game as Game1)?.SavedPlayerPosition is Microsoft.Xna.Framework.Vector2 savedPos)
                    {
                        Enter.Classes.Scenes.OverworldScene.SetNextSpawn(savedPos);
                    }
                    _sceneManager.TransitionTo("overworld");
                }

                // update UI
                wildUI.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(pokemonBackgroundColor);
            spriteBatch.Begin();
            // Draw UI elements
            wildUI.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
