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

    public static int ScreenWidth => Graphics.GraphicsDevice.Viewport.Width;
    public static int ScreenHeight => Graphics.GraphicsDevice.Viewport.Height;

    public static int VirtualScreenWidth { get; } = 640;
    public static int VirtualScreenHeight { get; } = 360;
    
    public static float ScreenScale => Math.Min(ScreenWidth * (9f / 16f), ScreenHeight) / VirtualScreenHeight;

    public static GameObject GameRoot { get; private set; }
    public static Camera Camera { get; private set; }

    private static float _timeSinceFixedUpdate { get; set; }
    private static float _fixedDeltaTimeMax { get; } = 1f / 60f;


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
        Graphics = new GraphicsDeviceManager(this)
        {
            // Set the graphics defaults.
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = fullScreen
        };

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

        GameRoot = GameObject.Create("Root");
        Camera = GameObject.Create<Camera>("MainCamera", GameRoot);
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

        LoopThroughComponents(GameRoot, deltaTime, PushComponentUpdate);

        base.Update(gameTime);
    }

    protected virtual void FixedUpdate(float deltaTime)
    {
        LoopThroughComponents(GameRoot, deltaTime, PushComponentFixedUpdate);
    }


    private void LoopThroughComponents(GameObject gameObject, float deltaTime, Action<ObjectComponent, float> callback)
    {
        if (!gameObject.Active)
            return;

        foreach (var component in gameObject.Components)
            callback(component, deltaTime);
        
        foreach (var child in gameObject.Children)
            LoopThroughComponents(child, deltaTime, callback);
    }

    private void PushComponentUpdate(ObjectComponent component, float deltaTime) => component.OnUpdate(deltaTime);
    private void PushComponentFixedUpdate(ObjectComponent component, float deltaTime) => component.OnFixedUpdate(deltaTime);


    protected void DrawSprites(SpriteBatch spriteBatch, GameObject gameObject)
    {
        if (!gameObject.Active)
            return;
        
        // TODO: Draw in order of textures to optimise (in order of TextureAtlas)?
        //      For this optimisation I'd have to figure out a better way of getting the parental offset..
        //      That's future me's problem!
        
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer?.Sprite?.Draw(spriteBatch);

        foreach (var child in gameObject.Children)
            DrawSprites(spriteBatch, child);
    }
}
