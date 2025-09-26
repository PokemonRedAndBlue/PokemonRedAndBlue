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

namespace GameFile;

public class Game1 : Core
{
    // Needed class vars
    private Sprite _bulbasaur;
    private Texture2D character;
    private Player player;
    private KeyboardController.KeyboardController controller;

        private Vector2 postion = new Vector2(100, 100);

        private PokemonState.SpriteState spriteState = PokemonState.SpriteState.Idle;

        private HurtAnimation hurt = new HurtAnimation();
        private AttackAnimation attack = new AttackAnimation();

        private DeathAnimation death = new DeathAnimation();

        public Game1() : base("PokemonRedAndBlue", 1280, 720, false) { }

        protected override void LoadContent()
        {
            PokemonFrontFactory.Instance.LoadAllTextures(Content);
            PokemonBackFactory.Instance.LoadAllTextures(Content);

        _bulbasaur = PokemonBackFactory.Instance.CreateStaticSprite("bulbasaur-back");
        character = Content.Load<Texture2D>("images/Pokemon_Characters");
        player = new Player(character, Window);
        controller = new KeyboardController.KeyboardController();
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
            case KeyboardController.Direction.Left:  ax = -1; break;
            case KeyboardController.Direction.Right: ax =  1; break;
            case KeyboardController.Direction.Up:    ay = -1; break;
            case KeyboardController.Direction.Down:  ay =  1; break;
            default:                                  ax =  0; ay = 0; break;
        }

        player.Update(gameTime, ax, ay);
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

        player.Draw(SpriteBatch, 4f);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
