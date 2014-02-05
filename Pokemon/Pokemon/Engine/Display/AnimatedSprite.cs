using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;

namespace Pokemon.Engine.Display
{

    /// <summary>
    /// Represents a sprite capable of animation, such as flowers, player characters, doors, water tiles...etc.
    /// One sprite sheet (texture), many possible frame sequences (animation data file).
    /// </summary>
    public class AnimatedSprite : ISprite
    {
        #region Protected Fields
        /// <summary>
        /// See <see cref="SpriteTexture"/>.
        /// </summary>
        protected Texture2D spriteTexture;
        /// <summary>
        /// See <see cref="FrameSequences"/>.
        /// </summary>
        protected Dictionary<string, FrameSequence> frameSequences;
        /// <summary>
        /// See <see cref="CurrentFrameKey"/>.
        /// </summary>
        protected string currentFrameKey;        
        /// <summary>
        /// See <see cref="CurrentFrameSequenceIndex"/>.
        /// </summary>
        protected int currentFrameSequenceIndex;
        /// <summary>
        /// See <see cref="MillisecondsElapsedSinceLastFrame"/>.
        /// </summary>
        protected double millisecondsElapsedSinceLastFrame;
        /// <summary>
        /// See <see cref="FrameDuration"/>.
        /// </summary>
        protected double frameDuration;
        /// <summary>
        /// See <see cref="Visible"/>.
        /// </summary>
        protected bool doDrawSprite;
        /// <summary>
        /// See <see cref="Enabled"/>.
        /// </summary>
        protected bool doUpdateSprite;
        /// <summary>
        /// The SpriteBatch object used to draw this AnimatedSprite object. It is passed into this class via Load() and is used for the Draw() method.
        /// </summary>
        protected SpriteBatch spriteBatch;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the sprite's texture. This is probably a PNG image file with alpha support which displays the sprites in an array of frozen positions. To set the sprite's texture, call the Load() method.
        /// </summary>
        /// <value>The sprite texture as a <c>Texture2D</c> object.</value>
        public Texture2D SpriteTexture
        {
            get { return spriteTexture; }
            private set { spriteTexture = value; }
        }

        /// <summary>
        /// Gets or sets the name of the FrameSequence that is playing. Remember FrameSequences is a Dictionary based
        /// on a string (FrameKey) and the FrameSequence. So there is a FrameSequence for player's left movement, a
        /// separate FrameSequence for player's right movement. The frame key could be "Left" or "Right".
        /// </summary>
        public string CurrentFrameKey
        {
            get { return currentFrameKey; }
            set { currentFrameKey = value; }
        }

        /// <summary>
        /// Gets or sets the frame index of the current FrameSequence. In other words, which frame of the FrameSequence
        /// is playing?
        /// </summary>
        public int CurrentFrameSequenceIndex
        {
            get { return currentFrameSequenceIndex; }
            set 
            { 
                currentFrameSequenceIndex = (int)MathHelper.Clamp(value, 0, NumberOfFrames); 
            }
        }

        /// <summary>
        /// Gets the number of frames in the current FrameSequence.
        /// </summary>
        public int NumberOfFrames
        {
            get 
            {
                return FrameSequences[CurrentFrameKey].Frames.Count;
            }
        }

