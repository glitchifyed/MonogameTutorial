using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.GameObjects;

namespace GameLibrary.Components
{
    public abstract class ObjectComponent
    {
        public GameObject GameObject { get; private set; }
        

        public void SetGameObject(GameObject gameObject)
        {
            if (GameObject != null)
                return;

            GameObject = gameObject;
        }
    }
}