using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.Components;
using Microsoft.Xna.Framework;

namespace GameLibrary.GameObjects
{
    public class GameObject
    {
        private readonly List<GameObject> _children = [];
        private readonly List<ObjectComponent> _components = [];


        public string Name { get; set; }

        public bool Active { get; set; } = true;

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

                GlobalPosition = (value?.GlobalPosition ?? Vector2.Zero) + Position;
                GlobalScale = (value?.GlobalScale ?? Vector2.Zero) + Scale;
                GlobalRotation = (value?.GlobalRotation ?? 0f) + Rotation;

                field = value;

                OnParentChanged(field, value);
            }
        }

        public IReadOnlyList<GameObject> Children => _children;
        public IReadOnlyList<ObjectComponent> Components => _components;


        // Translation components
        public Vector2 GlobalPosition
        {
            get;

            set
            {
                if (field == value)
                    return;
                
                field = value;

                OnTranslationUpdate();
            }
        }
        public Vector2 GlobalScale
        {
            get;

            set
            {
                if (field == value)
                    return;
                
                field = value;

                OnTranslationUpdate();
            }
        }
        public float GlobalRotation
        {
            get;

            set
            {
                if (field == value)
                    return;
                
                field = value;

                OnTranslationUpdate();
            }
        }

        public Vector2 Position
        {
            get
                => Parent == null ? GlobalPosition : GlobalPosition - Parent.GlobalPosition;

            set
            {
                if (field == value)
                    return;
                
                var parentPosition = Parent?.GlobalPosition ?? Vector2.Zero;

                GlobalPosition = parentPosition + value;
            }
        }

        public Vector2 Scale
        {
            get
                => Parent == null ? GlobalScale : GlobalScale - Parent.GlobalScale;

            set
            {
                if (field == value)
                    return;
                
                var parentScale = Parent?.GlobalScale ?? Vector2.Zero;

                GlobalScale = parentScale + value;
            }
        }

        public float Rotation
        {
            get
                => Parent == null ? GlobalRotation : GlobalRotation - Parent.GlobalRotation;

            set
            {
                if (field == value)
                    return;
                
                var parentRotation = Parent?.GlobalRotation ?? 0f;

                GlobalRotation = parentRotation + value;
            }
        }


        public static T Create<T>(string name = "New GameObject", GameObject parent = null, params ObjectComponent[] components) where T : GameObject
        {
            var gameObject = Activator.CreateInstance<T>();

            gameObject.Name = name;
            gameObject.Parent = parent;

            foreach (var component in components)
                gameObject.AddComponent(component);
            
            gameObject.OnCreated();
            
            return gameObject;
        }

        public static GameObject Create(string name = "New GameObject", GameObject parent = null, params ObjectComponent[] components) => Create<GameObject>(name, parent, components);


        private void AddComponentInternal(ObjectComponent component)
        {
            component.SetGameObject(this);
            _components.Add(component);
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

            AddComponentInternal(component);

            return component;
        }

        public T AddComponent<T>(T component) where T : ObjectComponent
        {
            if (HasComponent<T>())
                throw new InvalidOperationException($"Cannot add component of type '{typeof(T).FullName}' to '{GetType().FullName}' named '{Name}', as it already exists");
            
            AddComponentInternal(component);

            return component;
        }

        public void RemoveComponent<T>() where T : ObjectComponent
        {
            var component = GetComponent<T>() ?? throw new InvalidOperationException($"Cannot remove component of type '{typeof(T).FullName}' from '{GetType().FullName}' named '{Name}', as it doesn't currently exist");

            _components.Remove(component);
        }


        public void Destroy()
        {
            foreach (var child in _children)
                child.Destroy();
            
            _children.Clear();
            _components.Clear();

            Parent = null;

            OnDestroyed();
        }


        protected virtual void OnParentChanged(GameObject oldParent, GameObject newParent) { }
        protected virtual void OnCreated() { }
        protected virtual void OnDestroyed() { }


        // Translation
        private void OnTranslationUpdate()
        {
            foreach (var component in Components)
                component.OnTranslationUpdate(GlobalPosition, GlobalScale, GlobalRotation);

            foreach (var child in _children)
                child.OnTranslationUpdate();
        }
    }
}