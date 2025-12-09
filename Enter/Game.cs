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
    public Dictionary<string, Point> SavedPlayerTiles { get; set; } = new Dictionary<string, Point>();
    Dictionary<String, AnimatedSprite> FrontPokemon = new Dictionary<string, AnimatedSprite>();
    // Tracks which trainers have been defeated, keyed by trainer ID
    private Dictionary<string, bool> _defeatedTrainers = new Dictionary<string, bool>();
    private KeyboardController _controller = new KeyboardController();
    private Vector2 postion = new Vector2(100, 100);

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

        _sceneManager.AddScene("title_sequence", new TitleSequenceScene(_sceneManager, this));
        _sceneManager.AddScene("intro", new IntroScene(_sceneManager, this));
        _sceneManager.AddScene("menu", new MenuScene(_sceneManager, this));
        _sceneManager.AddScene("oakIntro", new OakIntroScene(_sceneManager, this));
        _sceneManager.AddScene("overworld", new OverworldScene(_sceneManager, this, _controller, player));
        _sceneManager.AddScene("overworld_city", new OverworldCityScene(_sceneManager, this, _controller, player));
        _sceneManager.AddScene("gym", new GymScene(_sceneManager, this, _controller, player));
        _sceneManager.AddScene("trainer", new TrainerBattleScene(_sceneManager, this, "youngster", player));
        _sceneManager.AddScene("gym_trainer_painter", new TrainerBattleScene(_sceneManager, this, "trainer-painter", player, "gym"));
        _sceneManager.AddScene("city_trainer1", new TrainerBattleScene(_sceneManager, this, "hiker", player, "overworld_city"));
        _sceneManager.AddScene("city_trainer2", new TrainerBattleScene(_sceneManager, this, "blackbelt", player, "overworld_city"));
        _sceneManager.AddScene("wild", new WildEncounter(_sceneManager, this, player));
        _sceneManager.TransitionTo("overworld"); // <-- Set the starting scene

        //Music && Sound effect
        BackgroundMusicLibrary.Load(Content);
        SoundEffectLibrary.Load(Content);


        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (ResetRequested)
        {
            Reset();
            return;
        }

        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.C))
        {
            _sceneManager.TransitionTo("overworld_city");
        }

        if (keyState.IsKeyDown(Keys.G))
        {
            _sceneManager.TransitionTo("gym");
        }

        _sceneManager.Update(gameTime);
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
