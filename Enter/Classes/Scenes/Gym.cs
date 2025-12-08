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
    public class GymScene : IGameScene
    {
        private const float ZoomLevel = 4f; 
        private SceneManager _sceneManager;
        private Trainer painterTrainer;
        private Camera Cam;
        private KeyboardController _controller;
        private Texture2D character;
        private Trainer trainer;
        private Player player;
        // Scene-managed player position (in pixels)
        private Vector2 _playerPosition = new Vector2(2 * 32, 6 * 32); // Default spawn (tile 1,3)
                public Vector2 GetPlayerPosition() => _playerPosition;

                public void SetPlayerPosition(Vector2 pos)
                {
                    _playerPosition = pos;
                    if (player != null)
                    {
                        player.Position = pos;
                        Point tile = player.PixelToTile(pos);
                        player.SetTilePosition(tile);
                    }
                }
        private Game1 _game;
        private Tilemap _currentMap;

        // We must pass in the SceneManager so this scene can request transitions
        public GymScene(SceneManager sceneManager, Game1 game1, KeyboardController controller, Player p)
        {
            _sceneManager = sceneManager;
            _game = game1;
            _controller = controller;
            player = p;
        }

        public void LoadContent(ContentManager content)
        {
            // Load tilemap, player sprites, NPCs, etc.
            Cam = new(((Game)_game).GraphicsDevice.Viewport);
            character = content.Load<Texture2D>("images/Pokemon_Characters");

            // Only restore from Game1.SavedPlayerPosition if returning from a battle scene, else use this scene's last known position
            Vector2 spawn = _playerPosition;
            if ((_game as Game1)?.SavedPlayerPosition is Microsoft.Xna.Framework.Vector2 savedPos)
            {
                // Only use if coming from a battle scene (trainer or wild)
                var prev = _sceneManager?.PreviousSceneName;
                if (prev == "trainer" || prev == "wild")
                {
                    spawn = savedPos;
                }
                (_game as Game1).SavedPlayerPosition = null;
            }
            SetPlayerPosition(spawn);

            if (_game.SavedPlayerTiles.TryGetValue("gym", out Point savedTile))
            {
                player.SetTilePosition(savedTile + new Point(0, -1));
            }
            else
            {
                // Default location for this scene if no saved tile, (on first visit)
                player.SetTilePosition(new Point(5, 12));
            }

            Cam.Update(player);
            Cam.Zoom = ZoomLevel; //Zoom level of world

            // prof painter trainer stuff
            TextureAtlas painterStuff = content.Load<TextureAtlas>("images/PainterTrainerSheet");
            character = painterStuff.GetRegion("trainer-painter").Texture;
            painterTrainer = new Trainer(
                character,
                new Vector2(5 * 32, 10 * 32),
                Facing.Down,
                false,
                trainerId: "trainer-painter"
            );
            _currentMap = TilemapLoader.LoadTilemap("Content/GymMap.xml");

            // Collision wiring (minimal)
            player.Map = _currentMap;
            painterTrainer.Map = _currentMap;

            // Build the solid tile index set from the "Ground" layer
            player.SolidTiles = Physics.Collision.BuildSolidIndexSet(
                _currentMap,
                "Ground",
                Physics.SolidTileCollision.IsSolid
            );

            Cam.Update(player);
        }

        public void Update(GameTime gameTime)
        {
            // Update Objects
            _controller.Update(_game, gameTime, Cam, player, trainer);
            Cam.Update(player);

            // Force a battle with trainer interaction
            if (trainer.colided)
            {
                // Save the actual player position before battle
                _playerPosition = player.Position;
                _game.SavedPlayerPosition = player.Position;
                _sceneManager.TransitionTo("trainer");
            }
            Vector2 PlayerPosition = GetPlayerPosition();
            Point exit = player.TilePos;
            //System.Console.WriteLine("exit Tile pos: " + exit);
            if (exit.X == 4 && exit.Y == 13)
            {
                _game.SavedPlayerTiles["gym"] = player.TilePos;
                _sceneManager.TransitionTo("overworld_city");
            }
            if (exit.X == 5 && exit.Y == 13)
            {
                _game.SavedPlayerTiles["gym"] = player.TilePos;
                _sceneManager.TransitionTo("overworld_city");
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

