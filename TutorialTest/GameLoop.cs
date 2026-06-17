using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using GameLibrary.Graphics;
using System;
using GameLibrary.GameObjects;
using GameLibrary.Components;

namespace TutorialTest;

public class GameLoop : Core
{
    public GameLoop() : base("Tutorial Game", 1920, 1080, false)
    {
        
    }


    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        var atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        for (int i = 0; i < 10; i++)
        {
            var ball = GameObject.Translatable($"Ball {i + 1}", GameRoot, new SpriteRenderer(atlas.CreateSprite("uglyredball")));
            ball.Scale = Vector2.One * 3f;
        }
    }

    protected override void Update(GameTime gameTime)
    {
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        //     Exit();

        var elapsed = gameTime.TotalGameTime.TotalSeconds;

        for (var i = 0; i < GameRoot.Children.Count; i++)
        {
            var child = (TranslatableGameObject)GameRoot.Children[i];

            child.Position = new Vector2((i - 4.5f) * 16f * (child.Scale.X + 0.5f), (float)Math.Sin(elapsed * 5f + i) * 20f);
        }

        base.Update(gameTime);
    }

    protected override void FixedUpdate(float deltaTime)
    {
        
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        DrawSprites(SpriteBatch, GameRoot);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
