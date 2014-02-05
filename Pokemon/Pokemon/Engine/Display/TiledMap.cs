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
    /// Represents an orthagonal map loaded from the Tiled map editor. This class is inherited by Map.
    /// </summary>
    public class TiledMap
    {
        #region Private Fields
        private List<Layer> _Layers = new List<Layer>(10);
        private List<TileSet> _TileSets = new List<TileSet>(50);
        private int _Width, _Height;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledMap"/> class.
        /// </summary>
        /// <param name="FilePath">The file path.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        public TiledMap(string FilePath, GraphicsDevice graphicsDevice)
        {
            Properties = new TiledPropertyCollection();

            Stream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (XmlReader reader = XmlReader.Create(fileStream, new XmlReaderSettings { CloseInput = true }))
            {
                while (reader.Read())
                {
                    // reader.Read() returns each starting element with any whitespaces and <xml? so we want to
                    // make sure we just get the starting elements. it reads in order, without depth sequence.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "map":
                                Width = int.Parse(reader["width"]);
                                Height = int.Parse(reader["height"]);
                                break;
                            case "properties":
                                LoadProperties(reader);
                                break;
                            case "tileset":
                                LoadTileset(reader);
                                break;
                            case "layer":
                                LoadLayer(reader);
                                break;
                            case "objectgroup":
                                LoadObjectGroupLayer(reader);
                                break;
                        }
                    }
                }
            }
            // TODO: After parsing the map file's XML, perhaps we should load the Texture2D of the tilesets?
            for (int i = 0; i < TileSets.Count; i++)
            {
                Stream textureFileStream = new FileStream(TileSets[i].SourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                TileSets[i].Texture = Texture2D.FromStream(graphicsDevice, textureFileStream, TileSets[i].Width, TileSets[i].Height, false);
            }
            // Now set the tile's texture
            // data is left-to-right, top-to-bottom
            foreach (Layer layer in Layers)
            {
                for (int x = 0; x < layer.Tiles.Width; x++)
                {
                    for (int y = 0; y < layer.Tiles.Height; y++)
                    {
                        if (layer.Tiles[x, y].Id == 0) continue;
                        layer.Tiles[x, y].ParentTileSet = GetTileSetFromGID(layer.Tiles[x, y].Id);
                        layer.Tiles[x, y].X = (layer.Tiles[x, y].Id - layer.Tiles[x, y].ParentTileSet.FirstGID) % (layer.Tiles[x, y].ParentTileSet.Width / 32) * 32;
                        layer.Tiles[x, y].Y = (layer.Tiles[x, y].Id - layer.Tiles[x, y].ParentTileSet.FirstGID) / (layer.Tiles[x, y].ParentTileSet.Width / 32) * 32;
                    }
                }
            }
        }

        /// <summary>
        /// Defines the dimensions of a map tile: 32 pixels wide by 32 pixels tall.
        /// </summary>
        public const byte TileDimensions = 32;

        /// <summary>
        /// Gets or sets the map height in tile units.
        /// </summary>
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        /// <summary>
        /// Gets or sets the map width in tile units.
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        /// <summary>
        /// Gets a list of the map's properties.
        /// </summary>
        public TiledPropertyCollection Properties { get; private set; }

        /// <summary>
        /// Gets or sets the map layers.
        /// </summary>
        public List<Layer> Layers
        {
            get
            {
                return _Layers;
            }
            set
            {
                _Layers = value;
            }
        }

        /// <summary>
        /// Gets or sets the tilesets.
        /// </summary>
        public List<TileSet> TileSets
        {
            get
            {
                return _TileSets;
            }
            set
            {
                _TileSets = value;
            }
        }       

        /// <summary>
        /// When the XmlReader gets to the <properties> node, LoadProperties() reads all the underlying <property>
        /// nodes and 
        /// </summary>
        private void LoadProperties(XmlReader reader)
        {
            // We are now at the <properties> node.
            // There are no attributes to read; perhaps in a future version of Tiled map editor there will be.
            XmlReader mapPropertiesReader = reader.ReadSubtree();
            // We are now at the <property> node.
            while (mapPropertiesReader.ReadToFollowing("property"))
            {
                Properties.Add(new TiledProperty(reader["name"], reader["value"]));
            }
        }

        /// <summary>
        /// When the XmlReader gets to the <tileset> node, LoadTileset() takes flow control and reads the tileset
        /// properties and underlying node image for the tileset's source path. LoadTileset() will be called for
        /// each tileset node.
        /// </summary>
        private void LoadTileset(XmlReader reader)
        {
            int tilesetFirstGID = int.Parse(reader["firstgid"]);
            XmlReader tilesetPropertiesReader = reader.ReadSubtree();
            tilesetPropertiesReader.ReadToFollowing("image");
            string tilesetSourcePath = tilesetPropertiesReader["source"];
            int tilesetWidth = int.Parse(reader["width"]);
            int tilesetHeight = int.Parse(reader["height"]);

            _TileSets.Add(new TileSet(tilesetFirstGID, tilesetSourcePath, tilesetWidth, tilesetHeight));
            reader.Skip(); // We already read the child 'image' node. No need to re-read that when we pass flow control
        }

        private void LoadLayer(XmlReader reader)
        {
            // We are now at the <layer> node.
            // Read the attribute(s) 'name'.
            // TODO: Should we make the name a #? Like the layer name is '1', and that indicates the draw order.
            string layerName = reader["name"];
            XmlReader layerDataReader = reader.ReadSubtree();
            layerDataReader.ReadToFollowing("data");
            // We are now at the <data> node.
            #region TODO: Handle XML, Base64, GZIP, and ZLib Compression
            /*
            var encoding = layerDataReader.GetAttribute("encoding");
            var compressor = reader.GetAttribute("compression");
            switch (encoding)
            {
                case "base64":
                    {
                        int dataSize = (TileDimensions * TileDimensions * 4) + 1024;
                        var buffer = new byte[dataSize];
                        reader.ReadElementContentAsBase64(buffer, 0, dataSize);

                        Stream stream = new MemoryStream(buffer, false);
                        if (compressor == "gzip")
                            stream = new GZipStream(stream, CompressionMode.Decompress, false);

                        using (stream)
                        using (var br = new BinaryReader(stream))
                        {
                            for (int i = 0; i < tileArray.Length; i++)
                                tileArray[i] = br.ReadInt32();
                        }

                        continue;
                    };

                default:
                    throw new Exception("Your map layer, called '" + name + "' is encoded with an unrecognized compression algorithm. The accepted values are XML, Base64, and GZIP. Don't use ZLib!");
            }*/
            #endregion
            XmlReader layerDataTileReader = layerDataReader.ReadSubtree();
            // We are now at the <tile> node. There are a lot of these...

            TileGrid layerTiles = new TileGrid(Width, Height, true);
            Layer layer = new Layer();
            layer.Name = layerName;

            int lineReadIndex = 0; // the # of lines read by the parser 

            while (layerDataTileReader.ReadToFollowing("tile"))
            {
                int tilegid = int.Parse(layerDataTileReader["gid"]);

                layerTiles[lineReadIndex % Width, lineReadIndex / Width].Id = tilegid;
                layerTiles[lineReadIndex % Width, lineReadIndex / Width].ParentLayer = layer;

                lineReadIndex++;
            }

            layer.Tiles = layerTiles;
            layer.ParentMap = (Map)this;

            Layers.Add(layer);
        }

        private void LoadObjectGroupLayer(XmlReader reader)
        {
            // We are now at the <objectgroup> node.
            // No need to read the attribute. For now, all objectgroup layers are interaction layers. Only 1 is needed.
            XmlReader objectReader = reader.ReadSubtree();

            while (objectReader.ReadToFollowing("object"))
            {
                LoadObjectTile(reader);
            }
        }

        /// <summary>
        /// Loads the object or tile within the 'objectgroup' layer.
        /// </summary>
        private void LoadObjectTile(XmlReader reader)
        {
            reader.ReadToFollowing("object");
            /* TODO: Implement Object layer support. */
        }
        
        /// <summary>
        /// Gets a layer by name.
        /// </summary>
        /// <param name="name">The name of the layer to retrieve.</param>
        /// <returns>The layer with the given name.</returns>
        public Layer GetLayer(string name)
        {
            foreach (Layer l in Layers)
            {
                if (l.Name == name)
                {
                    return l;
                }
            }
            return null;
        }

        public TileSet GetTileSetFromGID(int globalTileID)
        {
            if (globalTileID == 0) return null;
            // Just our luck that the FirstGID of the tilesets are arranged in increasing order
            for (int i = 0; i < TileSets.Count; i++)
            {
                if (TileSets[i].FirstGID >= globalTileID)
                {
                    return TileSets[i - 1];
                }
            }
            return null;
        }
    }
}
