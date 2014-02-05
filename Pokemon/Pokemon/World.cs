using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pokemon
{
    /// <summary>
    /// Encompasses the 
    /// </summary>
    public class World : IDrawable, IUpdateable
    {
        /// <summary>
        /// Indicates whether the game component's Update method should be called in Game.Update.
        /// </summary>
        /// <value></value>
        public bool Enabled
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Raised when the Enabled property changes.
        /// </summary>
        /// <param name=""/>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Called when the game component should be updated.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game's timing state.</param>
        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates when the game component should be updated relative to other game components. Lower values are updated first.
        /// </summary>
        /// <value></value>
        public int UpdateOrder
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Raised when the UpdateOrder property changes.
        /// </summary>
        /// <param name=""/>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Draws the IDrawable. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game's timing state.</param>
        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The order in which to draw this object relative to other objects. Objects with a lower value are drawn first.
        /// </summary>
        /// <value></value>
        public int DrawOrder
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Raised when the DrawOrder property changes.
        /// </summary>
        /// <param name=""/>
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>
        /// Indicates whether IDrawable.Draw should be called in Game.Draw for this game component.
        /// </summary>
        /// <value></value>
        public bool Visible
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Raised when the Visible property changes.
        /// </summary>
        /// <param name=""/>
        public event EventHandler<EventArgs> VisibleChanged;
    }
}
