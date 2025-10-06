using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.Behavior;
using Enter.Classes.Characters;
using Enter.Classes.GameState;
using Enter.Classes.Input;
using Enter.Classes.Scenes;
using Enter.Classes.Sprites;
using Enter.Classes.Textures;
using System;

namespace Enter;

public class Game1 : Core
{
    // Needed class vars
    Dictionary<String, AnimatedSprite> FrontPokemon = new Dictionary<string, AnimatedSprite>();
    private Texture2D character;
    private Player player;
    private Trainer trainer;
    private KeyboardController controller;
    private Vector2 frontPokemonPosition = new Vector2(720, 150);
    private Vector2 postion = new Vector2(100, 100);
    private PokeballthrowAnimation _pokeballthrow;
    private PokeballCaptureAnimation _pokeballCapture;

    private HurtAnimation hurt = new HurtAnimation();
    private AttackAnimation attack = new AttackAnimation();

    private DeathAnimation death = new DeathAnimation();

    private List<Tile> _tiles = new List<Tile>();
    public TileCycler TileCycler { get; private set; }

    public Game1() : base("PokemonRedAndBlue", 1280, 720, false) { }

    protected override void LoadContent()
    {
        PokemonFrontFactory.Instance.LoadAllTextures(Content);
        PokemonBackFactory.Instance.LoadAllTextures(Content);

        character = Content.Load<Texture2D>("images/Pokemon_Characters");
        player = new Player(character, Window);
        trainer = new Trainer(
            character,
            new Vector2(Window.ClientBounds.Height, Window.ClientBounds.Width) * 0.25f,
            Facing.Right
        );
        controller = new KeyboardController();

        _pokeballthrow = new PokeballthrowAnimation(640,360);
        _pokeballthrow.LoadContent(Content);

        _pokeballCapture = new PokeballCaptureAnimation(300, 360);
        _pokeballCapture.LoadContent(Content);

        _tiles = TileLoader.LoadTiles("Content/Tiles.xml");
        TileCycler = new TileCycler(_tiles);

        // Create the nine starter Pokémon animations
        String[] FrontPokemonAnimations = {
                            "bulbasaur-front",
                            "ivysaur-front",
                            "venusaur-front",
                            "squirtle-front",
                            "wartortle-front",
                            "blastoise-front",
                            "charmander-front",
                            "charmeleon-front",
                            "charizard-front"
                            };

        // create all animated front sprites and add to dictionary
        foreach (var currentPokemon in FrontPokemonAnimations)
        {
            String PokemonString = currentPokemon.Substring(0, currentPokemon.IndexOf('-'));
            FrontPokemon.Add(PokemonString, PokemonFrontFactory.Instance.CreateAnimatedSprite(currentPokemon));
        }

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        controller.Update(this, trainer);

        // Check for reset key
        if (controller.ResetRequested)
        {
            Reset();
            return; 
        }

        _pokeballthrow.Update(gameTime);  
        _pokeballCapture.Update(gameTime);

        var currentTileForUpdate = TileCycler?.GetCurrent();
        currentTileForUpdate?.Update();

        player.Update(gameTime, controller);
        trainer.Update(gameTime, player);

        // update all pokemon animations
        foreach (AnimatedSprite frontPokemon in FrontPokemon.Values)
        {
            frontPokemon.Update(gameTime);
        }

        base.Update(gameTime);
    }

    private void Reset()
    {
        // Reset player to initial position
        player = new Player(character, Window);

        // Reset trainer to initial position
        trainer = new Trainer(
            character,
            new Vector2(Window.ClientBounds.Height, Window.ClientBounds.Width) * 0.25f,
            Facing.Right
        );

        // Reset controller state
        controller = new KeyboardController();

        // Reset animations
        _pokeballthrow = new PokeballthrowAnimation(640, 360);
        _pokeballthrow.LoadContent(Content);

        _pokeballCapture = new PokeballCaptureAnimation(300, 360);
        _pokeballCapture.LoadContent(Content);

        // Reset tile cycler to first tile
        if (TileCycler != null)
        {
            TileCycler.Reset();
        }

        // Reset other state variables
        postion = new Vector2(100, 100);
        frontPokemonPosition = new Vector2(720, 150);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _pokeballthrow.Draw(SpriteBatch);
        _pokeballCapture.Draw(SpriteBatch);

        player.Draw(SpriteBatch, 4f);
        trainer.Draw(SpriteBatch, 4f);

        // Draw all Pokémon on the right half of the screen (x >= 640)
        frontPokemonPosition = new Vector2(720, 150);
        foreach (AnimatedSprite frontPokemon in FrontPokemon.Values)
        {
            frontPokemon.Draw(SpriteBatch, Color.White, frontPokemonPosition, 4f);

            // if we are not on final collumn keep adding
            if (frontPokemonPosition.X < 1200)
            {
                frontPokemonPosition.X += 240;
            }
            // if we are past the final row reset x and go to next row
            else
            {
                frontPokemonPosition.Y += 210;
                frontPokemonPosition.X = 720;
            }
        }

        var currentTile = TileCycler?.GetCurrent();
        currentTile.Draw(120, 120, 4f, SpriteEffects.None);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
