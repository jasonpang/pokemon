using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon.Engine.Display
{
    /// <summary>
    /// Represents one tile of a map.
    /// </summary>
    public class Tile
    {
        #region Constants
        /// <summary>
        /// Defines the dimensions of a map tile: 32 pixels wide by 32 pixels tall.
        /// </summary>
        public const byte TileDimensions = 32;
        #endregion

        #region Private Fields
        private int x, y;
        private int gid;
        private TileSet _ParentTileSet;

        private Layer parentLayer;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        public Tile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="X">The X tile-coordinate of this tile.</param>
        /// <param name="Y">The Y tile-coordinate of this tile.</param>
        /// <param name="ParentLayer">The parent layer this tile belongs to.</param>
        public Tile(int X, int Y, Layer ParentLayer)
        {
            this.x = X;
            this.y = Y;
            this.parentLayer = ParentLayer;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the X location of this tile in 32-piece (game) coordinates; the number of units to the right of the top left origin.
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets the Y location of this tile in 32-piece (game) coordinates; the number of units down from the top left origin.
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Gets the global tile ID (GID) of this tile. This is used by the map 
        /// </summary>
        public int Id
        {
            get { return gid; }
            set { gid = value; }
        }

        /// <summary>
        /// Gets or sets the TileSet reference which this tile belongs to. Called by TiledMap at the end of
        /// LoadMap() using the GID of each tile.
        /// </summary>
        /// <value>The parent texture.</value>
        public TileSet ParentTileSet
        {
            get { return _ParentTileSet; }
            set { _ParentTileSet = value; }
        }

        /// <summary>
        /// Gets the parent <c>Layer</c> class this tile belongs to on the map.
        /// </summary>
        public Layer ParentLayer
        {
            get { return parentLayer; }
            set { parentLayer = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Tile Clone()
        {
            return new Tile(X, Y, ParentLayer);
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
