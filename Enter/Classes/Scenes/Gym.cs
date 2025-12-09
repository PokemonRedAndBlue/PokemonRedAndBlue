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
using System.Collections.Generic;

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
        private Trainer _painterTrainer;
        private Camera _cam;
        private KeyboardController _controller;
        private Texture2D _character;
        private Trainer _trainer;
        private Player _player;
        // Scene-managed player position (in pixels)
        private Point _playerPosition = new(1, 3); // Default spawn (tile 1,3)
        // Allows other scenes to explicitly place the player when returning
        public static Point? NextSpawnPosition = null;
                public Point GetPlayerPosition() => _playerPosition;

                public void SetPlayerPosition(Point pos)
                {
                    _playerPosition = pos;
                    _player?.SetTilePosition(pos);
                }
        private Game1 _game;
        private Tilemap _currentMap;

        // We must pass in the SceneManager so this scene can request transitions
        public GymScene(SceneManager sceneManager, Game1 game1, KeyboardController controller, Player p)
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
            _cam = new(((Game)_game).GraphicsDevice.Viewport);
            _character = content.Load<Texture2D>("images/Pokemon_Characters");

            // Highest priority: explicit next spawn when returning from battle
            if (NextSpawnPosition is Point next)
            {
                SetPlayerPosition(next);
                NextSpawnPosition = null;
                _game.SavedPlayerPosition = null;
            }
            else if (_game?.SavedPlayerPosition is Vector2 savedPosVec)
            {
                // Coming back from a battle scene; use the saved tile position
                SetPlayerPosition(new Point((int)savedPosVec.X, (int)savedPosVec.Y));
                _game.SavedPlayerPosition = null;
            }
            else if (_game.SavedPlayerTiles.TryGetValue("gym", out Point savedTile))
            {
                SetPlayerPosition(savedTile);
            }
            else
            {
                // Default location for this scene if no saved tile
                SetPlayerPosition(new Point(5, 12));
            }

            _cam.Update(_player);
            _cam.Zoom = ZoomLevel; //Zoom level of world

            // prof painter trainer stuff
            TextureAtlas painterStuff = TextureAtlas.FromFile(content, "PainterTrainerSheet.xml");
            var painterRegion = painterStuff.GetRegion("painter-map-trainer");
            const float painterScale = 0.07f; // scale the atlas art to fit the scene
            _painterTrainer = new Trainer(
                painterRegion,
                new Vector2(2 * 32, .6f * 32),
                Facing.Down,
                moving: false,
                scale: painterScale,
                trainerId: "trainer-painter"
            );
            _trainer = _painterTrainer;
            _currentMap = TilemapLoader.LoadTilemap("Content/GymMap.xml");

            // Collision wiring (minimal)
            _player.Map = _currentMap;
            _painterTrainer.Map = _currentMap;

            // Build the solid tile index set from the "Ground" layer
            _player.SolidTiles = Physics.Collision.BuildSolidIndexSet(
                _currentMap,
                "Ground",
                Physics.SolidTileCollision.IsSolid
            );

            _cam.Update(_player);
        }

        public void Update(GameTime gameTime)
        {
            // Update Objects
            _controller.Update(_game, gameTime, _cam, _player, _trainer);
            _cam.Update(_player);

            // Force a battle with trainer interaction
            if (_trainer.colided)
            {
                // Save the actual player position before battle
                _playerPosition = _player.TilePos;
                _game.SavedPlayerPosition = new Vector2(_player.TilePos.X, _player.TilePos.Y);
                _sceneManager.TransitionTo("gym_trainer_painter");
            }
            Point exit = _player.TilePos;
            //System.Console.WriteLine("exit Tile pos: " + exit);
            if (exit.Y == 13 && ( exit.X == 4 || exit.X == 5 ) )
            {
                _game.SavedPlayerTiles["gym"] = _player.TilePos;
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
            Console.WriteLine("Player:" + _player.TilePos);
        }
    }
}

