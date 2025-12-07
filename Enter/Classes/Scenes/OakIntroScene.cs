using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Enter.Interfaces;

namespace Enter.Classes.Scenes
{
    public class OakIntroScene : IGameScene
    {
        private readonly SceneManager _sceneManager;
        private readonly Game1 _game;

        private SpriteFont _font;

        private Texture2D _oakTex;
        private Texture2D _pokemonTex;
        private Texture2D _playerTex;
        private Texture2D _rivalTex;
        private Texture2D _textboxTex;

        private KeyboardState _prevKeyboard;

        private enum Speaker
        {
            Oak,
            Pokemon,
            Player,
            Rival
        }

        private struct DialogueEntry
        {
            public Speaker Speaker;
            public string Text;

            public DialogueEntry(Speaker speaker, string text)
            {
                Speaker = speaker;
                Text = text;
            }
        }

        private DialogueEntry[] _dialogue =
        {
            // Professor Oak
            new DialogueEntry(Speaker.Oak,     "Hello  there!"),
            new DialogueEntry(Speaker.Oak,     "Welcome  To  the\nWorld  of  Pokemon!"),
            new DialogueEntry(Speaker.Oak,     "My  Name  is  Oak!\nPeople  call  me"),
            new DialogueEntry(Speaker.Oak,     "the  Pokemon  PROF!"),

            // Pokemon
            new DialogueEntry(Speaker.Pokemon, "This world is inhabited \nby  creatures called"),
            new DialogueEntry(Speaker.Pokemon, "POKEMON!"),
            new DialogueEntry(Speaker.Pokemon, "For  some  people, \nPOKEMON  are  pets."),
            new DialogueEntry(Speaker.Pokemon, "Others  use  them  \nfor  fights."),
            new DialogueEntry(Speaker.Pokemon, "Myself. . ."),
            new DialogueEntry(Speaker.Pokemon, "I  study  POKEMON  as \na  profession."),

            // Player
            new DialogueEntry(Speaker.Player,  "Nice to meet you!"),

            // Rival
            new DialogueEntry(Speaker.Rival,   "This  is  my  grandson.\nHe's  been  your  rival"),
            new DialogueEntry(Speaker.Rival,   "since  you  were  a  baby."),

            // Player
            new DialogueEntry(Speaker.Player,  "Your  very  own\nPOKEMON  legend  is"),
            new DialogueEntry(Speaker.Player,  "about  to  unfold!"),
            new DialogueEntry(Speaker.Player,  "A  world  of  dreams\nand  adventures"),
            new DialogueEntry(Speaker.Player,  "with  POKEMON\nawaits!  Let's  go!")
        };

        private int _currentIndex = 0;

        public OakIntroScene(SceneManager sceneManager, Game1 game)
        {
            _sceneManager = sceneManager;
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            _font        = content.Load<SpriteFont>("PokemonFont");
            _oakTex      = content.Load<Texture2D>("images/oak");
            _pokemonTex  = content.Load<Texture2D>("images/introPokemon");
            _playerTex   = content.Load<Texture2D>("images/introPlayer");
            _rivalTex    = content.Load<Texture2D>("images/rival");
            _textboxTex  = content.Load<Texture2D>("images/textbox");
            SoundEffectLibrary.Load(content);
        }

        public void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();

            bool enterPressed = kb.IsKeyDown(Keys.Enter) &&
                                !_prevKeyboard.IsKeyDown(Keys.Enter);

            if (enterPressed)
            {
                // Go to next line, or finish
                SoundEffectPlayer.Play(SfxId.SFX_PRESS_AB);
                _currentIndex++;

                if (_currentIndex >= _dialogue.Length)
                {
                    _sceneManager.TransitionTo("overworld");
                    _currentIndex = _dialogue.Length - 1;
                }
            }

            _prevKeyboard = kb;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(new Color(248,248,248));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            int W = _game.Window.ClientBounds.Width;
            int H = _game.Window.ClientBounds.Height;

            DialogueEntry entry = _dialogue[_currentIndex];

            // ===== Character sprite =====
            Texture2D speakerTex = GetSpeakerTexture(entry.Speaker);

            float charScale = 4f;  
            float charW = speakerTex.Width * charScale;
            float charH = speakerTex.Height * charScale;

            Vector2 charPos = new Vector2(
                (W - charW) / 2f,
                H * 0.15f
            );

            spriteBatch.Draw(
                speakerTex,
                charPos,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                charScale,
                SpriteEffects.None,
                0f
            );

            // ===== Text box at bottom =====
            float boxScale = (W * 0.9f) / _textboxTex.Width;   
            float boxW = _textboxTex.Width * boxScale;
            float boxH = _textboxTex.Height * boxScale;

            Vector2 boxPos = new Vector2(
                (W - boxW) / 2f,
                H - boxH - 20f          
            );

            Rectangle boxDest = new Rectangle(
                (int)boxPos.X,
                (int)boxPos.Y,
                (int)boxW,
                (int)boxH
            );

            spriteBatch.Draw(_textboxTex, boxDest, Color.White);

            // ===== Text inside the box =====
            string text = entry.Text;

            // small margin inside the box
            Vector2 textPos = new Vector2(
                boxDest.X + 50,
                boxDest.Y + 50
            );

            float textScale = 4.0f;  
            spriteBatch.DrawString(
                _font, text, textPos, 
                Color.Black, 0f, 
                Vector2.Zero, 
                textScale, 
                SpriteEffects.None, 
                0f
            );


            spriteBatch.End();
        }

        private Texture2D GetSpeakerTexture(Speaker s)
        {
            switch (s)
            {
                case Speaker.Oak:     return _oakTex;
                case Speaker.Pokemon: return _pokemonTex;
                case Speaker.Player:  return _playerTex;
                case Speaker.Rival:   return _rivalTex;
                default:              return _oakTex;
            }
        }
    }
}
