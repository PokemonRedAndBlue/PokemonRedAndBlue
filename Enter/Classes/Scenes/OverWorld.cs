using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For example input
using System;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.Behavior;
using Enter.Classes.Cameras;
using Enter.Classes.Characters;
using Enter.Classes.GameState;
using Enter.Classes.Input;
using Enter.Classes.Sprites;
using Enter.Classes.Textures;
using Enter;

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
        private Player _player;
        private Tilemap _tilemap;
        private GameWindow Window = Game1.Instance.Window;
        private Camera Cam;
        private Texture2D character;
        private Trainer trainer;
        private KeyboardController _controller;
        private Player player;
        private Game1 _game;
        private Tilemap _currentMap;

        // We must pass in the SceneManager so this scene can request transitions
        public OverworldScene(SceneManager sceneManager, Game1 game1, KeyboardController controller)
        {
            _sceneManager = sceneManager;
            _game = game1;
            _controller = controller;
        }

        public void LoadContent(ContentManager content)
        {
            // Load tilemap, player sprites, NPCs, etc.
            Cam = new(Window);
            character = content.Load<Texture2D>("images/Pokemon_Characters");
            player = new Player(character, Window);
            Cam.CenterOn(player.Position);
            trainer = new Trainer(
                character,
                new Vector2(Window.ClientBounds.Height, Window.ClientBounds.Width) * 0.25f,
                Facing.Right
            );
            _currentMap = TilemapLoader.LoadTilemap("Content/Route1Map.xml");
        }

        public void Update(GameTime gameTime)
        {
            // Update Objects
            _controller.Update(_game, gameTime, Cam, player, trainer);
            Cam.Update();

            // Force a battle with trainer interaction
            if (trainer.visible){
                // Example of starting a specific trainer battle
                _sceneManager.TransitionTo("trainerBattle_TESTER");
            }
            // no need for base.Update here
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black); // Overworld color

            // map & world entities affected by camera movement
            spriteBatch.Begin(transformMatrix: Cam.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            _currentMap?.DrawCropped(Cam.VisibleWorldRect, scale: 4f);
            player.Draw(spriteBatch, 4f);
            trainer.Draw(spriteBatch, 4f);
            spriteBatch.End();

            // no need for base.Draw here
        }
    }
}

