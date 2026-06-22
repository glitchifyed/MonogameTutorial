using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.Input.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
    public static class InputManager
    {
        private static KeyboardState _currentKeyboardState = new();
        private static KeyboardState _previousKeyboardState = new();

        private static MouseState _currentMouseState = new();
        private static MouseState _previousMouseState = new();

        private static readonly List<GamepadIndex> _gamepadIndexes = [];

        public static int DeviceCount { get; private set; } = 1;


        public static void Initialize()
        {
            for (var gamepadId = 0; gamepadId < GamePad.MaximumGamePadCount; gamepadId++)
                _gamepadIndexes.Add(new GamepadIndex((PlayerIndex)gamepadId));
        }

        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var deviceCount = 1;

            foreach (var gamepadIndex in _gamepadIndexes)
            {
                gamepadIndex.Update();

                if (gamepadIndex.IsConnected)
                    deviceCount++;
            }

            DeviceCount = deviceCount;
            
            foreach (var action in InputAction.InputActions)
                for (var deviceId = -1; deviceId < DeviceCount - 1; deviceId++)
                    if (HasActionPressChanged(action, out var pressed, deviceId))
                        action.RunCallbacks(pressed, deviceId);
        }


        public static bool IsActionPressed(InputAction action, int deviceId = -1)
        {
            if (deviceId == -1)
                return CheckActionForStates(action, _currentKeyboardState, _currentMouseState);

            var gamepadIndex = _gamepadIndexes[deviceId];

            foreach (var binding in action.InputBindings)
                if (gamepadIndex.CurrentState.IsButtonDown(binding.GamepadButton))
                    return true;

            return false;
        }

        public static bool WasActionPressed(InputAction action, int deviceId = -1)
        {
            if (deviceId == -1)
                return CheckActionForStates(action, _previousKeyboardState, _previousMouseState);

            var gamepadIndex = _gamepadIndexes[deviceId];

            foreach (var binding in action.InputBindings)
                if (gamepadIndex.PreviousState.IsButtonDown(binding.GamepadButton))
                    return true;

            return false;
        }

        public static bool HasActionPressChanged(InputAction action, out bool isActionPressed, int deviceId = -1)
        {
            isActionPressed = IsActionPressed(action, deviceId);

            return isActionPressed != WasActionPressed(action, deviceId);
        }

        public static bool HasActionPressChangedTo(InputAction action, bool checkIsCurrentlyPressed = true, int deviceId = -1)
            => HasActionPressChanged(action, out var isActionPressed, deviceId) && isActionPressed == checkIsCurrentlyPressed;
        

        public static void OnActionPressChanged(InputAction action, Action<bool, int> callback)
            => action.AddCallback(callback);
        

        private static bool IsMouseButtonPressed(MouseState mouseState, MouseBinding mouseButton)
        {
            switch (mouseButton)
            {
                case MouseBinding.Mouse1:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseBinding.Mouse2:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseBinding.Mouse3:
                    return mouseState.MiddleButton == ButtonState.Pressed;
            }

            return false;
        }

        private static bool CheckActionForStates(InputAction action, KeyboardState keyboardState, MouseState mouseState)
        {
            foreach (var binding in action.InputBindings)
                if (keyboardState.IsKeyDown(binding.KeyCode) || IsMouseButtonPressed(mouseState, binding.MouseButton))
                    return true;
            
            return false;
        }
    }
}