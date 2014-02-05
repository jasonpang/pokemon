using System;
using Microsoft.Xna.Framework;

namespace Pokemon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            MathHelper.Clamp(4, 3, 5);
            using (Game game = new Game())
            {
                game.Run();
            }

            
        }
    }
}

