using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace JewelClicker
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private double timer; 
        private const float frameInterval = 100f;
        private Engine engine;
        private MouseState oldState;
        private Menu menu;

        enum GameState
        {
            Menu,
            Playing
        }
        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
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
            gameState = GameState.Menu;
            
            
            graphics.PreferredBackBufferHeight = Engine.NUM_ROWS * (Jewel.HEIGHT + Jewel.VERTICAL_PADDING) + Engine.SCORE_HEIGHT;
            graphics.PreferredBackBufferWidth = Engine.NUM_COLS * Jewel.WIDTH;
            graphics.ApplyChanges();
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
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            menu = new Menu(this);
            menu.addButton("Start!", Color.Yellow, Color.Red, 100, 100, 200, 100, StartGame);
            engine = new Engine();
            Jewel.LoadJewelImages(Content);
            engine.LoadScoreFont(Content);
            menu.LoadButtons(Content, graphics);
            Components.Add(menu);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
            menu.UnloadButtons();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            MouseState newState = Mouse.GetState();

            if (gameState == GameState.Menu)
            {
                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    //menu.onMouseDown(newState);
                }
                else if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
                {
                    menu.onMouseUp(newState);
                }
            }

            else if (gameState == GameState.Playing)
            {
                timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer > frameInterval)
                {
                    timer = 0; 
                    //engine.UpdateJewels(gameTime);
                    base.Update(gameTime);
                }
            
                
                if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed) //mouse up
                {
                    engine.OnMouseUp(newState);
                }
                
            }

            oldState = newState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        private void StartGame()
        {
            gameState = GameState.Playing;
            menu.Visible = false;
            engine.MakeJewels(this);
        }
    }
}
