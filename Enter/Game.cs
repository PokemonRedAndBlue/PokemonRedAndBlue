using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;
using System.Data;
using System.Threading.Tasks;
using MonoGameLibrary.Storage;

namespace GameFile;

public class Game1 : Core
{  
    public Game1() : base("PokemonRedAndBlue", 1280, 720, false)
    {

    }

    private PlayerData _playerData = new();
    private string _savePath = string.Empty;
    private KeyboardState _previousKeyboardState;

    protected override void Initialize()
    {
        base.Initialize();

        // load or create default save
        _savePath = SaveManager.GetDefaultSavePath();
        _playerData = SaveManager.Load(_savePath) ?? new PlayerData();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboard = Keyboard.GetState();

        // Save on F5 (edge-triggered)
        if (keyboard.IsKeyDown(Keys.F5) && !_previousKeyboardState.IsKeyDown(Keys.F5))
        {
            try
            {
                SaveManager.SaveAsync(_playerData, _savePath).GetAwaiter().GetResult();
                Console.WriteLine($"Saved player data to {_savePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save: {ex.Message}");
            }
        }

        _previousKeyboardState = keyboard;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
