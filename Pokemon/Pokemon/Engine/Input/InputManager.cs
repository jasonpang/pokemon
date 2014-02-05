using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pokemon.Input
{
    /// <summary>
    /// 
    /// </summary>
    public static class InputManager
    {
        public static void Update(GameTime gameTime)
        {
            Keyboard.ProcessInput(gameTime);
            Cursor.ProcessInput(gameTime);
        }
    }
}
