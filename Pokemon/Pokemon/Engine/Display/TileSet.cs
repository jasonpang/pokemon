using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;
using System.IO.Compression;
using System.Globalization;

namespace Pokemon.Engine.Display
{
    /// <summary>
    /// Represents a map tileset.
    /// </summary>
    public class TileSet
    {
        private Texture2D _Texture;
        private string _SourcePath;        
        private int _FirstGID = 0;
        private int _Width, _Height;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileSet"/> class.
        /// </summary>
        /// <param name="FirstGID">The first GID of the tileset.</param>
        /// <param name="SourcePath">The source path to the tileset's texture.</param>
        public TileSet(int FirstGID, string SourcePath, int Width, int Height)
        {
            _SourcePath = SourcePath;
            _FirstGID = FirstGID;
            _Width = Width;
            _Height = Height;
        }

        /// <summary>
        /// Gets or sets the PNG image of tiles in the form of a Texture2D object.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return _Texture;
            }
            set
            {
                _Texture = value;
            }
        }

        /// <summary>
        /// Gets or sets the source path of the tileset texture.
        /// </summary>
        public string SourcePath
        {
            get
            { 
                return _SourcePath;
            }
            set
            { 
                _SourcePath = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the tileset.
        /// </summary>
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the tileset.
        /// </summary>
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the first global ID (GID) of this tileset. This GID is used by the tiles of a map 
        /// to describe which tileset to use and which region of the tileset to use as the source rectangle
        /// for drawing.
        /// </summary>
        /// <value>The first GID.</value>
        public int FirstGID
        {
            get
            { 
                return _FirstGID;
            }
            set 
            { 
                _FirstGID = value; 
            }
        }

        /// <summary>
        /// Converts a tile's GID to the specific X coordinate of the tile in the tileset.
        /// </summary>
        /// <param name="globalTileId">The tile's GID.</param>
        /// <returns></returns>
        public int ConvertGIDToX(int globalTileId)
        {
            /* Logic:
            int localTileId = globalTileId - _FirstGID;
            return localTileId % Width;
             */
            return (globalTileId - FirstGID) % (Width / 32);
        }

        /// <summary>
        /// Converts a tile's GID to the specific Y coordinate of the tile in the tileset.
        /// </summary>
        /// <param name="globalTileId">The tile's GID.</param>
        public int ConvertGIDToY(int globalTileId)
        {
            /* Logic:
            int localTileId = globalTileId - _FirstGID;
            return localTileId / Width;
             */
            return (globalTileId - FirstGID) / (Width / 32);
        }

        public Vector2 ConvertGIDToXY(int globalTileId)
        {
            return new Vector2((globalTileId - FirstGID) % (Width / 32), (globalTileId - FirstGID) / (Width / 32));
        }
    }
}
