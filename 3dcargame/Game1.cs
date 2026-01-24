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
        //keyboard
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        //mouse
        float yaw = -MathHelper.PiOver2;
        float pitch = 0;
        MouseState prevMouseState;

        //camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        //basic rendering
        BasicEffect basicEffect;

        //geometrische   info
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

        //orbit
        bool orbit = false;

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
            //level = new Level();
            
            //terrain
            CreateTerrain();
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            prevMouseState = Mouse.GetState();

            //setup camera
            camPosition = new Vector3(50f, 10f, 60f);
            camTarget = new Vector3(50f, 10f, 40f);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView
            (
                MathHelper.ToRadians(80f), 
                GraphicsDevice.DisplayMode.AspectRatio, 
                1f, 
                1000f
            );
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }
        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Vector3 newPos = camPosition;
            Vector3 lookDirection = new Vector3((float)Math.Cos(yaw), 0f, (float)Math.Sin(yaw));
            if(Keyboard.GetState().IsKeyDown(Keys.S))
            {
                newPos -= Vector3.Multiply(lookDirection, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                newPos += Vector3.Multiply(lookDirection, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
            {
                newPos.Y -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Add))
            {
                newPos.Y += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                newPos += new Vector3(lookDirection.Z, 0f, -lookDirection.X);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                newPos += new Vector3(-lookDirection.Z, 0f, lookDirection.X);
            }
            camPosition = newPos;

            currentKeyboardState = Keyboard.GetState();
            if(currentKeyboardState.IsKeyUp(Keys.Space) && previousKeyboardState.IsKeyDown(Keys.Space))
            {
                while (Keyboard.GetState().IsKeyDown(Keys.Space)) {}
                orbit = !orbit;
            }
            previousKeyboardState = Keyboard.GetState();

            if(orbit)
            { 
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }

            //mouse movement
            MouseState currentMouseState = Mouse.GetState();

            float mouseSpeed = 0.005f;
            float xDiff = currentMouseState.X - prevMouseState.X;
            float yDiff = currentMouseState.Y - prevMouseState.Y;

            yaw += xDiff * mouseSpeed;
            pitch -= yDiff * mouseSpeed;

            pitch = MathHelper.Clamp(pitch, -MathHelper.PiOver2 + 0.01f, MathHelper.PiOver2 - 0.01f);
            Vector3 direction = new Vector3(
            (float)(Math.Cos(yaw) * Math.Cos(pitch)),
            (float)(Math.Sin(pitch)),
            (float)(Math.Sin(yaw) * Math.Cos(pitch)));

            camTarget = camPosition + direction;
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            prevMouseState = Mouse.GetState();
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            //level.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = Matrix.Identity;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;

                }
                mesh.Draw();
            }
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
            //level.Draw();
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
