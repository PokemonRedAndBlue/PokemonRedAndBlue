using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.Input;
using Enter.Classes.Scenes;
using Enter.Classes.Sprites;
using Enter.Classes.Cameras;
using Microsoft.Xna.Framework.Input;

namespace Enter;

public class Game1 : Core
{
    // Needed class vars
    public bool ResetRequested { get; set; } = false;   // added to reset game
    Dictionary<String, AnimatedSprite> FrontPokemon = new Dictionary<string, AnimatedSprite>();
    private KeyboardController _controller = new KeyboardController();
    private Vector2 postion = new Vector2(100, 100);
    private SceneManager _sceneManager;
    public Game1() : base("PokemonRedAndBlue", 1280, 720, false) { }

    protected override void LoadContent()
    {
        // Initialize Scene Manager and Dependencies
        _sceneManager = new SceneManager(Content, SpriteBatch);
        _sceneManager.AddScene("overworld", new OverworldScene(_sceneManager, this, _controller));
        _sceneManager.AddScene("overworld_city", new OverworldCityScene(_sceneManager, this, _controller));
        _sceneManager.AddScene("gym", new GymScene(_sceneManager, this, _controller));
        _sceneManager.AddScene("trainer", new TrainerBattleScene(_sceneManager, this, "TRAINER_TESTER"));
        _sceneManager.AddScene("wild", new WildEncounter(_sceneManager, this));
        _sceneManager.TransitionTo("overworld"); // <-- Set the starting scene

        // This is just for testing to check that the stats are calculated correctly, will be a random pokemon with random IV
        var pokemon = PokemonGenerator.GenerateRandom();
        pokemon.PrintSummary();

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // basic update logic
        _sceneManager.Update(gameTime);

        // Check for reset key
        if (ResetRequested) { Reset(); return; }

        // check for wild encounter key
        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
        {
            _sceneManager.TransitionTo("wild");
        }
        // If caught in trainer battle in scene, defaults back to Route 1 for now
        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C))
        {
            _sceneManager.TransitionTo("overworld_city");
        }
        
        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G))
        {
            _sceneManager.TransitionTo("gym");
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
