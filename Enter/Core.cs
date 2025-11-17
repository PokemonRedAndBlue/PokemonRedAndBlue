using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enter;

public class Core : Game
{
    internal static Core s_instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static Core Instance => s_instance;

    /// <summary>
    /// Gets the graphics device manager to control the presentation of graphics.
    /// </summary>
    public static GraphicsDeviceManager Graphics { get; private set; }

    /// <summary>
    /// Gets the graphics device used to create graphical resources and perform primitive rendering.
    /// </summary>
    public static new GraphicsDevice GraphicsDevice { get; private set; }

    /// <summary>
    /// Gets the sprite batch used for all 2D rendering.
    /// </summary>
    public static SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    /// Gets the content manager used to load global assets.
    /// </summary>
    public static new ContentManager Content { get; private set; }

    // Simple FPS tracking
    private double _fpsElapsed = 0.0;
    private int _frameCounter = 0;
    private int _fps = 0;
    /// <summary>
    /// Exposes the most recently measured FPS to other classes.
    /// </summary>
    public static int CurrentFps { get; private set; } = 0;

    /// <summary>
    /// Creates a new Core instance.
    /// </summary>
    /// <param name="title">The title to display in the title bar of the game window.</param>
    /// <param name="width">The initial width, in pixels, of the game window.</param>
    /// <param name="height">The initial height, in pixels, of the game window.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    public Core(string title, int width, int height, bool fullScreen)
    {
        // Ensure that multiple cores are not created.
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        // Store reference to engine for global member access.
        s_instance = this;

        // Create a new graphics device manager.
        Graphics = new GraphicsDeviceManager(this);

        // Set the graphics defaults.
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;

        // Apply the graphic presentation changes.
        Graphics.ApplyChanges();

        // Set the window title.
        Window.Title = title;

        // Set the core's content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;

        // Set the root directory for content.
        Content.RootDirectory = "Content";

        // Mouse is visible by default.
        IsMouseVisible = true;

        // By default disable fixed time step and VSync to reduce input-to-display latency.
        // If you prefer a steady 60 FPS with potentially higher input latency, set these differently.
        Graphics.SynchronizeWithVerticalRetrace = false; // disable VSync
        IsFixedTimeStep = false;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Set the core's graphics device to a reference of the base Game's
        // graphics device.
        GraphicsDevice = base.GraphicsDevice;

        // Create the sprite batch instance.
        SpriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        // FPS counter: count frames and update once per second in window title
        _frameCounter++;
        _fpsElapsed += gameTime.ElapsedGameTime.TotalSeconds;
        if (_fpsElapsed >= 1.0)
        {
            _fps = _frameCounter;
            _frameCounter = 0;
            _fpsElapsed -= 1.0;

            // publish for callers
            CurrentFps = _fps;

            // Keep base title (text before '|') and append FPS
            string baseTitle = Window.Title?.Split('|')[0].Trim() ?? string.Empty;
            Window.Title = string.IsNullOrEmpty(baseTitle) ? $"FPS: {_fps}" : $"{baseTitle} | FPS: {_fps}";
        }

        base.Update(gameTime);
    }
}
