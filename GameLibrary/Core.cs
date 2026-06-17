using System;
using GameLibrary.Components;
using GameLibrary.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary;

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

    public static int ScreenWidth => s_instance.Window.ClientBounds.Width;
    public static int ScreenHeight => s_instance.Window.ClientBounds.Height;

    public static float ScreenScale => Math.Min(ScreenWidth * (9f / 16f), ScreenHeight) / 360f;

    public static GameObject GameRoot { get; private set; }

    private static float _timeSinceFixedUpdate;
    private const float _fixedDeltaTimeMax = 1f / 60f;


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

        Window.AllowUserResizing = true;

        // Mouse is visible by default.
        IsMouseVisible = true;

        IsFixedTimeStep = false;
        Graphics.SynchronizeWithVerticalRetrace = true;

        //GameRoot = new("Root");
        GameRoot = GameObject.Create("Root");
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
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _timeSinceFixedUpdate += deltaTime;

        if (_timeSinceFixedUpdate >= _fixedDeltaTimeMax)
        {
            _timeSinceFixedUpdate %= _fixedDeltaTimeMax;

            FixedUpdate(Math.Max(_fixedDeltaTimeMax, deltaTime));
        }

        base.Update(gameTime);
    }


    protected virtual void FixedUpdate(float deltaTime) { }


    protected void DrawSprites(SpriteBatch spriteBatch, GameObject gameObject)
    {
        foreach (var child in gameObject.Children)
            DrawSprites(spriteBatch, child);
        
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        spriteRenderer?.Sprite?.Draw(spriteBatch);
    }
}
