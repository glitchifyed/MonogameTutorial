using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.Components;
using Microsoft.Xna.Framework;

namespace GameLibrary.GameObjects
{
    public class TranslatableGameObject : GameObject
    {
        public Vector2 Position
        {
            get;

            set
            {
                if (field == value)
                    return;

                field = value;

                SendTranslationUpdate(position: value);
            }
        } = Vector2.Zero;
        public Vector2 Scale
        {
            get;

            set
            {
                if (field == value)
                    return;

                field = value;

                SendTranslationUpdate(scale: value);
            }
        } = Vector2.One;
        
        public float Rotation
        {
            get;

            set
            {
                if (field == value)
                    return;

                field = value;

                SendTranslationUpdate(rotation: value);
            }
        } = 0f;


        private void SendTranslationUpdate(Vector2? position = null, Vector2? scale = null, float? rotation = null)
        {
            foreach (var component in Components)
                component.OnTranslationUpdate(position, scale, rotation);
        }
    }
}