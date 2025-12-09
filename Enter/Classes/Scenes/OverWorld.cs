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
using System;
using Enter.Classes.Behavior;


namespace Enter.Classes.Scenes
{
    /// <summary>
    /// The scene for when the player is walking around in the world.
    /// Handles player movement, tilemap rendering, and collision.
    /// </summary>
    public class OverworldScene : IGameScene
    {
        private const float ZoomLevel = 4f; 
        private const float WildEncounterTriggerChance = 0.035f;
        private const double WildEncounterCooldown = 2.0;
        private const int WildGrassTileId = 5; // wild-grass-route1
        private SceneManager _sceneManager;
        private Camera _cam;
        private KeyboardController _controller;
        private Texture2D character;
        private Trainer _trainer;
        private Player _player;
        private Game1 _game;
        private Tilemap _currentMap;
        private readonly Random _rand = new();
        private double _encounterTimer = WildEncounterCooldown;

        // Scene-managed player position (in pixels)
        private Point _playerPosition = new(11, 0); // Default spawn
        // Static field to allow other scenes to set the next spawn position
        public static Point? NextSpawnPosition = null;

        public Point GetPlayerPosition() => _playerPosition;

        public void SetPlayerPosition(Point pos)
        {
            _playerPosition = pos;
            _player?.SetTilePosition(pos);
        }

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
            _cam = new(((Game)_game).GraphicsDevice.Viewport);

            // Load Background Music
            BackgroundMusicLibrary.Load(content);
            //Music
            BackgroundMusicPlayer.Play(SongId.RoadToViridianFromPallet, loop: true);

            // Only restore from Game1.SavedPlayerPosition if returning from a battle, else use this scene's last known position
            Point spawn = _playerPosition;
            if (_game?.SavedPlayerPosition is Point savedPos)
            {
                spawn = savedPos;
                // Clear after use so it doesn't leak between scenes
                _game.SavedPlayerPosition = null;
            }
            SetPlayerPosition(spawn);

            if (_game.SavedPlayerTiles.TryGetValue("overworld", out Point savedTile))
            {
                _player.SetTilePosition(savedTile + new Point(0, -1));
            }
            else
            {
                // Default location for this scene if no saved tile, (on first visit)
                _player.SetTilePosition(new Point(10, 0));
            }

            _cam.Update(_player);
            _cam.Zoom = ZoomLevel; //Zoom level of world
            // Create trainer with specific ID that matches what's used in TrainerBattle
            character = content.Load<Texture2D>("images/Pokemon_Characters");
            const string TRAINER_ID = "youngster"; // This should match the ID used in Game1's AddScene
            _trainer = new Trainer(
                character,
                new Vector2(_game.Window.ClientBounds.Height, _game.Window.ClientBounds.Width) * 0.25f,
                Facing.Right,
                false,  // not moving by default
                TRAINER_ID
            );
            _currentMap = TilemapLoader.LoadTilemap("Content/Route1Map.xml");
            _trainer.Map = _currentMap;

            // Collision wiring (minimal)
            _player.Map = _currentMap;

            // Build the solid tile index set from the "Ground" layer
            _player.SolidTiles = Physics.Collision.BuildSolidIndexSet(
                _currentMap,
                "Ground",
                Physics.SolidTileCollision.IsSolid
            );

        }
        // Static helper for other scenes to set the next spawn position before transitioning to overworld
        public static void SetNextSpawn(Point pos)
        {
            NextSpawnPosition = pos;
        }

        private void WildEncounterTrigger(Player player)
        {
            if (_currentMap.GetTileAt("Ground", player.TilePos.X, player.TilePos.Y) == WildGrassTileId && player.isMoving)
                if (_rand.NextSingle() <= WildEncounterTriggerChance)
                    _sceneManager.TransitionTo("wild");
        }

        public void Update(GameTime gameTime)
        {
            // Reset collision if trainer is defeated to prevent immediate retrigger
            if (_game.IsTrainerDefeated(_trainer.TrainerID)) {
                _trainer.HasBeenDefeated = true;
                _trainer.colided = false;
            }
            // Update Objects
            _controller.Update(_game, gameTime, _cam, _player, _trainer);
            _cam.Update(_player);

            // Handle trainer interactions
            if (_trainer.colided && !_trainer.IsApproachingPlayer)
            {
                if (!_game.IsTrainerDefeated(_trainer.TrainerID))
                {
                    // Save the actual player position before battle
                    // _playerPosition = _player.Position;
                    // _game.SavedPlayerPosition = _player.Position;
                    _game.SavedPlayerTiles["overworld"] = _player.TilePos + new Point(0, 1);
                    _sceneManager.TransitionTo("trainer");
                }
                // If trainer is defeated, they're just interacting without battle
                // Could show dialogue here
            }

            if (_encounterTimer > 0) _encounterTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (_encounterTimer <= 0 && _player.HasArrived) WildEncounterTrigger(_player);

            // check for wild encounter key (cache keyboard state)
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.W))
            {
                // Save the actual player position before wild battle
                // _playerPosition = _player.Position;
                // _game.SavedPlayerPosition = _player.Position;
                _game.SavedPlayerTiles["overworld"] = _player.TilePos + new Point(0, 1);    //TODO: why add (0,1)?
                _sceneManager.TransitionTo("wild");
            }

            Point exit = _player.TilePos;
            //Console.WriteLine("exit Tile pos: " + exit);
            if ( exit.Y == 35 && (exit.X == 10 || exit.X == 11) )
            {
                _game.SavedPlayerTiles["overworld"] = _player.TilePos;
                _sceneManager.TransitionTo("overworld_city");
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
            _trainer.Draw(spriteBatch);
            spriteBatch.End();

            // no need for base.Draw here
        }


    }
}

