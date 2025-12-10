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
        private Camera _cam;
        private KeyboardController _controller;
        private Texture2D _character;
        private Trainer _trainer1;
        private Trainer _trainer2;
        private Game1 _game;
        private Tilemap _currentMap;
        private Player _player;
        // Scene-managed player position (in pixels)
        private Point _playerPosition = new(0, 9); // Default spawn (tile 0,9)
        public static Point? NextSpawnPosition = null;
        public Point GetPlayerPosition() => _playerPosition;

        public void SetPlayerPosition(Point pos)
        {
            _playerPosition = pos;
            _player?.SetTilePosition(pos);
        }

        // We must pass in the SceneManager so this scene can request transitions
        public OverworldCityScene(SceneManager sceneManager, Game1 game1, KeyboardController controller, Player p)
        {
            _sceneManager = sceneManager;
            _game = game1;
            _controller = controller;
            _player = p;
        }

        public static void SetNextSpawn(Point pos)
        {
            NextSpawnPosition = pos;
        }

        public void LoadContent(ContentManager content)
        {
            // Load tilemap, player sprites, NPCs, etc.
            _currentMap = TilemapLoader.LoadTilemap("Content/CerucleanCityMap.xml");
            _cam = new(((Game)_game).GraphicsDevice.Viewport);
            _character = content.Load<Texture2D>("images/Pokemon_Characters");

            // Load Background Music
            BackgroundMusicLibrary.Load(content);
            //Music
            BackgroundMusicPlayer.Play(SongId.CeruleanCityTheme, loop: true);

            // Highest priority: explicit next spawn when returning from battle
            if (NextSpawnPosition is Point next)
            {
                SetPlayerPosition(next);
                NextSpawnPosition = null;
                _game.SavedPlayerPosition = null;
            }
            else if (_game?.SavedPlayerPosition is Vector2 savedPosVec)
            {
                SetPlayerPosition(new Point((int)savedPosVec.X, (int)savedPosVec.Y));
                _game.SavedPlayerPosition = null;
            }
            else if (_game.SavedPlayerTiles.TryGetValue("overworld_city", out Point savedTile))
            {
                SetPlayerPosition(savedTile);
            }
            else
            {
                // Default location for this scene if no saved tile, (on first visit)
                SetPlayerPosition(new Point(1, 18));
            }
            _cam.Update(_player);
            _cam.Zoom = ZoomLevel; //Zoom level of world

            // Use explicit sprite indices for unique trainers
            _trainer1 = new Trainer(
                _character,
                1, // spriteIndex for hiker (adjust as needed)
                new Vector2(5 * 32, 10 * 32),
                Facing.Right,
                false, // not moving by default
                "hiker"
            );
            _trainer2 = new Trainer(
                _character,
                2, // spriteIndex for blackbelt (adjust as needed)
                new Vector2(10 * 32, 11 * 32),
                Facing.Left,
                false, // not moving by default
                "blackbelt"
            );
            _trainer1.Map = _currentMap;
            _trainer2.Map = _currentMap;
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
            _trainer1.Update(gameTime, _player);
            _trainer2.Update(gameTime, _player);
            _controller.Update(_game, gameTime, _cam, _player, _trainer1); // Optionally pass both trainers to controller if needed
            _cam.Update(_player);

            // Prevent repeat battles and trigger correct battle scene
            if (_trainer1.colided && !_game.IsTrainerDefeated(_trainer1.TrainerID))
            {
                _game.SavedPlayerPosition = new Vector2(_player.TilePos.X, _player.TilePos.Y);
                _game.SavedPlayerTiles["overworld_city"] = _player.TilePos;
                _sceneManager.TransitionTo("city_trainer1");
            }
            else if (_trainer2.colided && !_game.IsTrainerDefeated(_trainer2.TrainerID))
            {
                _game.SavedPlayerPosition = new Vector2(_player.TilePos.X, _player.TilePos.Y);
                _game.SavedPlayerTiles["overworld_city"] = _player.TilePos;
                _sceneManager.TransitionTo("city_trainer2");
            }

            // Mark trainers as defeated if their ID is in the defeated list (after returning from battle)
            if (_game.IsTrainerDefeated(_trainer1.TrainerID)) {
                _trainer1.HasBeenDefeated = true;
                _trainer1.colided = false;
            }
            if (_game.IsTrainerDefeated(_trainer2.TrainerID)) {
                _trainer2.HasBeenDefeated = true;
                _trainer2.colided = false;
            }

            // If neither trainer is colliding, ensure player is not frozen
            if (!_trainer1.colided && !_trainer2.colided)
                _player.StopEnd();

            Point exit = _player.TilePos;
            //System.Console.WriteLine("exit Tile pos: " + exit);
            if ( exit.X == 0 && ( exit.Y == 18 || exit.Y == 19 ) )
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
            spriteBatch.Begin(transformMatrix: _cam.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            _currentMap?.DrawCropped(_cam.VisibleWorldRect);
            _player.Draw(spriteBatch);
            _trainer1.Draw(spriteBatch);
            _trainer2.Draw(spriteBatch);
            spriteBatch.End();

            // no need for base.Draw here
        }
    }
}

