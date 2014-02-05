using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pokemon.Input
{
    public static class Keyboard
    {
        /// <summary>
        /// Returns true if any movement key is pressed; false otherwise.
        /// </summary>
        public static bool AreMovementKeysDown()
        {
            KeyboardState currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.W)
                || currentKeyboardState.IsKeyDown(Keys.A)
                || currentKeyboardState.IsKeyDown(Keys.S)
                || currentKeyboardState.IsKeyDown(Keys.D))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the input.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public static void ProcessInput(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            // TODO: Set keyboard mappings
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                if (Game.Player.IsMoving) return;
                Game.Player.IsBeginningToMove = true;
                Game.Player.PlayerDirection = Engine.Display.Direction.Left;
                //Game.StarterMap.CameraOffset.X--;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                if (Game.Player.IsMoving) return;
                Game.Player.IsBeginningToMove = true;
                Game.Player.PlayerDirection = Engine.Display.Direction.Right;
                //Game.player.Position = new Vector2(Game.player.Position.X + 1, Game.player.Position.Y);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                if (Game.Player.IsMoving) return;
                Game.Player.IsBeginningToMove = true;
                Game.Player.PlayerDirection = Engine.Display.Direction.Up;
                //Game.player.Position = new Vector2(Game.player.Position.X, Game.player.Position.Y - 1);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                if (Game.Player.IsMoving) return;
                Game.Player.IsBeginningToMove = true;
                Game.Player.PlayerDirection = Engine.Display.Direction.Down;
                //Game.player.Position = new Vector2(Game.player.Position.X, Game.player.Position.Y + 1);
            }
        }

    }
}

/* New Movement Logic

Problem with Old Movement Logic: When character moved, screen would scroll one tile at a time. There was no smooth pixel
 * by pixel transition.
 * Character would also seem to jump all over the place.
 * 
 * Old Logic: If Keys.W, Game.Player.Position.Y--;
 * New Logic: If Keys.W  Game.Player.MoveDirection = Direction.Up; Game.Player.IsMoving = true; [move until 32 pixels]
 *            but remember, the PLAYER isn't moving, the tiles are.

*/