using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;


namespace RocketBooster
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;
        private OrthographicCamera _camera;
        private SpriteFont font;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // Texture rocket
            playerRocket.LoadContent(this.Content);
            //asteroidTexture = Content.Load<Texture2D>("asteroid1");
            _tiledMap = Content.Load<TiledMap>("rocketboostermap");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            // Debug Panel
            font = Content.Load<SpriteFont>("CameraPosition");
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _tiledMapRenderer.Update(gameTime);
            // Update the camera 
            playerRocket.MoveCamera(gameTime);
            _camera.LookAt(playerRocket._cameraPosition);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start of the drawing 
            _spriteBatch.Begin();

            // Draw the rocket 
            playerRocket.Draw(_spriteBatch, this.screenCenter);
            _spriteBatch.DrawString(font, "CameraPosition : " + playerRocket._cameraPosition, new Vector2(0,0), Color.Black);

            // End of the drawing
            _spriteBatch.End();

            _tiledMapRenderer.Draw(_camera.GetViewMatrix());

            base.Draw(gameTime);
        }
    }
}