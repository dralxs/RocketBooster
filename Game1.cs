using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System.Collections.Generic;
using System.Diagnostics;
using MonoGame.Extended.Collisions;

namespace RocketBooster
{
    public class Game1 : Game
    {
        const int MapWidth = 10000000;
        const int MapHeight = 10000000;

        private readonly CollisionComponent _collisionComponent;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private Vector2 screenCenter;
        private SpriteFont camerapositionfont;
        private SpriteFont speedfont;
        private List<Asteroid> asteroids;
        private Rocket playerRocket;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Instanciate the camera
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 720);
            _camera = new OrthographicCamera(viewportadapter);
            // Get the center of the screen
            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            screenCenter =  new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            // Instanciate the main rocket
            playerRocket = new Rocket(_camera);
            // List of asteroid
            asteroids = new List<Asteroid>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // Texture rocket
            playerRocket.LoadContent(this.Content);
            _tiledMap = Content.Load<TiledMap>("maprocketbooster");
            // Spawn obstacles 
            SpawnObstacles(_tiledMap);
            // Texture asteroids
            foreach (var asteroid in asteroids)
            {
                asteroid.LoadContent(Content);
                // Add asteroids collisions
                _collisionComponent.Insert(asteroid);
            }
            // Add rocket collisions
            _collisionComponent.Insert(playerRocket);
            // Render map
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            // Debug Panel 
            camerapositionfont = Content.Load<SpriteFont>("CameraPosition");
            speedfont = Content.Load<SpriteFont>("Speed");  
        }

        protected override void Update(GameTime gameTime)
        {
            // Update player
            playerRocket.Update(gameTime);
            _camera.LookAt(playerRocket.CameraPosition);
            // Update asteroids 
            foreach (var asteroid in asteroids)
            {
                asteroid.Update(gameTime, playerRocket);
            }
            // Update collisions
            _collisionComponent.Update(gameTime);
            // Update map
            _tiledMapRenderer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Start of the drawing 
            _spriteBatch.Begin();
            // Draw the rocket 
            playerRocket.Draw(_spriteBatch, this.screenCenter);
            // Draw asteroids
            foreach (var asteroid in asteroids)
            {
                asteroid.Draw(_spriteBatch, this.screenCenter);
            }
            // Draw the map
            _tiledMapRenderer.Draw(_camera.GetViewMatrix());
            // Draw Debug Panel
            _spriteBatch.DrawString(camerapositionfont, "CameraPosition : " + playerRocket.CameraPosition, new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(speedfont, "Speed : " + playerRocket.Speed, new Vector2(0, 15), Color.Black);
            // End of the drawing
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /** 
         * @param tiledMap : map of the game
         * @return void
         */
        public void SpawnObstacles(TiledMap tiledMap)
        {
            var objects = tiledMap.GetLayer<TiledMapObjectLayer>("AsteroidSpawn").Objects;

            foreach (var obj in objects)
            {
                var asteroid = new Asteroid(this, obj.Position, new RectangleF(obj.Position, new Size2(500, 500)));
                asteroids.Add(asteroid);
            }
        }
    }
}