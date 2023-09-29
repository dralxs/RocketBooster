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
        public IShapeF Bounds { get; set; }

        protected Obstacle(Game1 game, Vector2 position)
        {
            _game = game;
            Position = position;
            Velocity = new Vector2(100,0);
        }

        public abstract void Draw(SpriteBatch spriteBatch, Vector2 screenCenter);
        public virtual void Update(GameTime gameTime)
        {

        }
        public abstract void Update(GameTime gameTime, Rocket playerRocket);
        public abstract void OnCollision(CollisionEventArgs collisionInfo);
    }
}
