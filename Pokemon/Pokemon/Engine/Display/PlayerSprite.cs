using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;

namespace Pokemon.Engine.Display
{
    public class PlayerSprite : AnimatedSprite
    {
        public Direction PlayerDirection = Direction.Idle;
        public bool IsMoving = false, IsBeginningToMove = false;

        public PlayerSprite()
            : base()
        {
            CurrentFrameKey = "Down";
        }

        /// <summary>
        /// Called when the game component should be updated.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game's timing state.</param>
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Visible)
            {
                switch (PlayerDirection)
                {
                    case Direction.Down:
                        if (CurrentFrameKey != "Down")
                        {
                            IsBeginningToMove = true;
                            CurrentFrameKey = "Down";
                            CurrentFrameSequenceIndex = 0;
                        }
                        break;
                    case Direction.Up:
                        if (CurrentFrameKey != "Up")
                        {
                            IsBeginningToMove = true;
                            CurrentFrameKey = "Up";
                            CurrentFrameSequenceIndex = 0;
                        }
                        break;
                    case Direction.Left:
                        if (CurrentFrameKey != "Left")
                        {
                            IsBeginningToMove = true;
                            CurrentFrameKey = "Left";
                            CurrentFrameSequenceIndex = 0;
                        }
                        break;
                    case Direction.Right:
                        if (CurrentFrameKey != "Right")
                        {
                            IsBeginningToMove = true;
                            CurrentFrameKey = "Right";
                            CurrentFrameSequenceIndex = 0;
                        }
                        break;
                }
            }
            if (IsMoving)
            {

                // Update this so we can...
                MillisecondsElapsedSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;

                // Check if we need to advance the frame, see if the frame has reached its duration limit
                if (MillisecondsElapsedSinceLastFrame >= FrameDuration)
                {
                    // Time for the animation to advance one frame!                    
                    // Genius code that faithfully increments the frame index, but also resets it back to 0 when it reaches the max number of frames.
                    CurrentFrameSequenceIndex = (CurrentFrameSequenceIndex + 1) % (NumberOfFrames);

                    MillisecondsElapsedSinceLastFrame = 0;
                }
            }
            else
            {
                // Player is not moving, but make sure the player ends the animation frame on the correct frame.
                // If on an odd frame, we can't stop the sprite in mid-air with a movement frame!
                if (CurrentFrameSequenceIndex % 2 != 0 && !Input.Keyboard.AreMovementKeysDown())
                {
                    // Increment a Left_1 to Left_2 (movement -> still); Left_3 to Left_0.
                    CurrentFrameSequenceIndex = (CurrentFrameSequenceIndex + 1) % (NumberOfFrames);
                }
            }
        }


        /// <summary>
        /// Draws the IDrawable. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game's timing state.</param>
        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Visible)
            {
                // TODO: Check if the sprite is within the camera/viewport

                /* First, let's get the correct FrameSequence out of all the different FrameSequence objects. Remember there is a separate FrameSequence for multi-directional player movement animation. */
                FrameSequence frameSequence = FrameSequences[CurrentFrameKey];
                /* Now that we have the correct FrameSequence, let's get the correct frame. Which frame of the FrameSequence do we get? We get it based on the CurrentFrameSequenceIndex, which increments during Update() if enough time has elapsed (also defined as FrameDuration). */
                Rectangle currentFrame = frameSequence.Frames[CurrentFrameSequenceIndex];

                /* If frame is odd (i.e Left_1 or Left_3), it is a motion frame. This is important as motion frames have a special 'bounce' property where the DestinationRectangle is one pixel lower to make it look like the character is moving. */
                // Add compensation pixels (-2, -5)
                if (CurrentFrameSequenceIndex % 2 != 0)
                {
                    spriteBatch.Draw(
                           SpriteTexture,
                           new Rectangle(Math.Abs(Game.StarterMap.camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.Screen).X) + 2, Math.Abs(Game.StarterMap.camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.Screen).Y) - 2 - 16, currentFrame.Width, currentFrame.Height),
                               currentFrame,
                               Color.White);
                }
                else
                {
                    spriteBatch.Draw(
                           SpriteTexture,
                           new Rectangle(Math.Abs(Game.StarterMap.camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.Screen).X) + 2, Math.Abs(Game.StarterMap.camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.Screen).Y) - 16, currentFrame.Width, currentFrame.Height),
                               currentFrame,
                               Color.White);
                }
            }
        } // Character is moving too fast 4-23-11-14
        /* The origin for Player Coordinate system isn't a spawn point on a map. It's the actual window border. No
         * wonder...it actually does move 32 each time (FROM THE WINDOW BORDER). */

        /// <summary>
        /// Gets the screen center coordinates for an object of specified dimensions.
        /// </summary>
        /// <returns></returns>
        public Point GetScreenCenterCoordinates()
        {
            int x = (Game.graphics.PreferredBackBufferWidth / 2) - (CurrentFrame.Width / 2) - 16;
            int y = (Game.graphics.PreferredBackBufferHeight / 2) - (CurrentFrame.Height / 2) + 5;

            return new Point(x, y);
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Idle
    }
}
