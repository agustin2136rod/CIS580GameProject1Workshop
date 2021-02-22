/*GameProject1.cs
 * Written By: Agustin Rodriguez
 */
using CIS580GameProject1.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Timers;

namespace CIS580GameProject1
{
    /// <summary>
    /// Class to represent the entire GameProject1 where a slime ghost has to evade a moving ball for as long as possible
    /// </summary>
    public class GameProject1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //set up all variables
        private Vector2 ballPosition;
        private Vector2 ballVelocity;
        private Texture2D ballTexture;
        private SlimeGhostSprite slimeGhost;
        private SpriteFont spriteFont;
        private BoundingCircle bounding;
        private Stopwatch stopWatch;

        private KeyboardState keyboardState;
        private bool gameOver;




        /// <summary>
        /// constructor
        /// </summary>
        public GameProject1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //instructions for the game
            Window.Title = "Don't Get Hit Game - Avoid getting hit by the ball for as long as possible!";
            
        }

        /// <summary>
        /// initialization of the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            bounding = new BoundingCircle(ballPosition, 64);
            System.Random random = new System.Random();
            ballVelocity = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            ballVelocity.Normalize();
            ballVelocity *= 1500;
            slimeGhost = new SlimeGhostSprite();
            stopWatch = new Stopwatch();
            gameOver = false;


            base.Initialize();
        }

        /// <summary>
        /// content to load into the game
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");
            slimeGhost.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("arial");

            // TODO: use this.Content to load your game content here
        }

        private void restartGame()
        {
            Initialize();
        }

        /// <summary>
        /// Update the game as events happen
        /// </summary>
        /// <param name="gameTime">total game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();  

            // TODO: Add your update logic here
            slimeGhost.Update(gameTime);
            slimeGhost.Color = Color.White;
            if (slimeGhost.Bounds.CollidesWith(bounding))
            {
                slimeGhost.Color = Color.Black;
                gameOver = true;
                while (gameOver)
                {
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        gameOver = false;
                        restartGame();
                    }
                }
            }

            //code receieved from Nathan Bean to implement the ball moving across the screen from HelloGame Demo
            ballPosition += (float)gameTime.ElapsedGameTime.TotalSeconds * ballVelocity;
            bounding.Center = ballPosition;
            if (ballPosition.X < GraphicsDevice.Viewport.X || ballPosition.X > GraphicsDevice.Viewport.Width - 64)
            {
                ballVelocity.X *= -1;
            }
            if (ballPosition.Y < GraphicsDevice.Viewport.Y || ballPosition.Y > GraphicsDevice.Viewport.Height - 64)
            {
                ballVelocity.Y *= -1;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Method that draws each of the game pieces on the window
        /// </summary>
        /// <param name="gameTime">total time of the game</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            slimeGhost.Draw(gameTime, _spriteBatch);
            _spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            _spriteBatch.DrawString(spriteFont, "Total time without being hit: " + gameTime.ElapsedGameTime.TotalSeconds.ToString(), new Vector2(2, 2), Color.Gold);

            if (gameOver)
            {
                string msg = "Press 'Space Bar' to play again!";
                Vector2 sizeOfStr = spriteFont.MeasureString(msg);
                _spriteBatch.DrawString(spriteFont, msg, new Vector2((GraphicsDevice.Viewport.Width / 2) - (sizeOfStr.X / 2),
                    (GraphicsDevice.Viewport.Height / 2) - (sizeOfStr.Y / 2)), Color.White);

            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
