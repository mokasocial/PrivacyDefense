using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using SafeAndFree.Data;
using SafeAndFree.Game_States;
using SafeAndFree.InputHandlers;

namespace SafeAndFree
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEngine : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        /// <summary>
        /// Required to draw onto the screen.
        /// </summary>
        SpriteBatch spriteBatch;

        /// <summary>
        /// The currently active game screen.
        /// </summary>
        Screen currentGameScreen = null;

        public static GameEngine RunningEngine { get; private set; }

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            GameEngine.RunningEngine = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// <remarks>
        /// We have no game components to load.
        /// </remarks>
        protected override void Initialize()
        {
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

            // Give the TextureLibrary a reference to our ContentManager object.
            TextureLibrary.Content = this.Content;

            // Load a new Board object.
             Load(Screens.TITLE);
            //Load(Screens.GAME);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            TouchHandler.Update();

            if (null != currentGameScreen)
                currentGameScreen.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the current game screen, if one exists.
            if (null != currentGameScreen)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                currentGameScreen.Draw(spriteBatch);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Load a game screen to be the active one (drawn and updated every update loop).
        /// Only one screen can be active at any time.
        /// </summary>
        /// <param name="screenToLoad">The screen to set as active.</param>
        public void Load(Screens screenToLoad)
        {
            switch (screenToLoad)
            {
                case Screens.TITLE:
                    currentGameScreen = new BasicMenu(MEDIA_ID.TITLESCREEN, Screens.GAME);
                    break;
                case Screens.GAME:
                    currentGameScreen = new Board();
                    break;
                case Screens.LOSE:
                    currentGameScreen = new BasicMenu(MEDIA_ID.LOSESCREEN, Screens.TITLE);
                    break;
                case Screens.WIN:
                    currentGameScreen = new BasicMenu(MEDIA_ID.WINSCREEN, Screens.TITLE);
                    break;
            }
        }
    }
}
