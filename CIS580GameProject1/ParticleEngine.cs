/*ParticleEngine.cs
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
    /// Class to trail particles behind moving objects
    /// </summary>
    public class ParticleEngine
    {
        Random random;
        private List<DiamondParticle> particles;
        public Vector2 Emit { get; set; }

        public Color Color { get; set; }

        private Texture2D spriteTexture;

        /// <summary>
        /// Constructor for the ParticleEngine Class
        /// </summary>
        /// <param name="texture">Sprite Texture</param>
        /// <param name="location">location the particles need to be emitted to</param>
        public ParticleEngine(Texture2D texture, Vector2 location, Color color)
        {
            Emit = location;
            spriteTexture = texture;
            particles = new List<DiamondParticle>();
            random = new Random();
            this.Color = color;
        }

        /// <summary>
        /// Method to generate a new DiamondParticle within the game screen
        /// </summary>
        /// <returns>the newly created DiamondParticle</returns>
        private DiamondParticle Generate()
        {
            Texture2D texture = spriteTexture;
            Vector2 position = Emit;

            Vector2 velocity = new Vector2(1f * (float)(random.NextDouble() * 2 - 1), 1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = Color;
            float size = (float)random.NextDouble();
            int time = 10 + random.Next(20);

            return new DiamondParticle(texture, position, velocity, angle, angularVelocity, color, size, time);
        }

        /// <summary>
        /// Method to update each of the particles within the particles list
        /// </summary>
        public void Update()
        {
            int particles = 4;
            for (int i = 0; i < particles; i++)
            {
                this.particles.Add(Generate());
            }
            for (int j = 0; j < this.particles.Count; j++)
            {
                this.particles[j].Update();
                if (this.particles[j].Time <= 0)
                {
                    this.particles.RemoveAt(j);
                    particles--;
                }
            }
        }


        /// <summary>
        /// Method to draw each of the particles within the particles list 
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch passed in to draw all the particles</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            for (int i = 0; i < this.particles.Count; i++)
            {
                this.particles[i].Draw(spriteBatch);
            }
            //spriteBatch.End();
        }
    }
}
