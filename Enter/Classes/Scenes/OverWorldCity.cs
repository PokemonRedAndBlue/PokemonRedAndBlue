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

namespace Enter.Classes.Scenes
{
    /// <summary>
    /// The scene for when the player is walking around in the world.
    /// Handles player movement, tilemap rendering, and collision.
    /// </summary>
    public class OverworldCityScene : IGameScene
    {
        private const float ZoomLevel = 4f; 
        private SceneManager _sceneManager;
        private SpriteFont _font; // Placeholder for UI/debug text
        private Tilemap _tilemap;
        private Camera Cam;
        private KeyboardController _controller;
        private Texture2D character;
        private Trainer trainer;
        private Player player;
        private Game1 _game;
        private Tilemap _currentMap;
        private Player _player;

        // We must pass in the SceneManager so this scene can request transitions
        public OverworldCityScene(SceneManager sceneManager, Game1 game1, KeyboardController controller, Player p)
        {
            _sceneManager = sceneManager;
            _game = game1;
            _controller = controller;
            _player = p;
        }

        public void LoadContent(ContentManager content)
        {
            // Load tilemap, player sprites, NPCs, etc.
            Cam = new(((Game)_game).GraphicsDevice.Viewport);
            character = content.Load<Texture2D>("images/Pokemon_Characters");
            Cam.Update(player);
            Cam.Zoom = ZoomLevel; //Zoom leve of world
            trainer = new Trainer(
                character,
                new Vector2(_game.Window.ClientBounds.Height, _game.Window.ClientBounds.Width) * 0.25f,
                Facing.Right,
                trainerId: "youngster"
            );
            _currentMap = TilemapLoader.LoadTilemap("Content/CerucleanCityMap.xml");

            // Collision wiring (minimal)
            player.Map = _currentMap;

            // Build the solid tile index set from the "Ground" layer
            player.SolidTiles = Physics.Collision.BuildSolidIndexSet(
                _currentMap,
                "Ground",
                Physics.SolidTileCollision.IsSolid
            );
        }

        public void Update(GameTime gameTime)
        {
            // Update Objects
            _controller.Update(_game, gameTime, Cam, player, trainer);
            Cam.Update(player);

            // Force a battle with trainer interaction
            if (trainer.colided){
                // Example of starting a specific trainer battle
                _sceneManager.TransitionTo("trainer");
            }
            // no need for base.Update here
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black); // Overworld color

            // map & world entities affected by camera movement
            spriteBatch.Begin(transformMatrix: Cam.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            _currentMap?.DrawCropped(Cam.VisibleWorldRect);
            player.Draw(spriteBatch);
            trainer.Draw(spriteBatch);
            spriteBatch.End();

            // no need for base.Draw here
        }
    }
}

