using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pokemon.Engine.Display
{
    /// <summary>
    /// Represents any drawable game entity: the player character, other online players, NPCs, pokeballs, other pick-up items. Tiles are not considered sprites - tiles are tiles.
    /// </summary>
    interface ISprite : IDrawable, IUpdateable
    {

    }
}
