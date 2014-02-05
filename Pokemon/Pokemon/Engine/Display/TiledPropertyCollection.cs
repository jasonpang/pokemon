using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Pokemon.Engine.Display
{
    /// <summary>
    /// An enumerable collection of properties.
    /// </summary>
    public class TiledPropertyCollection : IEnumerable<TiledProperty>
    {
        // cheating under the hood :)
        private readonly Dictionary<string, TiledProperty> values = new Dictionary<string, TiledProperty>();

        /// <summary>
        /// Gets a TiledProperty with the given name.
        /// </summary>
        /// <param name="name">The name of the TiledProperty to retrieve.</param>
        /// <returns>The TiledProperty if a matching one is found or null if no TiledProperty exists for the given name.</returns>
        public TiledProperty this[string name]
        {
            get
            {
                TiledProperty p;
                if (values.TryGetValue(name, out p))
                    return p;
                return null;
            }
        }

        /// <summary>
        /// Creates a new TiledPropertyCollection.
        /// </summary>
        public TiledPropertyCollection() { }

        /// <summary>
        /// Adds a TiledProperty to the collection.
        /// </summary>
        /// <param name="TiledProperty">The TiledProperty to add.</param>
        public void Add(TiledProperty TiledProperty)
        {
            values.Add(TiledProperty.Name, TiledProperty);
        }

        /// <summary>
        /// Attempts to get a TiledProperty by name.
        /// </summary>
        /// <param name="name">The name of the TiledProperty to retrieve.</param>
        /// <param name="TiledProperty">The TiledProperty that is found, if one matches.</param>
        /// <returns>True if the TiledProperty was found, false otherwise.</returns>
        public bool TryGetValue(string name, out TiledProperty TiledProperty)
        {
            return values.TryGetValue(name, out TiledProperty);
        }

        /// <summary>
        /// Removes a TiledProperty with the given name.
        /// </summary>
        /// <param name="name">The name of the TiledProperty to remove.</param>
        /// <returns>True if the TiledProperty was removed, false otherwise.</returns>
        public bool Remove(string name)
        {
            return values.Remove(name);
        }

        // internal constructor because games shouldn't make their own TiledPropertyCollections
        internal TiledPropertyCollection(ContentReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                string value = reader.ReadString();

                values.Add(key, new TiledProperty(key, value));
            }
        }

        /// <summary>
        /// Gets an enumerator that can be used to iterate over the properties in the collection.
        /// </summary>
        /// <returns>An enumerator over the properties.</returns>
        public IEnumerator<TiledProperty> GetEnumerator()
        {
            return values.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.Values.GetEnumerator();
        }
    }
}
