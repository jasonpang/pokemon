using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;
using System.IO.Compression;

namespace Pokemon.Engine.Display
{
    /// <summary>
    /// Defines the player viewport in screen and world coordinates.
    /// </summary>
    public class Camera
    {
        private Vector2 screenCoordOffset, worldCoordOffset;

        /// <summary>
        /// Gets the offset distance, from (0, 0) of the game viewport to the player's sprite, in pixel units.
        /// </summary>
        public Vector2 ScreenCoordPixelOffset
        {
            get { return screenCoordOffset; }
            private set { screenCoordOffset = value; }
        }

        /// <summary>
        /// Gets the offset distance, from (0, 0) of the upper-left hand corner to the player's spawn point, in pixel units. subsequent panning of the camera will decrease and increase this value respectively.
        /// </summary>
        public Vector2 WorldCoordPixelOffset
        {
            get { return worldCoordOffset; }
            set { worldCoordOffset = value; }
        }

        /// <summary>
        /// Gets the offset distance, from (0, 0) of the game viewport to the player's sprite, in tile units.
        /// </summary>
        public Vector2 ScreenCoordTileOffset
        {
            get { return screenCoordOffset / 32; }
        }

        /// <summary>
        /// Gets the offset distance, from (0, 0) of the upper-left hand corner to the player's spawn point, in tile units. subsequent panning of the camera will decrease and increase this value respectively.
        /// </summary>
        public Vector2 WorldCoordTileOffset
        {
            get { return worldCoordOffset / 32; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="PlayerSpawnPoint">The player spawn point in tile units in world coordinates.</param>
        public Camera(Point PlayerSpawnPoint, Units units)
        {
            if (units == Units.Tile)
            {
                // Negative because the origin is moving away from us
                // The distance from the physical window's (screen coord) (0, 0) to the center of the player's foot
                ScreenCoordPixelOffset = new Vector2(-239, -177);
                // The distance from the map origin to the player's spawn point.
                WorldCoordPixelOffset = new Vector2(-(PlayerSpawnPoint.X * 32), -(PlayerSpawnPoint.Y * 32));
            }
            else
            {
                // Negative because the origin is moving away from us
                // The distance from the physical window's (screen coord) (0, 0) to the center of the player's foot
                ScreenCoordPixelOffset = new Vector2(-239, -177);
                // The distance from the map origin to the player's spawn point.
                WorldCoordPixelOffset = new Vector2(-(PlayerSpawnPoint.X), -(PlayerSpawnPoint.Y));
            }
        }

        /// <summary>
        /// Pans the camera the specified direction <paramref name="distance"/> units. Use this method to move
        /// the camera.
        /// </summary>
        /// <param name="direction">The direction to pan the camera.</param>
        /// <param name="distance">The distance to pan.</param>
        /// <param name="unit">Whether to use tile or pixel units when panning.</param>
        public void Pan(Direction direction, int distance, Units unit)
        {
            /* Note screen coordinates do not change when the camera pans, because the upper-left corner of the camera                * (the game viewport) always remains (0, 0). (0, 0) in screen coordinates is defined by the window's
             * upper left corner. Not relative to the map origin or player spawn.
             */
            if (unit == Units.Pixel)
            {
                switch (direction)
                {
                    case Direction.Down:
                        worldCoordOffset.Y -= distance;
                        break;
                    case Direction.Left:
                        worldCoordOffset.X += distance;
                        break;
                    case Direction.Up:
                        worldCoordOffset.Y += distance;
                        break;
                    case Direction.Right:
                        worldCoordOffset.X -= distance;
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case Direction.Down:
                        worldCoordOffset.Y -= distance * 32;
                        break;
                    case Direction.Left:
                        worldCoordOffset.X += distance * 32;
                        break;
                    case Direction.Up:
                        worldCoordOffset.Y += distance * 32;
                        break;
                    case Direction.Right:
                        worldCoordOffset.X -= distance * 32;
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the offset distance of the specified tile coordinate from (0, 0), with (0, 0) being the upper-left corner tile.
        /// </summary>
        public Point GetOffsetFromOrigin(Units returnUnits, Coordinates returnCoordinateSystem)
        {
            if (returnCoordinateSystem == Coordinates.Screen)
            {
                // Player coordinates
                if (returnUnits == Units.Pixel)
                {
                    // Pixel units
                    return new Point((int)ScreenCoordPixelOffset.X, (int)ScreenCoordPixelOffset.Y);
                }
                else
                {
                    // Tile units
                    return new Point((int)ScreenCoordTileOffset.X, (int)ScreenCoordTileOffset.Y);
                }
            }
            else
            {
                // World coordinates
                if (returnUnits == Units.Pixel)
                {
                    // Pixel units
                    return new Point((int)WorldCoordPixelOffset.X, (int)WorldCoordPixelOffset.Y);
                }
                else
                {
                    // Tile units
                    return new Point((int)WorldCoordTileOffset.X, (int)WorldCoordTileOffset.Y);
                }
            }
        }
    }

    /// <summary>
    /// Defines the camera distance unit.
    /// </summary>
    public enum Units
    {
        /// <summary>
        /// The main game viewport is 15 tile units wide by 10 tile units high. One tile unit is equivalent to 32 pixel units.
        /// </summary>
        Tile,
        /// <summary>
        /// The main game viewport is 480 pixel units by 320 pixel units. One pixel unit is 1/32th of a tile unit.
        /// </summary>
        Pixel,
    }

    /// <summary>
    /// Defines the camera coordinates unit.
    /// </summary>
    public enum Coordinates
    {
        /// <summary>
        /// (0, 0) is the upper-left corner of the physical game window's viewport on the client's operating system (viewport because the window includes the titlebar, which isn't part of this).
        /// </summary>
        Screen,
        /// <summary>
        /// Tile (0, 0) is the top-left corner tile of the map. In pixel units, (0, 0) still becomes the very top-left corner of the tile.
        /// </summary>
        World
    }
}
