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
using PokemonGame;

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
        Pokemon[] playersPokemon = {new Pokemon("squirtle", 6)};
        Team team = new Team(playersPokemon);
        Player player = new Player(character, this.Window, team);

        // Initialize Scene Manager and Dependencies
        _sceneManager = new SceneManager(Content, SpriteBatch);
        _sceneManager.AddScene("overworld", new OverworldScene(_sceneManager, this, _controller, player));
        _sceneManager.AddScene("overworld_city", new OverworldCityScene(_sceneManager, this, _controller, player));
        _sceneManager.AddScene("gym", new GymScene(_sceneManager, this, _controller, player));
        _sceneManager.AddScene("trainer", new TrainerBattleScene(_sceneManager, this, "youngster", player));
        _sceneManager.AddScene("city_trainer1", new TrainerBattleScene(_sceneManager, this, "hiker", player, "overworld_city"));
        _sceneManager.AddScene("city_trainer2", new TrainerBattleScene(_sceneManager, this, "blackbelt", player, "overworld_city"));
        _sceneManager.AddScene("wild", new WildEncounter(_sceneManager, this, player));
        _sceneManager.TransitionTo("overworld"); // <-- Set the starting scene

        // This is just for testing to check that the stats are calculated correctly, will be a random pokemon with random IV
        // var pokemon = PokemonGenerator.GenerateWildPokemon();
        // pokemon.PrintSummary();

        // DamageTest.Run();
        // BattleAITest.Run();

        //Music && Sound effect
        BackgroundMusicLibrary.Load(Content);
        SoundEffectLibrary.Load(Content);


        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // basic update logic
        _sceneManager.Update(gameTime);

        // Check for reset key
        if (ResetRequested) { Reset(); return; }
        
        // Cache keyboard state to avoid multiple calls
        KeyboardState keyState = Keyboard.GetState();
        
        // If caught in trainer battle in scene, defaults back to Route 1 for now
        if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C))
        {
            _sceneManager.TransitionTo("overworld_city");
        }
        
        if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G))
        {
            _sceneManager.TransitionTo("gym");
        }

        //Music
        _sceneManager.Update(gameTime);
        // ----------------------------------------
        //  MUSIC TESTING KEYS (CAN BE REMOVED)
        // ----------------------------------------
        var kb = Keyboard.GetState();

        if (kb.IsKeyDown(Keys.U))
        {
            BackgroundMusicPlayer.Play(SongId.OpeningPart2);
        }
        if (kb.IsKeyDown(Keys.O))
        {
            SoundEffectPlayer.Play(SfxId.SFX_LEVEL_UP);
        }
        if (kb.IsKeyDown(Keys.I))
        {
            BackgroundMusicPlayer.Play(SongId.BattleWildPokemon);
        }

        if (kb.IsKeyDown(Keys.Q))
        {
            BackgroundMusicPlayer.Stop();
        }

        base.Update(gameTime);

        // Update window title with FPS and recent scene timings (update/draw ms)
        try
        {
            string baseTitle = Window.Title?.Split('|')[0].Trim() ?? "";
            int fps = Core.CurrentFps;
            double uMs = _sceneManager?.LastUpdateMs ?? 0.0;
            double dMs = _sceneManager?.LastDrawMs ?? 0.0;
            Window.Title = string.IsNullOrEmpty(baseTitle)
                ? $"FPS: {fps} | U:{uMs:F1}ms D:{dMs:F1}ms"
                : $"{baseTitle} | FPS: {fps} | U:{uMs:F1}ms D:{dMs:F1}ms";
        }
        catch { }
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
