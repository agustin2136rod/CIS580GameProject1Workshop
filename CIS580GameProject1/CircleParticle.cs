/*CircleParticle.cs
 * Author: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CIS580GameProject1
{
    /// <summary>
    /// Class to represent a circle particle that trails the ball
    /// </summary>
    public class CircleParticle
    {
        //All properties for the circle particles
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int Time { get; set; }

        /// <summary>
        /// Constructor for DiamondParticle class
        /// </summary>
        /// <param name="texture">texture of the particle</param>
        /// <param name="position">position of the particle</param>
        /// <param name="velocity">velocity of the partcile</param>
        /// <param name="angle">angle of the particle</param>
        /// <param name="angleVelocity">angular velocity of the particle</param>
        /// <param name="color">color of the particle</param>
        /// <param name="size">size of the particle</param>
        /// <param name="time">time left till particle dissipates</param>
        public CircleParticle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angleVelocity, Color color, float size, int time)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angleVelocity;
            Color = color;
            Size = size;
            Time = time;
        }

        /// <summary>
        /// Method to update the particles in real time
        /// </summary>
        public void Update()
        {
            Position += Velocity;
            Angle += AngularVelocity;
            Time--;
        }

        /// <summary>
        /// Method to draw the particles on the game screen
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used to draw</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            
            Rectangle source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 center = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, Position, source, Color, Angle, center, Size, SpriteEffects.None, 0f);
        }
    }
}
