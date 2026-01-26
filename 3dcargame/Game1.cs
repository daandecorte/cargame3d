
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

        //geometrische   info
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

        //terrain
        VertexPositionColor[] terrainVertices;
        VertexBuffer terrainBuffer;
        int terrainWidth=100;
        int terrainHeight=100;
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

            CreateTerrain();

            //basic effect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;

            //want to see the colors of the vertices, this needs to be true
            basicEffect.VertexColorEnabled = true;

            //for custom lighting, this needs to be false
            basicEffect.LightingEnabled = false;

            //triangle about origin
            triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green);
            triangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue);

            //vertex buffer
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);

            model = Content.Load<Model>("Car");
            level = new Level(model, Camera.Instance.Target);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            Camera.Instance.Update(GraphicsDevice);
            Vector3 direction = Camera.Instance.Target - Camera.Instance.Position;
            level.Update(Camera.Instance.Target + Vector3.Multiply(direction, 20));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            basicEffect.Projection = Camera.Instance.Projection;
            basicEffect.View = Camera.Instance.View;
            basicEffect.World = Matrix.Identity;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.SetVertexBuffer(terrainBuffer);
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, terrainVertices.Length/3);

                basicEffect.World = worldMatrix;
                pass.Apply();
                GraphicsDevice.SetVertexBuffer(vertexBuffer);
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }
            level.Draw(Camera.Instance.View, Camera.Instance.Projection);
            base.Draw(gameTime);
        }
        private void CreateTerrain()
        {
            //6 vertices per squad: twee driehoeken voor vierhoek -> twee keer 3 punten
            terrainVertices = new VertexPositionColor[terrainWidth * terrainHeight * 6];
            int i = 0;
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    Vector3 topLeft = new Vector3(x, 0, y);
                    Vector3 topRight= new Vector3(x+1, 0, y);
                    Vector3 bottomLeft = new Vector3(x, 0, y + 1);
                    Vector3 bottomRight = new Vector3(x+1, 0, y + 1);

                    //driehoek 1
                    terrainVertices[i++] = new VertexPositionColor(topLeft, Color.DarkGreen);
                    terrainVertices[i++] = new VertexPositionColor(bottomRight, Color.Green);
                    terrainVertices[i++] = new VertexPositionColor(bottomLeft, Color.DarkGreen);
                    //driehoek 2
                    terrainVertices[i++] = new VertexPositionColor(topLeft, Color.DarkGreen);
                    terrainVertices[i++] = new VertexPositionColor(topRight, Color.Green);
                    terrainVertices[i++] = new VertexPositionColor(bottomRight, Color.Green);
                }
            }
            terrainBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), terrainVertices.Length, BufferUsage.WriteOnly);
            terrainBuffer.SetData(terrainVertices);
        }
    }
}
