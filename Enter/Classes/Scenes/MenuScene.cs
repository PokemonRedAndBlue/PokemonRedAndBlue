using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Enter.Interfaces;

namespace Enter.Classes.Scenes
{
    public class MenuScene : IGameScene
    {
        private readonly SceneManager _sceneManager;
        private readonly Game1 _game;
        private SpriteFont _font;

        private string[] options = { "Start Game", "Exit" };
        private int selected = 0;
        private KeyboardState prev;
        private double _inputDelay = 1;   

        private Texture2D _menuImage;


        public MenuScene(SceneManager sm, Game1 game)
        {
            _sceneManager = sm;
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("PokemonFont");
            _menuImage = content.Load<Texture2D>("images/menu");
            SoundEffectLibrary.Load(content);

        }

        public void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();

            _inputDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            if (_inputDelay > 0)
                return;

            if (IsPressed(kb, Keys.Up)) selected = (selected - 1 + options.Length) % options.Length;
            if (IsPressed(kb, Keys.Down)) selected = (selected + 1) % options.Length;

            if (IsPressed(kb, Keys.Enter))
            {
                SoundEffectPlayer.Play(SfxId.SFX_PRESS_AB);
                if (selected == 0)     // Start Game
                    _sceneManager.TransitionTo("oakIntro");
                else if (selected == 1)
                    _game.Exit();
            }

            prev = kb;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(new Color(248,248,248));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            int W = _game.Window.ClientBounds.Width;
            int H = _game.Window.ClientBounds.Height;

            float startY = H * 0.4f;
            float spacing = 50f;

            float scale = 0.5f;

            float imgW = _menuImage.Width * scale;
            float imgH = _menuImage.Height * scale;

            Vector2 menuPos = new Vector2(
                (W - imgW) / 2f,
                H * 0.28f
            );

            spriteBatch.Draw(
                _menuImage,
                menuPos,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );

            for (int i = 0; i < options.Length; i++)
            {
                string text = options[i];
                Vector2 size = _font.MeasureString(text);
                Vector2 pos = new Vector2((W - size.X) / 2, startY + spacing * i);

                Color color = (i == selected ? Color.Black : Color.Gray);

                spriteBatch.DrawString(_font, text, pos, color);
            }

            spriteBatch.End();
        }

        private bool IsPressed(KeyboardState kb, Keys key)
        {
            return kb.IsKeyDown(key) && !prev.IsKeyDown(key);
        }
    }
}
