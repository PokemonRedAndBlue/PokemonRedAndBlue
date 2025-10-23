using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.Behavior;
using Enter.Classes.Cameras;
using Enter.Classes.Characters;
using Enter.Classes.GameState;
using Enter.Classes.Input;
using Enter.Classes.Sprites;
using Enter.Classes.Textures;
using PokemonGame.Engine;
using PokemonGame.Scenes;

namespace Enter;

public class Game1 : Core
{
    // Needed class vars
    Dictionary<String, AnimatedSprite> FrontPokemon = new Dictionary<string, AnimatedSprite>();
    private KeyboardController _controller = new KeyboardController();
    private Vector2 postion = new Vector2(100, 100);
    private Tilemap _currentMap;
    public bool ResetRequested { get; set; } = false;   // added to reset game
    private SceneManager _sceneManager;
    public Game1() : base("PokemonRedAndBlue", 1280, 720, false) { }

    protected override void LoadContent()
    {
        // Initialize Scene Manager and Dependencies
        _sceneManager = new SceneManager(Content);
        _sceneManager.AddScene("overworld", new OverworldScene(_sceneManager, this, _controller));
        _sceneManager.AddScene("battle", new BattleScene(_sceneManager)); // Add placeholder battle scene
        _currentMap = TilemapLoader.LoadTilemap("Content/Route1Map.xml");
        _sceneManager.TransitionTo("overworld"); // <-- Set the starting scene
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // basic update logic
        _sceneManager.Update(gameTime);

        // Check for reset key
        if (ResetRequested)
        {
            Reset();
            return;
        }

        base.Update(gameTime);
    }

    private void Reset()
    {
        // The new way to reset is to just reload the scene
        _sceneManager.TransitionTo("overworld");
        ResetRequested = false;
    }

    protected override void Draw(GameTime gameTime)
    {
        _sceneManager.Draw(SpriteBatch);
        base.Draw(gameTime);
    }
}
