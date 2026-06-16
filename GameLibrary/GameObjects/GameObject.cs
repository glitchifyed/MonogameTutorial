using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.GameObjects
{
    public class GameObject
    {
        private readonly List<GameObject> _children = [];


        public string Name { get; set; }

        public GameObject Parent
        {
            get;

            set
            {
                if (field == value)
                    return;
                
                if (value == this)
                    throw new InvalidOperationException("Cannot set the parent of a GameObject to itself");

                field?._children.Remove(this);
                value?._children.Add(this);

                OnParentChanged(field, value);

                field = value;
            }
        }

        public IReadOnlyList<GameObject> Children => _children;


        public GameObject(string name = "New GameObject", GameObject parent = null)
        {
            Name = name;
            Parent = parent;
        }


        public bool FindChild(string name, out GameObject result)
        {
            foreach (var child in _children)
            {
                if (child.Name != name)
                    continue;
                
                result = child;

                return true;
            }

            result = null;

            return false;
        }

        public bool FindChild(string name) => FindChild(name, out _);


        protected virtual void OnParentChanged(GameObject oldParent, GameObject newParent) { }
    }
}