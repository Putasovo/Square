using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mojehra
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // game state
        GameState gameState = GameState.Menu;

        // Increment 1: opening screen fields
        Texture2D openingScreen;
        Rectangle openingRect;

        // Increment 2: board field
        Vector2 boardCenter;
        NumberBoard deska;
        // Increment 5: random field
        System.Random rand = new System.Random();
        // Increment 5: new game sound effect field
        SoundEffect winSound;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //GraphicsAdapter.UseReferenceDevice = true;
            // Increment 1: set window resolution and make mouse visible
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            openingRect.Width = graphics.PreferredBackBufferWidth;
            openingRect.Height = graphics.PreferredBackBufferHeight;

            boardCenter = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

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

            // Increment 1: load opening screen and set opening screen draw rectangle
            openingScreen = Content.Load <Texture2D>(@"graphics/openingscreen");
            // Increment 2: create the board object (this will be moved before you're done with the project)
            StartGame();
            // Increment 5: load new game sound effect
            winSound = Content.Load<SoundEffect>(@"audio/applause");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keys = Keyboard.GetState();
            // Increment 2: change game state if game state is GameState.Menu and user presses Enter
            if (keys.IsKeyDown(Keys.Enter)) 
            {
                gameState = GameState.Play;
            }
            // Increment 4: if we're actually playing, update mouse state and update board
            if (gameState == GameState.Play)
            {
                MouseState mouse = Mouse.GetState();
                bool guessed = (deska.Update(gameTime, mouse));
            
            // Increment 5: check for correct guess
                if (guessed)
                {
                    winSound.Play();
                    StartGame();
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // Increments 1 and 2: draw appropriate items here
            if (gameState == GameState.Menu)
            {
                spriteBatch.Draw(openingScreen, openingRect, Color.White);
            }
            else
            {
                deska.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Starts a game
        /// </summary>
        void StartGame()
        {
            // Increment 5: randomly generate new number for game
            int correctNum = rand.Next(1,10);
            // Increment 5: create the board object
            deska = new NumberBoard(Content, boardCenter,
                (int)(graphics.PreferredBackBufferHeight / 1.1), correctNum);
        }
    }
}