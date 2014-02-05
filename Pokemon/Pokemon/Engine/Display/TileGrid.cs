namespace Pokemon.Engine.Display
{
    /// <summary>
    /// A 2D grid of Tile objects. This class is used in the Tile class.
    /// </summary>
    public class TileGrid
    {
        private Tile[,] rawTiles;

        /// <summary>
        /// Gets or sets a Tile at a given index.
        /// </summary>
        /// <param name="x">The X index.</param>
        /// <param name="y">The Y index.</param>
        /// <returns></returns>
        public Tile this[int x, int y]
        {
            get { return rawTiles[x, y]; }
            set { rawTiles[x, y] = value; }
        }

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Creates a new TileGrid.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        public TileGrid(int width, int height)
        {
            rawTiles = new Tile[width, height];
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new TileGrid.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="Initialize">Initialize each value in the array.</param>
        public TileGrid(int width, int height, bool Initialize)
        {
            rawTiles = new Tile[width, height];
            Width = width;
            Height = height;

            if (Initialize)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        rawTiles[x, y] = new Tile();
                    }
                }
            }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        public TileGrid Clone()
        {
            TileGrid tileGrid = new TileGrid(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tileGrid[x, y] = this[x, y].Clone();
                }
            }
            return tileGrid;
        }
    }
}
