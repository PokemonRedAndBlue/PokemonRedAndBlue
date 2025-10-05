using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Enter.Classes.Animations;
using Enter.Classes.Behavior;
using Enter.Classes.Characters;
using Enter.Classes.Data;
using Enter.Classes.GameState;
using Enter.Classes.Input;
using Enter.Classes.Scenes;
using Enter.Classes.Sprites;
using Enter.Classes.Textures;

namespace Enter;

public class Game1 : Core
{
    // Needed class vars
    private AnimatedSprite _bulbasuar, _ivysaur, _venusaur, _squirtle,
        _wartortle, _blastoise, _charmander, _charmeleon, _charizard;
    private Texture2D character;
    private Player player;
    private Trainer trainer;
    private KeyboardController controller;
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

        // Hardcode the nine Pokémon animations
        _bulbasuar = PokemonFrontFactory.Instance.CreateAnimatedSprite("bulbasaur-front");
        _ivysaur = PokemonFrontFactory.Instance.CreateAnimatedSprite("ivysaur-front");
        _venusaur = PokemonFrontFactory.Instance.CreateAnimatedSprite("venusaur-front");
        _squirtle = PokemonFrontFactory.Instance.CreateAnimatedSprite("squirtle-front");
        _wartortle = PokemonFrontFactory.Instance.CreateAnimatedSprite("wartortle-front");
        _blastoise = PokemonFrontFactory.Instance.CreateAnimatedSprite("blastoise-front");
        _charmander = PokemonFrontFactory.Instance.CreateAnimatedSprite("charmander-front");
        _charmeleon = PokemonFrontFactory.Instance.CreateAnimatedSprite("charmeleon-front");
        _charizard = PokemonFrontFactory.Instance.CreateAnimatedSprite("charizard-front");

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

        int ax = 0, ay = 0;
        switch (controller.MoveDirection)
        {
            case Direction.Left:
                ax = -1;
                break;
            case Direction.Right:
                ax =  1;
                break;
            case Direction.Up:
                ay = -1;
                break;
            case Direction.Down:
                ay =  1;
                break;
            default:
                ax =  0;
                ay = 0;
                break;
        }
        _pokeballthrow.Update(gameTime);  
        _pokeballCapture.Update(gameTime);

        var currentTileForUpdate = TileCycler?.GetCurrent();
        currentTileForUpdate?.Update();

        player.Update(gameTime, ax, ay);
        trainer.Update(gameTime, player);

        // update all pokemon animations
        _bulbasuar.Update(gameTime);
        _ivysaur.Update(gameTime);
        _venusaur.Update(gameTime);
        _squirtle.Update(gameTime);
        _wartortle.Update(gameTime);
        _blastoise.Update(gameTime);
        _charmander.Update(gameTime);
        _charmeleon.Update(gameTime);
        _charizard.Update(gameTime);

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
        
        // Reset all Pokémon animations
        _bulbasuar = PokemonFrontFactory.Instance.CreateAnimatedSprite("bulbasaur-front");
        _ivysaur = PokemonFrontFactory.Instance.CreateAnimatedSprite("ivysaur-front");
        _venusaur = PokemonFrontFactory.Instance.CreateAnimatedSprite("venusaur-front");
        _squirtle = PokemonFrontFactory.Instance.CreateAnimatedSprite("squirtle-front");
        _wartortle = PokemonFrontFactory.Instance.CreateAnimatedSprite("wartortle-front");
        _blastoise = PokemonFrontFactory.Instance.CreateAnimatedSprite("blastoise-front");
        _charmander = PokemonFrontFactory.Instance.CreateAnimatedSprite("charmander-front");
        _charmeleon = PokemonFrontFactory.Instance.CreateAnimatedSprite("charmeleon-front");
        _charizard = PokemonFrontFactory.Instance.CreateAnimatedSprite("charizard-front");
        
        // Reset other state variables
        postion = new Vector2(100, 100);
        hurt = new HurtAnimation();
        attack = new AttackAnimation();
        death = new DeathAnimation();
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
        _bulbasuar.Draw(SpriteBatch, Color.White, new Vector2(720, 150), 4f);
        _ivysaur.Draw(SpriteBatch, Color.White, new Vector2(960, 150), 4f);
        _venusaur.Draw(SpriteBatch, Color.White, new Vector2(1200, 150), 4f);

        _squirtle.Draw(SpriteBatch, Color.White, new Vector2(720, 360), 4f);
        _wartortle.Draw(SpriteBatch, Color.White, new Vector2(960, 360), 4f);
        _blastoise.Draw(SpriteBatch, Color.White, new Vector2(1200, 360), 4f);

        _charmander.Draw(SpriteBatch, Color.White, new Vector2(720, 570), 4f);
        _charmeleon.Draw(SpriteBatch, Color.White, new Vector2(960, 570), 4f);
        _charizard.Draw(SpriteBatch, Color.White, new Vector2(1200, 570), 4f);


        var currentTile = TileCycler?.GetCurrent();
        currentTile.Draw(120, 120, 4f, SpriteEffects.None);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
