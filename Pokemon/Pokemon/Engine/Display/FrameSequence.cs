using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pokemon.Engine.Display
{
    /// <summary>
    /// As opposed to a BeginningFrameIndex and EndingFrameIndex where each frame is played for a short duration to
    /// give the illusion of animation, a FrameSequence is simply an array of integers which defines, in order, which
    /// frames of the sprite sheet are to be played. This works when we need multi-directional animations
    /// (character movement) and when the sprite sheets animations are not in order.
    /// </summary>
    /// <remarks>This only supports one-dimensional sprite sheets.</remarks>
    public class FrameSequence
    {
        private SortedDictionary<int, Rectangle> frames;
        private int currentFrameIndex;

        public FrameSequence()
        {
            currentFrameIndex = 0;
            frames = new SortedDictionary<int, Rectangle>();
        }

        /// <summary>
        /// Gets or sets the frame indices of the sprite sheet. The Rectangle values are loaded by a separate
        /// animation data file.
        /// </summary>
        public SortedDictionary<int, Rectangle> Frames
        {
            get { return frames; }
            set { frames = value; }
        }

        /// <summary>
        /// Gets the number of frames in this sequence.
        /// </summary>
        public int NumberOfFrames
        {
            get
            {
                return Frames.Count;
            }
        }

        /// <summary>
        /// Gets or sets the index, in the total number of frames of the animation (as if you were to open up the image file in a program and count the number of frames), of the current frame that this <c>FrameSequence</c> object is currently rendering.
        /// </summary>
        /// <value>The index of the beginning frame.</value>
        public int CurrentFrameIndex
        {
            get { return currentFrameIndex; }
            set { currentFrameIndex = (int)MathHelper.Clamp(value, 0, NumberOfFrames); }
        }
    }
}
