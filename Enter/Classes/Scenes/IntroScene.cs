using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Enter.Interfaces;
using Enter.Classes.Sprites;
using Enter.Classes.Animations;

namespace Enter.Classes.Scenes
{
    public class IntroScene : IGameScene
    {
        private readonly SceneManager _sceneManager;
        private readonly Game1 _game;

        private SpriteFont _font;
        private Texture2D _background;   

        private AnimatedSprite _currentPokemon;
        private double _pokemonTimer = 0.0;
        private const double PokemonInterval = 2.0;

        private readonly string[] _pokemonAnimationNames =
        {
            "bulbasaur-front",
            "charmander-front",
            "squirtle-front",
            "pikachu-front",
            "diglett-front",
            "meowth-front",
            "psyduck-front",
            "jigglypuff-front",
            "clefairy-front",
            "mankey-front"
        };

        private readonly Random _rng = new Random();

        public IntroScene(SceneManager sceneManager, Game1 game)
        {
            _sceneManager = sceneManager;
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("PokemonFont");

            // Load intro background image
            BackgroundMusicLibrary.Load(content);
            _background = content.Load<Texture2D>("images/intro1");

            PokemonFrontFactory.Instance.LoadAllTextures(content);
            
            BackgroundMusicLibrary.Load(content);
            BackgroundMusicPlayer.Play(SongId.OpeningPart2, loop: true);

            SoundEffectLibrary.Load(content);

            PickRandomPokemon();
        }

        public void Update(GameTime gameTime)
        {
            _pokemonTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_pokemonTimer >= PokemonInterval)
            {
                _pokemonTimer = 0.0;
                PickRandomPokemon();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SoundEffectPlayer.Play(SfxId.SFX_CRY_19);
                _sceneManager.TransitionTo("menu");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(new Color(245, 232, 247));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            int W = _game.Window.ClientBounds.Width;
            int H = _game.Window.ClientBounds.Height;

            // --- Draw Background ---
            spriteBatch.Draw(
                _background,
                destinationRectangle: new Rectangle(
                    (W/2)-_background.Width/4,
                    (H/2)-_background.Width/4,
                    _background.Width/2,
                    _background.Width/2
                ),
                color: Color.White
            );

            // --- Random Pokémon Sprite ---
            if (_currentPokemon != null)
            {
                Vector2 pokePos = new Vector2(W / 2.3f, H / 1.4f);
                _currentPokemon.Draw(spriteBatch, Color.White, pokePos, 2f);
            }

            // --- Press Enter Text ---
            string press = "Press  enter  to  continue";
            Vector2 pSize = _font.MeasureString(press);
            Vector2 pPos = new Vector2((W - pSize.X) / 2, H * 0.82f);
            spriteBatch.DrawString(_font, press, pPos, Color.Black);

            spriteBatch.End();
        }

        private void PickRandomPokemon()
        {
            string name = _pokemonAnimationNames[_rng.Next(_pokemonAnimationNames.Length)];
            _currentPokemon = PokemonFrontFactory.Instance.CreateAnimatedSprite(name);
            _currentPokemon.CenterOrigin();
        }
    }
}
