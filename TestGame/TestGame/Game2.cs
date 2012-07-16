using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.DebugViews;
using FarseerPhysics;
using JoyOS.System.Screen;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game2 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D bird;
        Texture2D floor;

        //Farseer objects
        World MyWorld;
        Body HeroBird, FloorBody;

        DebugViewXNA DebugView;
        Camera2D Camera;
        public Game2()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Test solution";

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;

            Content.RootDirectory = "Content";

            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Create New World with gravity of 10 units, downward.
            MyWorld = new World(Vector2.UnitY * 10);
            bird = Content.Load<Texture2D>("AngryRedBird");
            floor = Content.Load<Texture2D>("blank");


            //camera

            DebugView = new DebugViewXNA(MyWorld);
            DebugView.AppendFlags(DebugViewFlags.Shape);
            DebugView.RemoveFlags(DebugViewFlags.Joint);
            DebugView.DefaultShapeColor = Color.White;
            DebugView.SleepingShapeColor = Color.LightGray;
            DebugView.LoadContent(GraphicsDevice, Content);
            Camera = new Camera2D(GraphicsDevice);

            //Create Floor
            FloorBody = BodyFactory.CreateBody(MyWorld);
            Fixture floorFixture = FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(480), ConvertUnits.ToSimUnits(10), 10, Vector2.Zero, FloorBody);
            floorFixture.Restitution = 0.5f;        //Bounceability
            floorFixture.Friction = 0.5f;           //Friction
            FloorBody = floorFixture.Body;          //Get Body from Fixture
            FloorBody.IsStatic = true;              //Floor must be stationary object

            //Create Box, (Note:Different way from above code, just to show it otherwise there is no difference)
            HeroBird = BodyFactory.CreateBody(MyWorld);
            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(50), ConvertUnits.ToSimUnits(50), 10, Vector2.Zero, HeroBird);
            foreach (Fixture fixture in HeroBird.FixtureList)
            {
                fixture.Restitution = 0.5f;
                fixture.Friction = 0.5f;
            }
            HeroBird.BodyType = BodyType.Dynamic;

            //Place floor object to bottom of the screen.
            FloorBody.Position = ConvertUnits.ToSimUnits(new Vector2(240, 700));

            //Place Box on screen, somewhere
            HeroBird.Position = ConvertUnits.ToSimUnits(new Vector2(240, 25));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (MyWorld != null)
            {
                // Update the camera
                Camera.Update(gameTime);

                // variable time step but never less then 30 Hz
                MyWorld.Step(Math.Min((float)gameTime.ElapsedGameTime.Seconds, (1f / 30f)));
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            if (MyWorld != null)
            {
                Matrix projection = Camera.SimProjection;
                Matrix view = Camera.SimView;
                //Render the Data
                DebugView.RenderDebugData(ref projection, ref view);
            }


            base.Draw(gameTime);
        }
    }
}