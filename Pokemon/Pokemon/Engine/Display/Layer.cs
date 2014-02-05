using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon.Engine.Display
{
    public class Layer
    {
        private Map parentMap;
        private string _Name;


        /// <summary>
        /// Gets the layout of tiles on the layer.
        /// </summary>
        public TileGrid Tiles { get; set; }

        /// <summary>
        /// Gets the parent <c>Map</c> class this tile belongs to on the map.
        /// </summary>
        public Map ParentMap
        {
            get { return parentMap; }
            set { parentMap = value; }
        }

        /// <summary>
        /// Gets or sets the name of the layer.
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
        public Layer()
        {
        }

        /// <summary>
        /// Sets the tile grid for this layer.
        /// </summary>
        /// <param name="tileGrid">The tile grid this Layer manages.</param>
        /// <param name="parentMap">The reference to the parent map of this Layer.</param>
        public void SetTileGrid(TileGrid tileGrid)
        {
            Tiles = new TileGrid(tileGrid.Width, tileGrid.Height);

            // data is left-to-right, top-to-bottom
            for (int x = 0; x < tileGrid.Width; x++)
            {
                for (int y = 0; y < tileGrid.Height; y++)
                {
                    /* It is extremely important to clone instead of simply passing in tileGrid[x, y], as Clone()                              ensures the stored entity is an separate object, not a reference that dissapears when the                               parameter tileGrid is destroyed. */
                    Tiles[x, y] = tileGrid[x, y].Clone();
                }
            }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Layer Clone()
        {
            Layer newLayer = new Layer();
            newLayer.SetTileGrid(Tiles);
            newLayer.ParentMap = parentMap;
            return newLayer;
        }
    }
}
