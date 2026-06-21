using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Graphics
{
    public class Animation(int fps, List<TextureRegion> frames)
    {
        public List<TextureRegion> Frames { get; private set; } = frames;
        public float FrameDelay { get; private set; } = 1f / fps;
    }
}