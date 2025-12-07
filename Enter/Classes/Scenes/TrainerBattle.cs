using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For example input
using System;
using Enter.Classes.Animations;
using Enter.Classes.Sprites;
using Enter.Classes.Textures;
using Enter.Interfaces;
using Enter.Classes.Characters;
using TrainerMethods;

namespace Enter.Classes.Scenes    
{
    /// <summary>
    /// The scene for a battle against a Trainer.
    /// </summary>
    public class TrainerBattleScene: IGameScene
    {
        // 4-argument constructor for backward compatibility
        public TrainerBattleScene(SceneManager sceneManager, Game game1, string trainerID, Player ourPlayer)
            : this(sceneManager, game1, trainerID, ourPlayer, null) { }
        private string _returnSceneName;
        private Color pokemonBackgroundColor = new Color(246, 232, 248);
        private SceneManager _sceneManager;
        private String _trainerID;
        private Vector2 _enemyPokemonPosition = new Vector2(800, 200);
        private Vector2 _playerPokemonPosition = new Vector2(400, 400);
        private Game _game;
        private Player _player;
        private Team _playersTeam;
        private Team _trainersTeam;
        private TrainerBattleUI _trainerUI;
        private TextureAtlas _PokemonBackAtlas;
        private TextureAtlas _PokemonFrontAtlas;
        private TextureAtlas _UIAtlas;
        private TextureAtlas _BattleCharactersAtlas;
        private TextureAtlas _BordersAtlas;
        private TextSprite trainerText;
        private SpriteFont _font;
        public TrainerBattleScene(SceneManager sceneManager, Game game1, string trainerID, Player ourPlayer, string returnSceneName = null)
        {
            _sceneManager = sceneManager;
            _trainerID = trainerID; // e.g., "TRAINER_BROCK"
            _player = ourPlayer;
            _game = game1;
            // Store the scene to return to after battle
            _returnSceneName = returnSceneName ?? sceneManager.PreviousSceneName ?? "overworld";
        }

        public void LoadContent(ContentManager content)
        {
            PokemonFrontFactory.Instance.LoadAllTextures(content);
            PokemonBackFactory.Instance.LoadAllTextures(content);
            _PokemonBackAtlas = TextureAtlas.FromFile(content, "Pokemon_BACK.xml");
            _PokemonFrontAtlas = TextureAtlas.FromFile(content, "Pokemon_FRONT.xml");
            _UIAtlas = TextureAtlas.FromFile(content, "BattleInterface.xml");
            _BattleCharactersAtlas = TextureAtlas.FromFile(content, "BattleChars.xml");
            _BordersAtlas = TextureAtlas.FromFile(content, "Borders.xml");

            // load from trainer dict
            TrainerTeam trainerTeams = new TrainerTeam();

            // Load Background Music
            BackgroundMusicLibrary.Load(content);
            BackgroundMusicPlayer.Play(SongId.BattleTrainer, loop: true);

            // Load Trainer and their Pokemon
            _trainersTeam = trainerTeams.GetTrainerTeam(_trainerID);
            _playersTeam = _player.thePlayersTeam;

            // Load UI
            _font = content.Load<SpriteFont>("PokemonFont");
            trainerText = new TextSprite($"TRAINER BATTLE", _font, Color.Black);
            _trainerUI = new TrainerBattleUI(_UIAtlas, _BattleCharactersAtlas, _BordersAtlas, content, _trainerID, _playersTeam, _trainersTeam);
            _trainerUI.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            // update UI
            _trainerUI.Update(gameTime);

            if(_trainerUI.didRunOrCatch || _trainerUI.resetBattle){
                    // Save the player's last position for the overworld
                    if ((_game as Game1)?.SavedPlayerPosition is Microsoft.Xna.Framework.Vector2 savedPos)
                    {
                        Enter.Classes.Scenes.OverworldScene.SetNextSpawn(savedPos);
                    }
                    (_game as Game1)?.MarkTrainerDefeated(_trainerID);
                    _sceneManager.TransitionTo("overworld");
                }

            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.Clear(pokemonBackgroundColor);
            // Draw UI elements
            _trainerUI.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
