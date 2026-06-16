using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.GameObjects;
using GameLibrary.Graphics;

namespace GameLibrary.Components
{
    public class SpriteRenderer : ObjectComponent
    {
        public Sprite Sprite { get; set; }
    }
}