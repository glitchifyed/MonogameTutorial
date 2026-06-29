using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Graphics
{
    public class AnimatedSprite : Sprite
    {
        public Animation Animation
        {
            get;
            
            set
            {
                CurrentFrame = 0;
                _timeSinceLastFrame = 0f;

                Region = value?.Frames?[0];

                field = value;
            }
        }

        public int CurrentFrame { get; private set; }

        public float PlaybackSpeed { get; private set; } = 1f;

        private float _timeSinceLastFrame;


        public AnimatedSprite(Animation animation)
            => Animation = animation;
        

        public void Update(float deltaTime)
        {
            if (Animation == null)
                return;

            _timeSinceLastFrame += deltaTime * PlaybackSpeed;

            if (_timeSinceLastFrame >= Animation.FrameDelay)
            {
                _timeSinceLastFrame %= Animation.FrameDelay;

                var count = Animation.Frames.Count;

                CurrentFrame = CurrentFrame < count - 1 ? CurrentFrame + 1 : 0;

                Region = Animation.Frames[CurrentFrame];
            }
        }


        public AnimatedSprite SetPlaybackSpeed(float speed)
        {
            PlaybackSpeed = speed;

            return this;
        }

        public AnimatedSprite OffsetPlaybackTime(float timeOffset)
        {
            _timeSinceLastFrame += timeOffset;

            return this;
        }
    }
}