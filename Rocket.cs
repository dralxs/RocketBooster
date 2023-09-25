using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Screens;
using System.Diagnostics;

namespace RocketBooster
{
    internal class Rocket
    {
        Texture2D rocketTexture;
        private OrthographicCamera _camera;   
        private int speed;
        private int damage;
        private float angle;
        private Vector2 imageCenter;
        private const float RotationSpeed = 0.1f;

        public Vector2 _cameraPosition;

        // PUBLIC FUNCTION //

        public Rocket(OrthographicCamera camera)
        {
            _camera = camera;
            _cameraPosition = new Vector2(0, 0);
            Speed = 500;
            damage = 1;
        }
        
        public void LoadContent(ContentManager content)
        {
            this.rocketTexture = content.Load<Texture2D>("rocket1");
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 screenCenter)
        {
            this.imageCenter = new Vector2(rocketTexture.Width / 2f, rocketTexture.Height / 2f);
            spriteBatch.Draw(rocketTexture, screenCenter, null, Color.White, this.angle, imageCenter, 1f, SpriteEffects.None, 0f);
        }

        public void MoveCamera(GameTime gameTime)
        {
            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection();
            this._cameraPosition += this.speed * movementDirection * seconds;
        }

        public int Speed { get => speed; set => speed = value; }

        // PRIVATE FUNCTION //

        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();

            // Handle movement
            if (state.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }

            // Calculate the target angle based on the movement direction
            if (movementDirection != Vector2.Zero)
            {
                float targetAngle = MathF.Atan2(movementDirection.X, -movementDirection.Y);
                Debug.WriteLine("Target Angle: " + targetAngle);

                // Smoothly transition the current angle towards the target angle
                float deltaAngle = targetAngle - this.angle;
                if (MathF.Abs(deltaAngle) > MathF.PI)
                {
                    deltaAngle += MathF.Sign(deltaAngle) * 2 * MathF.PI;
                }
                this.angle += deltaAngle * RotationSpeed;

                // Normalize the movement direction
                movementDirection.Normalize();
            }

            return movementDirection;
        }

    }
} 

