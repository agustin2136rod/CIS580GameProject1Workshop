/*GameProject1.cs
 * Written By: Agustin Rodriguez
 */
using CIS580GameProject1.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using System.Timers;

namespace CIS580GameProject1
{
    /// <summary>
    /// Class to represent the entire GameProject1 where a slime ghost has to evade a moving ball for as long as possible
    /// </summary>
    public class GameProject4 : Game
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
        private double gameClock;

        private KeyboardState keyboardState;
        private bool gameOver;

        //music and sound effect variables
        private SoundEffect slimeHit;
        private Song backgroundMusic;

        //Particle engine for ball effects
        ParticleEngine particleEngine;

        //Particle engine for collision effects
        ExplosionEngine explosionEngine;

        //layer textures
        private Texture2D _background;
        private Texture2D _foreground;
        private Texture2D _midground;

        /// <summary>
        /// constructor
        /// </summary>
        public GameProject4()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //instructions for the game
            Window.Title = "Slime Dodge - Avoid getting hit by the ball for as long as possible!";
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        /// <summary>
        /// initialization of the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            slimeGhost = new SlimeGhostSprite();
            startGame();
            base.Initialize();
        }

        /// <summary>
        /// content to load into the game
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");
            Texture2D ballEffect = Content.Load<Texture2D>("circle");
            Texture2D explosionEffect = Content.Load<Texture2D>("Explosion");
            slimeGhost.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("arial");
            backgroundMusic = Content.Load<Song>("music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            slimeHit = Content.Load<SoundEffect>("Hit_Hurt5");
            // TODO: use this.Content to load your game content here

            particleEngine = new ParticleEngine(ballEffect, ballPosition, Color.Brown);
            explosionEngine = new ExplosionEngine(explosionEffect, bounding.Center, Color.Orange);

            _background = Content.Load<Texture2D>("Background");
            _foreground = Content.Load<Texture2D>("foreground");
            _midground = Content.Load<Texture2D>("midground");
        }

        private void startGame()
        {
            ballPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            bounding = new BoundingCircle(ballPosition, 32);
            Random random = new Random();
            ballVelocity = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            ballVelocity.Normalize();
            ballVelocity *= 1500;
            slimeGhost.Reset();
            gameOver = false;
            gameClock = 0;
            MediaPlayer.Play(backgroundMusic);
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

            if (gameOver)
            {
                explosionEngine.Emit = new Vector2(bounding.Center.X, bounding.Center.Y);
                explosionEngine.Update();
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    gameOver = false;
                    startGame();
                }
            }
            else
            {
                // TODO: Add your update logic here
                slimeGhost.Update(gameTime);
                slimeGhost.Color = Color.White;
                if (slimeGhost.Bounds.CollidesWith(bounding))
                {
                    slimeGhost.Color = Color.Black;
                    gameOver = true;
                    slimeHit.Play();
                    MediaPlayer.Stop();
                }

                //code receieved from Nathan Bean to implement the ball moving across the screen from HelloGame Demo
                ballPosition += (float)gameTime.ElapsedGameTime.TotalSeconds * ballVelocity;
                particleEngine.Emit = new Vector2(bounding.Center.X, bounding.Center.Y);
                particleEngine.Update();
                bounding.Center = ballPosition;
                if (ballPosition.X < GraphicsDevice.Viewport.X || ballPosition.X > GraphicsDevice.Viewport.Width - 64)
                {
                    ballVelocity.X *= -1;
                }
                if (ballPosition.Y < GraphicsDevice.Viewport.Y || ballPosition.Y > GraphicsDevice.Viewport.Height - 64)
                {
                    ballVelocity.Y *= -1;
                }
                gameClock += Math.Round(gameTime.ElapsedGameTime.TotalSeconds, 3);
                
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

            float playerX = MathHelper.Clamp(slimeGhost.Position.X, 50, 2000);
            float offset = 200 - playerX;

            Matrix transform;

            transform = Matrix.CreateTranslation(offset * 0.333f, 0, 0);
            _spriteBatch.Begin(transformMatrix: transform);
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.End();

            transform = Matrix.CreateTranslation(offset * 0.666f, 0, 0);
            _spriteBatch.Begin(transformMatrix: transform);
            _spriteBatch.Draw(_midground, Vector2.Zero, Color.White);
            _spriteBatch.End();

            transform = Matrix.CreateTranslation(offset, 0, 0);
            _spriteBatch.Begin(transformMatrix: transform);
            _spriteBatch.Draw(_foreground, Vector2.Zero, Color.White);
            slimeGhost.Draw(gameTime, _spriteBatch);
            _spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            particleEngine.Draw(_spriteBatch);
            explosionEngine.Draw(_spriteBatch);
            _spriteBatch.DrawString(spriteFont, "Total time without being hit: " + gameClock, new Vector2(2, 2), Color.Gold);

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
