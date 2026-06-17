using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics
{
    public class TextureRegion
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRect { get; set; }

        public int Width => SourceRect.Width;
        public int Height => SourceRect.Height;


        public TextureRegion(Texture2D texture, int x, int y, int width, int height)
        {
            Texture = texture;
            SourceRect = new Rectangle(x, y, width, height);
        }

        
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, Color? color = null, Vector2? scale = null, float rotation = 0f, float depth = 0f, SpriteEffects effects = SpriteEffects.None)
        {
            spriteBatch.Draw(Texture, position + new Vector2(Core.ScreenWidth * 0.5f, Core.ScreenHeight * 0.5f), SourceRect, color ?? Color.White, rotation, origin, scale ?? Vector2.One, effects, depth);
        }
    }
}