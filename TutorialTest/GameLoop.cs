using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using GameLibrary.Graphics;
using System;

namespace TutorialTest;

public class GameLoop : Core
{
    private TextureRegion _uglyRedBall;
    private TextureRegion _uglyBlueBall;

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
        // var atlas = new TextureAtlas(Content.Load<Texture2D>("images/ballatlas"));

        // _uglyRedBall = atlas.AddRegion("uglyredball", 0, 0, 16, 16);
        // _uglyBlueBall = atlas.AddRegion("uglyblueball", 16, 0, 16, 16);

        var atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        _uglyRedBall = atlas.GetRegion("uglyredball");
        _uglyBlueBall = atlas.GetRegion("uglyblueball");
    }

    protected override void Update(GameTime gameTime)
    {
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        //     Exit();

        // TODO: Add your update logic here

        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        var elapsed = gameTime.TotalGameTime.TotalSeconds;

        _uglyRedBall.Draw(SpriteBatch, new Vector2(0f, 40f * (float)Math.Sin(elapsed * 5f)), scale: 10f);
        _uglyBlueBall.Draw(SpriteBatch, new Vector2(_uglyRedBall.Width * 10f + 25f, 50f * (float)Math.Sin(elapsed * 4f)), scale: 10f);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
