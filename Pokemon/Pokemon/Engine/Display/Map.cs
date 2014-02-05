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
    /// Represents a map loaded from Tiled map editor with helper methods and properties for the game.
    /// </summary>
    public class Map : TiledMap
    {
        public Camera camera;

        public Map(string FilePath, GraphicsDevice graphicsDevice)
            : base(FilePath, graphicsDevice)
        {
            TiledProperty prop = Properties["SpawnPoint"];
            Point spawnPoint = new Point(int.Parse(prop.RawValue.Split(new char[] { ',' })[0]), int.Parse(prop.RawValue.Split(new char[] { ',' })[1]));
            camera = new Camera(spawnPoint, Units.Tile);
        }


        public void Update()
        {
            if (Game.Player.IsBeginningToMove)
            {
                Game.Player.IsBeginningToMove = false;

                if (CanPlayerMove(Game.Player.PlayerDirection))
                {
                    Game.Player.IsMoving = true;
                }
                else
                {
                    return;
                }
            }
            if (Game.Player.IsMoving) //|| Math.Abs(CameraOffset.Y) % 32 != 0)
            {
                // Player wants to move
                // Collision detection!
                camera.Pan(Game.Player.PlayerDirection, 2, Units.Pixel);
                /* It's like parallax scrolling - it's scrolling doubly. The PlayerCoordOffset does faithfully
                 * increment 32, but so does the WorldCoordOffset. */
            }
            if ((Math.Abs(camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.World).X) % 32 == 0) && (Math.Abs(camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.World).Y) % 32 == 0))
            { // ^ Until the camera has panned 32 units in the given direction, it will keep panning, 2 units at a time
                Game.Player.IsMoving = false;
            }
        }

        public bool CanPlayerMove(Direction direction)
        {
            int x = Math.Abs((int)camera.WorldCoordTileOffset.X);
            int y = Math.Abs(((int)camera.WorldCoordTileOffset.Y));

            // Collision layer
            switch (direction)
            {
                case Direction.Down:
                    return (y <= Height - 1) &&(Layers[1].Tiles[x, y + 1].Id == 0);
                    break;
                case Direction.Left:
                    return (x > 0) && (Layers[1].Tiles[x - 1, y].Id == 0);
                    break;
                case Direction.Right:
                    return (x <= Width - 1) && (Layers[1].Tiles[x + 1, y].Id == 0);
                    break;
                case Direction.Up:
                    return (y > 0) && (Layers[1].Tiles[x, y - 1].Id == 0);
                    break;
            }
            return false;
        }

        /// <summary>
        /// Debugging function: returns, in tile units, player's position relative to world (world coordinates).
        /// </summary>
        /// <returns></returns>
        public Point GetPos()
        {
            return camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.World);
        }



        /// <summary>
        /// Performs a basic rendering of the map.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use to render the map.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, new Rectangle(0, 0, 25, 20));
        }

        /// <summary>
        /// Draws an area of the map defined in world space (pixel) coordinates.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use to render the map.</param>
        /// <param name="worldArea">The area of the map to draw in world coordinates.</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle worldArea)
        {
            // data is left-to-right, top-to-bottom
            foreach (Layer layer in Layers)
            {
                for (int x = 0; x < layer.Tiles.Width; x++)
                {
                    for (int y = 0; y < layer.Tiles.Height; y++)
                    {
                        if (layer.Tiles[x, y].Id == 0) continue;
                        Tile tile = layer.Tiles[x, y];
                        spriteBatch.Draw(
                            tile.ParentTileSet.Texture
                            , new Rectangle((x * 32) + (int)camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.World).X + Math.Abs(camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.Screen).X),
                                (y * 32) + (int)camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.World).Y + Math.Abs(camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.Screen).Y), 32, 32),
                                new Rectangle(tile.X, tile.Y, 32, 32),
                                Color.White);

                    }
                }
            }
        }

        /// <summary>
        /// Gets the screen center coordinates for an object of specified dimensions.
        /// </summary>
        /// <returns></returns>
        public Point GetScreenCenterCoordinates(Point spriteDimensions)
        {
            int x = (Game.graphics.PreferredBackBufferWidth / 2) - (spriteDimensions.X / 2);
            int y = (Game.graphics.PreferredBackBufferHeight / 2) - (spriteDimensions.Y / 2);

            return new Point(x, y);
        }
    }
}
