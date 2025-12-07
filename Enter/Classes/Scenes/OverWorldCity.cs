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
    public class OverworldCityScene : IGameScene
    {
        private const float ZoomLevel = 4f; 
        private SceneManager _sceneManager;
        private SpriteFont _font; // Placeholder for UI/debug text
        private Tilemap _tilemap;
        private Camera Cam;
        private KeyboardController _controller;
        private Texture2D character;
        private Trainer trainer1;
        private Trainer trainer2;
        private Game1 _game;
        private Tilemap _currentMap;
        private Player _player;
        // Scene-managed player position (in pixels)
        private Vector2 _playerPosition = new Vector2(0, 9 * 32); // Default spawn (tile 0,9)
        public Vector2 GetPlayerPosition() => _playerPosition;

        public void SetPlayerPosition(Vector2 pos)
        {
            _playerPosition = pos;
            if (_player != null)
            {
                _player.Position = pos;
                Point tile = _player.PixelToTile(pos);
                _player.SetTilePosition(tile);
            }
        }

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
            _currentMap = TilemapLoader.LoadTilemap("Content/CerucleanCityMap.xml");
            Cam = new(((Game)_game).GraphicsDevice.Viewport);
            character = content.Load<Texture2D>("images/Pokemon_Characters");

            // Load Background Music
            BackgroundMusicLibrary.Load(content);
            //Music
            BackgroundMusicPlayer.Play(SongId.CeruleanCityTheme, loop: true);

            // Only restore from Game1.SavedPlayerPosition if available
              Vector2 spawn = _playerPosition;
            if ((_game as Game1)?.SavedPlayerPosition is Microsoft.Xna.Framework.Vector2 savedPos)
            {
                spawn = savedPos;
                (_game as Game1).SavedPlayerPosition = null;
            }

            if (_game.SavedPlayerTiles.TryGetValue("overworld_city", out Point savedTile))
            {
                _player.SetTilePosition(savedTile);
            }
            else
            {
                // Default location for this scene if no saved tile, (on first visit)
                _player.SetTilePosition(new Point(1, 18));
            }

            //SetPlayerPosition(spawn);
            Cam.Update(_player);
            Cam.Zoom = ZoomLevel; //Zoom level of world

            // Use explicit sprite indices for unique trainers
            trainer1 = new Trainer(
                character,
                1, // spriteIndex for hiker (adjust as needed)
                new Vector2(5 * 32, 10 * 32),
                Facing.Right,
                false, // not moving by default
                "hiker"
            );
            trainer2 = new Trainer(
                character,
                2, // spriteIndex for blackbelt (adjust as needed)
                new Vector2(10 * 32, 11 * 32),
                Facing.Left,
                false, // not moving by default
                "blackbelt"
            );
            trainer1.Map = _currentMap;
            trainer2.Map = _currentMap;
            _player.Map = _currentMap;

            // Build the solid tile index set from the "Ground" layer
            _player.SolidTiles = Physics.Collision.BuildSolidIndexSet(
                _currentMap,
                "Ground",
                Physics.SolidTileCollision.IsSolid
            );
        }

        public void Update(GameTime gameTime)
        {
            // Update Objects
            trainer1.Update(gameTime, _player);
            trainer2.Update(gameTime, _player);
            _controller.Update(_game, gameTime, Cam, _player, trainer1); // Optionally pass both trainers to controller if needed
            Cam.Update(_player);

            // Prevent repeat battles and trigger correct battle scene
            if (trainer1.colided && !_game.IsTrainerDefeated(trainer1.TrainerID))
            {
                // _playerPosition = _player.Position;
                // _game.SavedPlayerPosition = _player.Position;
                _game.SavedPlayerTiles["overworld_city"] = _player.TilePos;
                _sceneManager.TransitionTo("city_trainer1");
            }
            else if (trainer2.colided && !_game.IsTrainerDefeated(trainer2.TrainerID))
            {
                // _playerPosition = _player.Position;
                // _game.SavedPlayerPosition = _player.Position;
                _game.SavedPlayerTiles["overworld_city"] = _player.TilePos;
                _sceneManager.TransitionTo("city_trainer2");
            }

            // Mark trainers as defeated if their ID is in the defeated list (after returning from battle)
            if (_game.IsTrainerDefeated(trainer1.TrainerID)) {
                trainer1.HasBeenDefeated = true;
                trainer1.colided = false;
            }
            if (_game.IsTrainerDefeated(trainer2.TrainerID)) {
                trainer2.HasBeenDefeated = true;
                trainer2.colided = false;
            }

            // If neither trainer is colliding, ensure player is not frozen
            if (!trainer1.colided && !trainer2.colided)
                _player.StopEnd();

            Vector2 PlayerPosition = GetPlayerPosition();
            Point exit = _player.TilePos;
            //System.Console.WriteLine("exit Tile pos: " + exit);
            if (exit.X == 0 && exit.Y == 18)
            {
                _game.SavedPlayerTiles["overworld_city"] = _player.TilePos + new Point(1, 0);
                _sceneManager.TransitionTo("overworld");
            }
            if (exit.X == 0 && exit.Y == 19)
            {
                _game.SavedPlayerTiles["overworld_city"] = _player.TilePos + new Point(1, 0);
                _sceneManager.TransitionTo("overworld");
            }
            if (exit.X == 30 && exit.Y == 19)
            {
                _game.SavedPlayerTiles["overworld_city"] = _player.TilePos + new Point(0, 1);
                _sceneManager.TransitionTo("gym");
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
            trainer1.Draw(spriteBatch);
            trainer2.Draw(spriteBatch);
            spriteBatch.End();

            // no need for base.Draw here
        }
    }
}

