
using _3dcargame.GameObjects;
using _3dcargame.IO;
using _3dcargame.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace _3dcargame
{
    public class Cars3D : Game
    {
        private GraphicsDeviceManager _graphics;
        private Level level;
        private SpriteBatch _spriteBatch;
        //car
        Model model;

        //camera
        Camera camera;
        Matrix worldMatrix;

        //basic rendering
        BasicEffect basicEffect;

        public Cars3D()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            Camera.Instance.Init(GraphicsDevice);

            worldMatrix = Matrix.CreateWorld(Camera.Instance.Target, Vector3.Forward, Vector3.Up);

            Terrain.Instance.Init(GraphicsDevice);

            //basic effect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            model = Content.Load<Model>("Car");
            level = new Level(model, Camera.Instance.Target);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            Camera.Instance.Update(GraphicsDevice, gameTime);
            Vector3 direction = Camera.Instance.Target - Camera.Instance.Position;
            level.Update(Camera.Instance.Target + Vector3.Multiply(direction, 10));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            basicEffect.Projection = Camera.Instance.Projection;
            basicEffect.View = Camera.Instance.View;
            //basicEffect.World = Matrix.Identity;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            Terrain.Instance.Draw(GraphicsDevice, basicEffect, worldMatrix);
            level.Draw(Camera.Instance.View, Camera.Instance.Projection);
            base.Draw(gameTime);
        }
    }
}