        /// <summary>
        /// Gets or sets the FrameSequences of the animation.
        /// </summary>
        public Dictionary<string, FrameSequence> FrameSequences
        {
            get
            {
                return frameSequences;
            }
            set
            {
                frameSequences = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds elapsed since the last frame (i.e. since the moment this current frame was drawn).
        /// </summary>
        /// <value>The number of milliseconds elapsed since last frame.</value>
        public double MillisecondsElapsedSinceLastFrame
        {
            get { return millisecondsElapsedSinceLastFrame; }
            set { millisecondsElapsedSinceLastFrame = value; }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds each frame lasts before advancing to the next frame.
        /// </summary>
        /// <value>
        /// The number of milliseconds each frame lasts before advancing to the next frame.
        /// </value>
        public double FrameDuration
        {
            get { return frameDuration; }
            set { frameDuration = value; }
        }

        /// <summary>
        /// Gets the number of milliseconds remaining before advancing to the next frame.
        /// </summary>
        /// <value>
        /// The number of milliseconds remaining before advancing to the next frame.
        /// </value>
        public double MillisecondsRemainingUntilNextFrame
        {
            get { return frameDuration - millisecondsElapsedSinceLastFrame; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IDrawable.Draw should be called in Game.Draw for this game component.
        /// </summary>
        /// <value>
        /// If true, the sprite will be drawn during Game.Draw(); if false, the sprite will not be drawn.
        /// </value>
        public bool Visible
        {
            get { return doDrawSprite; }
            set { doDrawSprite = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the game component's Update method should be called in Game.Update.
        /// </summary>
        /// <value>
        /// If true, the sprite will be updated during Game.Update(); otherwise, the sprite will not be updated.
        /// </value>
        public bool Enabled
        {
            get { return doUpdateSprite; }
            set { doUpdateSprite = value; }
        }

        /// <summary>
        /// Gets the current frame rectangle within the current FrameSequence.
        /// </summary>
        public Rectangle CurrentFrame
        {
            get { return FrameSequences[CurrentFrameKey].Frames[CurrentFrameSequenceIndex]; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedSprite"/> class.
        /// </summary>
        public AnimatedSprite()
        {
            frameSequences = new Dictionary<string, FrameSequence>(10);
        }
        #endregion

        /// <summary>
        /// Loads the AnimatedSprite with a given Texture2D and initializes the frame properties.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw the AnimatedSprite with. This is probably passed by reference from the Game class somewhere up the hierachy.</param>
        public void Load(Texture2D texture, SpriteBatch spriteBatch, string AnimationDataFilePath)
        {
            this.spriteBatch = spriteBatch;
            SpriteTexture = texture;
            AnimationDataFileParser.ParseFile(AnimationDataFilePath, FrameSequences);
        }

        #region Public Methods
        /// <summary>
        /// Called when the game component should be updated.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game's timing state.</param>
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // If we should even bother to update this sprite (why wouldn't it update? debugging purposes maybe?)
            if (doUpdateSprite)
            {
                // Update this so we can...
                MillisecondsElapsedSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;

                // Check if we need to advance the frame, see if the frame has reached its duration limit
                if (MillisecondsElapsedSinceLastFrame >= FrameDuration)
                {
                    // Time for the animation to advance one frame!                    
                    // Genius code that faithfully increments the frame index, but also resets it back to 0 when it reaches the max number of frames.
                    CurrentFrameSequenceIndex =  (CurrentFrameSequenceIndex + 1) % (NumberOfFrames);

                    MillisecondsElapsedSinceLastFrame = 0;
                }
            }
        }

        /// <summary>
        /// Draws the IDrawable. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game's timing state.</param>
        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (doDrawSprite)
            {
                // TODO: Check if the sprite is within the camera/viewport
                
                    /* First, let's get the correct FrameSequence out of all the different FrameSequence objects. Remember there is a separate FrameSequence for multi-directional player movement animation. */
                    FrameSequence frameSequence = FrameSequences[CurrentFrameKey];
                    /* Now that we have the correct FrameSequence, let's get the correct frame. Which frame of the FrameSequence do we get? We get it based on the CurrentFrameSequenceIndex, which increments during Update() if enough time has elapsed (also defined as FrameDuration). */
                    Rectangle currentFrame = frameSequence.Frames[CurrentFrameSequenceIndex];

                spriteBatch.Draw(spriteTexture, new Rectangle(1 + (32 * 7), 42 - 32 + (32 * 4), currentFrame.Width, currentFrame.Height), currentFrame, Color.White); 
            }
        }


        #endregion
        
        #region Private Methods
#endregion

        #region Unused Interface Implementations
        /// <summary>
        /// Raised when the UpdateOrder property changes.
        /// </summary>
        /// <param name=""/>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Raised when the Enabled property changes.
        /// </summary>
        /// <param name=""/>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Raised when the Visible property changes.
        /// </summary>
        /// <param name=""/>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Raised when the DrawOrder property changes.
        /// </summary>
        /// <param name=""/>
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>
        /// Indicates when the game component should be updated relative to other game components. Lower values are updated first.
        /// </summary>
        public int UpdateOrder
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// The order in which to draw this object relative to other objects. Objects with a lower value are drawn first.
        /// </summary>
        public int DrawOrder
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}
