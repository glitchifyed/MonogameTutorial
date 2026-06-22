using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
    public class GamepadIndex
    {
        public PlayerIndex PlayerIndex { get; }

        public GamePadState CurrentState { get; private set; } = new();
        public GamePadState PreviousState { get; private set; } = new();

        public bool IsConnected { get; private set; }


        public GamepadIndex(PlayerIndex playerIndex)
            => PlayerIndex = playerIndex;


        public void Update()
        {
            PreviousState = CurrentState;
            CurrentState = GamePad.GetState(PlayerIndex);

            IsConnected = CurrentState.IsConnected;
        }
    }
}