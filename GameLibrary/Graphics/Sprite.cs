using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics
{
    public class Sprite
    {
        public TextureRegion Region { get; set; }
        public Color Color { get; set; } = Color.White;

        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Origin { get; set; } = new Vector2(0.5f, 0.5f);
        public Vector2 Scale { get; set; } = Vector2.One;
        public float Rotation { get; set; } = 0f;
        public float Depth { get; set; } = 0f;

        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public float Width => Region.Width * Scale.X;
        public float Height => Region.Height * Scale.Y;


        public Sprite() { }

        public Sprite(TextureRegion region)
        {
            Region = region;
        }


        public void Draw(SpriteBatch spriteBatch) => Region.Draw(spriteBatch, Position, new Vector2(Origin.X * Region.Width, Origin.Y * Region.Height), Color, Scale, Rotation, Depth, Effects);
    }
}