using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
    public class InputAction
    {
        private static readonly List<InputAction> _inputActions = [];
        public static IReadOnlyList<InputAction> InputActions => _inputActions;


        private readonly InputBinding[] _inputBindings;

        private readonly List<Action<bool, int>> _callbacks = [];
        
        public ReadOnlyCollection<InputBinding> InputBindings => Array.AsReadOnly(_inputBindings);


        public InputAction(params InputBinding[] inputBindings)
        {
            _inputBindings = inputBindings;

            _inputActions.Add(this);
        }


        public void AddCallback(Action<bool, int> callback)
            => _callbacks.Add(callback);
        
        public void RunCallbacks(bool pressed, int deviceId)
        {
            foreach (var callback in _callbacks)
                callback(pressed, deviceId);
        }
    }
}