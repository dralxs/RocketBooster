﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;

namespace RocketBooster
{
    internal interface IEntity : ICollisionActor
    {
        public void Update(GameTime gameTime, Rocket playerRocket);
        public void Draw(SpriteBatch spriteBatch);
    }
}