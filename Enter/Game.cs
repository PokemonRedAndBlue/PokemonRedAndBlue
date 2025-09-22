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

namespace GameFile;

public class Game1 : Core
{
    // Needed class vars
    private Sprite _bulbasaur;

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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // all testing code goes here
        _bulbasaur.Draw(SpriteBatch, new Vector2(100, 100), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.5f);
        var keyboardState = Keyboard.GetState();
        Time timer = new Time();
        if (keyboardState.IsKeyDown(Keys.W))
        {
            AttackAnimation attackAnimation = new AttackAnimation();
            attackAnimation.BackAttackAnimation(_bulbasaur, SpriteBatch, timer);
        }
        // end testing code

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
