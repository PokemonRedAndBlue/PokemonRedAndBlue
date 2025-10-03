using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using MonoGameLibrary.Storage;
using Behavior.Time;
using Enter.Classes.Characters;
using Enter.Classes.Animation;
using ISprite;
using Enter.Classes.Sprites;

namespace GameFile;

public class Game1 : Core
{
    // Needed class vars
    private AnimatedSprite _bulbasuar;
    private AnimatedSprite _ivysaur;
    private AnimatedSprite _venusaur;
    private AnimatedSprite _squirtle;
    private AnimatedSprite _wartortle;
    private AnimatedSprite _blastoise;
    private AnimatedSprite _charmander;
    private AnimatedSprite _charmeleon;
    private AnimatedSprite _charizard;
    private Texture2D character;
    private Player player;
    private Trainer trainer;
    private KeyboardController.KeyboardController controller;
    private Vector2 postion = new Vector2(100, 100);
    private PokeballthrowAnimation _pokeballthrow;
    private PokeballCaptureAnimation _pokeballCapture;

    private HurtAnimation hurt = new HurtAnimation();
    private AttackAnimation attack = new AttackAnimation();

    private DeathAnimation death = new DeathAnimation();

    private List<Tile> _tiles = new List<Tile>();
    public ISprite.TileCycler TileCycler { get; private set; }

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
            Trainer.Facing.Right
        );
        controller = new KeyboardController.KeyboardController();

        _pokeballthrow = new PokeballthrowAnimation(640,360);
        _pokeballthrow.LoadContent(Content);

        _pokeballCapture = new PokeballCaptureAnimation(300, 360);
        _pokeballCapture.LoadContent(Content);

        _tiles = TileLoader.LoadTiles("Content/Tiles.xml");
        TileCycler = new ISprite.TileCycler(_tiles);

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

        controller.Update(this);
        int ax = 0, ay = 0;
        switch (controller.moveDirection)
        {
            case KeyboardController.Direction.Left:
                ax = -1;
                break;
            case KeyboardController.Direction.Right:
                ax =  1;
                break;
            case KeyboardController.Direction.Up:
                ay = -1;
                break;
            case KeyboardController.Direction.Down:
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
