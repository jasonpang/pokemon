using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Pokemon.Engine.Display;
using DebugTerminal;

namespace Pokemon
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static Map StarterMap;
        public static Engine.Display.PlayerSprite Player;
        SpriteFont spriteFont;

        public Game()
        {
            
            Window.Title = "Pokémon Revolution";
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            // Set the window size
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 640;
            graphics.PreferredBackBufferWidth = 15 * 32;
            graphics.PreferredBackBufferHeight = 10 * 32;
            Content.RootDirectory = "Data";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Player = new Engine.Display.PlayerSprite();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            StarterMap = new Map(@"TestMap2_City_XML.tmx", GraphicsDevice);
            Point a = StarterMap.camera.GetOffsetFromOrigin(Units.Tile, Coordinates.World);
            Point b = StarterMap.camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.World);
            Point c = StarterMap.camera.GetOffsetFromOrigin(Units.Tile, Coordinates.Screen);
            Point d = StarterMap.camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.Screen);
            Stream textureStream = new FileStream(@"Brendan_Walking.png", FileMode.Open, FileAccess.Read, FileShare.Read);
            Texture2D texture = Texture2D.FromStream(GraphicsDevice, textureStream);
            Player.Load(texture, spriteBatch, @"Brendan_Walking.xml");
            Player.Visible = true;
            Player.Enabled = true;
            Player.FrameDuration = 115;

            //d = new World();
            //d.EnabledChanged += new EventHandler<EventArgs>(d_EnabledChanged);

            spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            Terminal.Init(this, spriteBatch, spriteFont, GraphicsDevice);
            TerminalSkin ts = new TerminalSkin(TerminalThemeType.WHITE_ON_BLACK_GRADIENT, TerminalThemeSHType.WHITE_ON_BLACK_GRADIENT);
            Terminal.SetSkin(ts);
        }

        void d_EnabledChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {



            // TODO: Add your update logic here
            StarterMap.Update();
            Player.Update(gameTime);

            if (!Terminal.CheckOpen(Keys.OemTilde, Keyboard.GetState()))
            {
            
            }
Input.InputManager.Update(gameTime);
            base.Update(gameTime);
        }

        public string D1()
        {
            return "Map Offset: " + StarterMap.camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.World);
        }

        public string D2()
        {
            return "Player Offset: " + Game.StarterMap.camera.GetOffsetFromOrigin(Units.Pixel, Coordinates.Screen);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            // TODO: Add your drawing code here
            StarterMap.Draw(spriteBatch);
            Player.Draw(gameTime);
            spriteBatch.End();
            Terminal.CheckDraw(false);

            base.Draw(gameTime);
        }
    }
}
