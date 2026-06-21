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

                SendTranslationUpdate(position: Vector2.Zero);
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

                SendTranslationUpdate(scale: Vector2.One);
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

                SendTranslationUpdate(rotation: 0f);
            }
        } = 0f;

        public Vector2 AncestralPosition { get; private set; } = Vector2.Zero;
        public Vector2 AncestralScale { get; private set; } = Vector2.One;
        public float AncestralRotation { get; private set; } = 0f;

        // public Vector2 GlobalPosition => AncestralPosition + Position * Scale;
        // public Vector2 GlobalScale => AncestralScale * Scale;
        // public float GlobalRotation => AncestralRotation + Rotation;

        public Vector2 GlobalPosition { get; private set; } = Vector2.Zero;
        public Vector2 GlobalScale { get; private set; } = Vector2.One;
        public float GlobalRotation { get; private set; } = 0f;


        private void UpdateGlobalTranslation(Vector2 parentalPosition, Vector2 parentalScale, float parentalRotation)
        {
            AncestralPosition = parentalPosition;
            AncestralScale = parentalScale;
            AncestralRotation = parentalRotation;

            GlobalPosition = parentalPosition + Vector2.Rotate(Position * parentalScale, parentalRotation);
            GlobalScale = parentalScale * Scale;
            GlobalRotation = parentalRotation + Rotation;

            foreach (var child in Children)
            {
                
            }
        }

        private void SendTranslationUpdate(Vector2? position = null, Vector2? scale = null, float? rotation = null)
        {
            UpdateGlobalTranslation(AncestralPosition, AncestralScale, AncestralRotation);

            foreach (var component in Components)
                component.OnTranslationUpdate(GlobalPosition, GlobalScale, GlobalRotation);
        }
    }
}