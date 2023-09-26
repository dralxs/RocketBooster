using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System.Collections.Generic;
using System.Diagnostics;

namespace RocketBooster
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;
        private OrthographicCamera _camera;
        private SpriteFont camerapositionfont;
        private SpriteFont speedfont;
        private List<Asteroid> asteroids;

        private Rocket playerRocket;

        Vector2 screenCenter;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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
            // Instanciate the main rocket
            playerRocket = new Rocket(_camera);
            // Get the center of the screen
            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            screenCenter =  new Vector2(viewport.Width / 2f, viewport.Height / 2f);
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
            SpawnObstacles(_tiledMap);
            // Texture asteroids
            foreach (var asteroid in asteroids)
            {
                asteroid.LoadContent(Content);
            }
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            // Debug Panel
            camerapositionfont = Content.Load<SpriteFont>("CameraPosition");
            speedfont = Content.Load<SpriteFont>("Speed");
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _tiledMapRenderer.Update(gameTime);
            // Update the camera 
            playerRocket.MoveCamera(gameTime);
            _camera.LookAt(playerRocket._cameraPosition);
            // Update asteroids 
            foreach (var asteroid in asteroids)
            {
                asteroid.Update(gameTime, playerRocket);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start of the drawing 
            _spriteBatch.Begin();

            // Draw the rocket 
            playerRocket.Draw(_spriteBatch, this.screenCenter);
            _spriteBatch.DrawString(camerapositionfont, "CameraPosition : " + playerRocket._cameraPosition, new Vector2(0,0), Color.Black);
            _spriteBatch.DrawString(speedfont, "Speed : " + playerRocket.Speed, new Vector2(0, 15), Color.Black);
            _tiledMapRenderer.Draw(_camera.GetViewMatrix());
            // Draw asteroids
            foreach (var asteroid in asteroids)
            {
                asteroid.Draw(_spriteBatch);
            }
            
            // End of the drawing
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SpawnObstacles(TiledMap tiledMap)
        {
            var objects = tiledMap.GetLayer<TiledMapObjectLayer>("AsteroidSpawn").Objects;

            foreach (var obj in objects)
            {
                var asteroid = new Asteroid(this, obj.Position);
                Debug.WriteLine("Asteroid position : ", asteroid.Position);
                asteroids.Add(asteroid);
            }
        }
    }
}