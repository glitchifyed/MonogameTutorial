using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.Components;

namespace GameLibrary.GameObjects
{
    public class GameObject
    {
        private readonly List<GameObject> _children = [];
        private readonly List<ObjectComponent> _components = [];


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
        public IReadOnlyList<ObjectComponent> Components => _components;


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


        public bool HasComponent<T>() where T : ObjectComponent
        {
            return _components.Exists(component => component is T);
        }

        public T GetComponent<T>() where T : ObjectComponent
        {
            if (!HasComponent<T>())
                return null;

            return (T)_components.Find(component => component is T);
        }

        public T AddComponent<T>() where T : ObjectComponent
        {
            if (HasComponent<T>())
                throw new InvalidOperationException($"Cannot add component of type '{typeof(T).FullName}' to '{GetType().FullName}' named '{Name}', as it already exists");

            var component = Activator.CreateInstance<T>();

            component.SetGameObject(this);
            _components.Add(component);

            return component;
        }

        public void RemoveComponent<T>() where T : ObjectComponent
        {
            var component = GetComponent<T>() ?? throw new InvalidOperationException($"Cannot remove component of type '{typeof(T).FullName}' from '{GetType().FullName}' named '{Name}', as it doesn't currently exist");

            _components.Remove(component);
        }


        protected virtual void OnParentChanged(GameObject oldParent, GameObject newParent) { }
    }
}