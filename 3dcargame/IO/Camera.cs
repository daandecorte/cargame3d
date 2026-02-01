using _3dcargame.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dcargame.IO
{
    internal class Camera
    {
        public Vector3 Target { get; set; }
        public Vector3 Position { get; set; } = new Vector3(50f, 5f, 60f);
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }

        public float Yaw { get; set; } = -MathHelper.PiOver2;
        public float Pitch { get; set; } = 0;

        private MouseState prevMouseState;

        private static Camera instance;
        public static Camera Instance 
        {
            get 
            { 
                if(instance == null)
                {
                    instance = new Camera();
                }
                return instance; 
            } 
        }
        private Camera() {}
        public void Init(GraphicsDevice GraphicsDevice)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView
            (
                MathHelper.ToRadians(80f),
                GraphicsDevice.DisplayMode.AspectRatio,
                1f,
                1000f
            );

            prevMouseState = Mouse.GetState();
        }
        public void Update(GraphicsDevice GraphicsDevice)
        {
            float distance = 15f;

            //mouse movement
            MouseState currentMouseState = Mouse.GetState();
            float mouseSpeed = 0.005f;
            
            float xDiff = currentMouseState.X - prevMouseState.X;
            float yDiff = currentMouseState.Y - prevMouseState.Y;
            Yaw += xDiff * mouseSpeed;
            Pitch -= yDiff * mouseSpeed;
            Pitch = MathHelper.Clamp(Pitch, -MathHelper.PiOver2 + 0.01f, MathHelper.PiOver2 - 0.01f);
            
            Vector3 direction = new Vector3
            (
                (float)(Math.Cos(Yaw) * Math.Cos(Pitch)),
                (float)(Math.Sin(Pitch)),
                (float)(Math.Sin(Yaw) * Math.Cos(Pitch))
            ) * distance;

            Target = Player.Instance.Position;

            Position = Target + direction;

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            prevMouseState = Mouse.GetState();
            View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }
    }
}
