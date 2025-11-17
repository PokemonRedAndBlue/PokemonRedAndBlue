using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For example input
using Enter.Classes.Cameras;
using Enter.Classes.Characters;
using Enter.Classes.Input;
using Enter.Classes.Sprites;
using Enter.Classes.Physics;
using Enter.Interfaces;
using Enter.Classes.Textures;

namespace Enter.Classes.Scenes
{
    /// <summary>
    /// The scene for when the player is walking around in the world.
    /// Handles player movement, tilemap rendering, and collision.
    /// </summary>
    public class OverworldScene : IGameScene
    {
        private const float ZoomLevel = 4f; 
        private SceneManager _sceneManager;
        private Camera Cam;
        private KeyboardController _controller;
        private Texture2D character;
        private Trainer trainer;
        private Player _player;
        private Game1 _game;
        private Tilemap _currentMap;

        public Vector2 GetPlayerPosition() => _player.Position;

        // We must pass in the SceneManager so this scene can request transitions
        public OverworldScene(SceneManager sceneManager, Game1 game1, KeyboardController controller, Player player)
        {
            _sceneManager = sceneManager;
            _game = game1;
            _controller = controller;
            _player = player;
        }

        public void LoadContent(ContentManager content)
        {
            // Load tilemap, player sprites, NPCs, etc.
            Cam = new(((Game)_game).GraphicsDevice.Viewport);
            
            // if ((_game as Game1)?.SavedPlayerPosition is Microsoft.Xna.Framework.Vector2 savedPos)
            // {
            //     _player.Position = savedPos;
            // }

            Cam.Update(_player);
            Cam.Zoom = ZoomLevel; //Zoom level of world
            // Create trainer with specific ID that matches what's used in TrainerBattle
            character = content.Load<Texture2D>("images/Pokemon_Characters");
            const string TRAINER_ID = "youngster"; // This should match the ID used in Game1's AddScene
            trainer = new Trainer(
                character,
                new Vector2(_game.Window.ClientBounds.Height, _game.Window.ClientBounds.Width) * 0.25f,
                Facing.Right,
                false,  // not moving by default
                TRAINER_ID
            );
            _currentMap = TilemapLoader.LoadTilemap("Content/Route1Map.xml");

            // Collision wiring (minimal)
            _player.Map = _currentMap;

            // Build the solid tile index set from the "Ground" layer
            _player.SolidTiles = Physics.Collision.BuildSolidIndexSet(
                _currentMap,
                "Ground",
                Physics.SolidTileCollision.IsSolid
            );

            // * TEMP: Initialize player tile position (spawn at same 160px,0px => tile (10,0))
            _player.SetTilePosition(new Point(10, 0));
            Cam.Update(_player);
        }

        public void Update(GameTime gameTime)
        {
            // Update Objects
            _controller.Update(_game, gameTime, Cam, _player, trainer);
            Cam.Update(_player);

            // Handle trainer interactions
            if (trainer.colided && !trainer.IsApproachingPlayer)
            {
                if (!_game.IsTrainerDefeated(trainer.TrainerID))
                {
                    // Save position and transition to battle if trainer isn't defeated
                    _game.SavedPlayerPosition = _player.Position;
                    _sceneManager.TransitionTo("trainer");
                }
                // If trainer is defeated, they're just interacting without battle
                // Could show dialogue here
            }

            // check for wild encounter key
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                // Save player position before entering wild battle
                _game.SavedPlayerPosition = _player.Position;
                _sceneManager.TransitionTo("wild");
            }
            // no need for base.Update here
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black); // Overworld color

            // map & world entities affected by camera movement
            spriteBatch.Begin(transformMatrix: Cam.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            _currentMap?.DrawCropped(Cam.VisibleWorldRect);
            _player.Draw(spriteBatch);
            trainer.Draw(spriteBatch);
            spriteBatch.End();

            // no need for base.Draw here
        }


    }
}

