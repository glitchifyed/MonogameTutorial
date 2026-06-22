using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.Input.Enums;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
    public record InputBinding(Keys KeyCode = Keys.None, Buttons GamepadButton = Buttons.None, MouseBinding MouseButton = MouseBinding.None);
}