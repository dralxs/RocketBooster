using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RocketBooster
{
    internal class Asteroid : Obstacle
    {
        Texture2D asteroidTexture;
        public Asteroid(Game1 game, Vector2 position, RectangleF rectangleF) : base(game, position)
        {
            Bounds = rectangleF;
        }

        public void LoadContent(ContentManager content)
        {
            this.asteroidTexture = content.Load<Texture2D>("asteroid1");
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 screenCenter)
        {
            spriteBatch.Draw(asteroidTexture, this.Position, null, Color.White);
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 3);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            //Velocity.X *= -1;
            //Velocity.Y *= -1;
            this.Position -= collisionInfo.PenetrationVector;
            Bounds.Position -= collisionInfo.PenetrationVector;
            Debug.WriteLine("test");
        }
        public override void Update(GameTime gameTime, Rocket playerRocket)
        {
            Vector2 oppositeDirection = -playerRocket.GetMovementDirection();
            this.Position += oppositeDirection * playerRocket.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Bounds.Position += oppositeDirection * playerRocket.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

    }
}
