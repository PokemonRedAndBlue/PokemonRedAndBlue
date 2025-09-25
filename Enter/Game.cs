using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace GameFile;

public class Game1 : Core
{
    // Needed class vars
    private Sprite _bulbasaur;

    private Vector2 postion = new Vector2(100, 100);

    private PokemonState.SpriteState spriteState = PokemonState.SpriteState.Idle;

    AnimationKernel kernel = new AnimationKernel();

    AttackAnimation attack = new AttackAnimation();

    HurtAnimation hurt = new HurtAnimation();

    public Game1() : base("PokemonRedAndBlue", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        PokemonFrontFactory.Instance.LoadAllTextures(Content);
        PokemonBackFactory.Instance.LoadAllTextures(Content);

        _bulbasaur = PokemonBackFactory.Instance.CreateStaticSprite("bulbasaur-back");
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        if (keyboardState.IsKeyDown(Keys.W))
        {
            kernel.StartAnimation(postion);
            spriteState = PokemonState.SpriteState.Hurt;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Update(gameTime);

        switch (spriteState)
        {
            case PokemonState.SpriteState.Idle:
                _bulbasaur.Draw(SpriteBatch, postion, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.5f);
                break;
            case PokemonState.SpriteState.Attack:
                spriteState = attack.UpdateAttackAnimation(postion, _bulbasaur, SpriteBatch);
                break;
            case PokemonState.SpriteState.Hurt:
                spriteState = hurt.UpdateHurtAnimation(postion, _bulbasaur, SpriteBatch);
                break;
            default:
                break;
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
