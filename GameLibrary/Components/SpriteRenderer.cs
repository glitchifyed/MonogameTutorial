using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.GameObjects;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace GameLibrary.Components
{
    public class SpriteRenderer : ObjectComponent
    {
        public Sprite Sprite { get; set; }

        
        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
        }


        public override void OnTranslationUpdate(Vector2? position, Vector2? scale, float? rotation)
        {
            if (position.HasValue)
                Sprite.Position = position.Value;
            
            if (scale.HasValue)
                Sprite.Scale = scale.Value;
            
            if (rotation.HasValue)
                Sprite.Rotation = rotation.Value;
        }


        public override void OnUpdate(float deltaTime)
        {
            if (Sprite is not AnimatedSprite animatedSprite)
                return;
            
            animatedSprite.Update(deltaTime);
        }
    }
}