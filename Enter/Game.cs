using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.Behavior;
using Enter.Classes.Characters;
using Enter.Classes.GameState;
using Enter.Classes.Input;
using Enter.Classes.Sprites;
using Enter.Classes.Textures;

namespace Enter;

public class Game1 : Core
{
    // Needed class vars
    Dictionary<String, AnimatedSprite> FrontPokemon = new Dictionary<string, AnimatedSprite>();
    private Texture2D character;
    private Player player;
    private Trainer trainer;
    private KeyboardController controller;
    private TextureAtlas _PokemonBackAtlas;
    private Vector2 frontPokemonPosition = new Vector2(720, 150);
    private Vector2 postion = new Vector2(100, 100);
    private PokeballthrowAnimation _pokeballthrow;
    private PokeballCaptureAnimation _pokeballCapture;

    private HurtAnimation hurt = new HurtAnimation();
    private AttackAnimation attack = new AttackAnimation();

    private DeathAnimation death = new DeathAnimation();

    private List<Tile> _tiles = new List<Tile>();
    public TileCycler TileCycler { get; private set; }
    public bool ResetRequested { get; set; } = false;   // added to reset 
    private double elapsedTime = 0;    
    private int regionsToDraw = 0; 

    public Game1() : base("PokemonRedAndBlue", 1280, 720, false) { }

    protected override void LoadContent()
    {
        PokemonFrontFactory.Instance.LoadAllTextures(Content);
        PokemonBackFactory.Instance.LoadAllTextures(Content);

        _PokemonBackAtlas = TextureAtlas.FromFile(Content, "Pokemon_BACK.xml");

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
        controller.Update(this, gameTime, player, trainer);
        
        // logic for cycling back pokemon
        elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
        if (elapsedTime >= 0.5 && regionsToDraw < _PokemonBackAtlas._regions.Count)
        {
            regionsToDraw++;
            elapsedTime = 0; // reset timer
        }

        // Check for reset key
        if (ResetRequested)
        {
            Reset();
            return;
        }

        _pokeballthrow.Update(gameTime);  
        _pokeballCapture.Update(gameTime);

        var currentTileForUpdate = TileCycler?.GetCurrent();
        currentTileForUpdate?.Update();

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

        // Draw all of the back sprites cycling
        int i = 0;
        foreach (var region in _PokemonBackAtlas._regions)
        {
            if (i >= regionsToDraw)
                break;

            region.Value.Draw(SpriteBatch, new Vector2(100, 500), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            i++;
        }


        var currentTile = TileCycler?.GetCurrent();
        currentTile.Draw(120, 120, 4f, SpriteEffects.None);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
