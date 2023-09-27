using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RocketBooster
{
    internal class Asteroid : Obstacle
    {
        Texture2D asteroidTexture;
        public Asteroid(Game1 game, Vector2 position) : base(game, position)
        {

        }

        public void LoadContent(ContentManager content)
        {
            this.asteroidTexture = content.Load<Texture2D>("asteroid1");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(asteroidTexture, this.Position, null, Color.White);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            Velocity.X *= -1;
            Velocity.Y *= -1;
            Bounds.Position -= collisionInfo.PenetrationVector;
        }
        public override void Update(GameTime gameTime, Rocket playerRocket)
        {
            Vector2 oppositeDirection = -playerRocket.GetMovementDirection();
            this.Position += oppositeDirection * playerRocket.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
