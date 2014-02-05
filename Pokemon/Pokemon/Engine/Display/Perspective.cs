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
    /// A Point with a unit and coordinate system. Used by the Camera class.
    /// </summary>
    public class Perspective
    {
        private Point point;
        private Units unit;
        private Coordinates coordinates;

        /// <summary>
        /// Initializes a new instance of the <see cref="Perspective"/> class.
        /// </summary>
        public Perspective(Point point, Units unit, Coordinates coordinates)
        {
            this.point = point;
            this.unit = unit;
            this.coordinates = coordinates;
        }

        /// <summary>
        /// Gets or sets the perspective's point.
        /// </summary>
        public Point Point
        {
            get { return point; }
            set { point = value; }
        }

        /// <summary>
        /// Gets or sets the perspective's unit.
        /// </summary>
        public Units Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        /// <summary>
        /// Gets or sets the perspective's coordinate system.
        /// </summary>
        public Coordinates Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }
    }
}
