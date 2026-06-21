using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.GameObjects
{
    public class Camera : GameObject
    {
        public static Camera Main { get; private set; }


        public float Zoom { get; set; } = 1f;


        public static void SetMainCamera(Camera camera)
            => Main = camera;
        

        protected override void OnCreated()
        {
            if (Main == null)
                SetMainCamera(this);
        }

        protected override void OnDestroyed()
        {
            if (Main == this)
                Main = null;
        }
    }
}