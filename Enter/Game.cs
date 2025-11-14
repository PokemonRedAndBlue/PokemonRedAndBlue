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
using Enter.Classes.Characters;
using System.Reflection.PortableExecutable;

namespace Enter;

public class Game1 : Core
{
    // Needed class vars
    public bool ResetRequested { get; set; } = false;   // added to reset game
    Dictionary<String, AnimatedSprite> FrontPokemon = new Dictionary<string, AnimatedSprite>();
    // Tracks which trainers have been defeated, keyed by trainer ID
    private Dictionary<string, bool> _defeatedTrainers = new Dictionary<string, bool>();
    private KeyboardController _controller = new KeyboardController();
    private Vector2 postion = new Vector2(100, 100);
    private Tilemap _currentMap;

    private SceneManager _sceneManager;
    // State that persists across scene transitions
    public Microsoft.Xna.Framework.Vector2? SavedPlayerPosition { get; set; } = null;

    // Methods to check/update trainer defeat status
    public bool IsTrainerDefeated(string trainerId)
    {
        return _defeatedTrainers.TryGetValue(trainerId, out bool defeated) && defeated;
    }

    public void MarkTrainerDefeated(string trainerId)
    {
        _defeatedTrainers[trainerId] = true;
    }
    public Game1() : base("PokemonRedAndBlue", 1280, 720, false) { }

    protected override void LoadContent()
    {
        // get player object
        Texture2D character = Content.Load<Texture2D>("images/Pokemon_Characters");
        Player player = new Player(character, this.Window);

        // Initialize Scene Manager and Dependencies
        _sceneManager = new SceneManager(Content, SpriteBatch);
        _sceneManager.AddScene("overworld", new OverworldScene(_sceneManager, this, _controller));
        _sceneManager.AddScene("trainer", new TrainerBattleScene(_sceneManager, this, "youngster", player));
        _sceneManager.AddScene("wild", new WildEncounter(_sceneManager, this));
        _currentMap = TilemapLoader.LoadTilemap("Content/Route1Map.xml");
        _sceneManager.TransitionTo("overworld"); // <-- Set the starting scene

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // basic update logic
        _sceneManager.Update(gameTime);

        // Check for reset key
        if (ResetRequested) { Reset(); return; }

        base.Update(gameTime);
    }

    private void Reset()
    {
        // The new way to reset is to just reload the scene
        SavedPlayerPosition = new Vector2(160, 0);
        _defeatedTrainers.Clear();
        _sceneManager.TransitionTo("overworld");
        ResetRequested = false;
    }

    protected override void Draw(GameTime gameTime)
    {
        _sceneManager.Draw(SpriteBatch);
        base.Draw(gameTime);
    }
}
