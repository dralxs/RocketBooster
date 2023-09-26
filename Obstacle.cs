using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace RocketBooster
{
    abstract internal class Obstacle : IEntity
    {
        private readonly Game1 _game;
        public Vector2 Position;
        public Vector2 Velocity;
        public IShapeF Bounds { get; }

        protected Obstacle(Game1 game, Vector2 position)
        {
            _game = game;
            Position = position;
            Velocity = new Vector2(100,0);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 3);
        }

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
            Velocity.X *= -1;
            Velocity.Y *= -1;
            Bounds.Position -= collisionInfo.PenetrationVector;
        }

        public virtual void Update(GameTime gameTime, Rocket playerRocket)
        {
            Vector2 directionToPlayer = playerRocket._cameraPosition - this.Position;
            directionToPlayer.Normalize();
            this.Position += directionToPlayer * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
