using _3dcargame.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dcargame.GameObjects
{
    internal class Player : GameObject
    {
        public static Player Instance { get => instance; }
        private static Player instance;
        private Player(Model model, Vector3 position) : base(model, position)
        {

        }
        public static void Init(Model model)
        {
            if(instance==null)
            {
                instance = new Player(model, Vector3.Zero);
            }
        }
        public override void Update(Vector3 direction)
        {
            Vector3 forward = new Vector3((float)Math.Sin(Rotation.Y), 0, (float)Math.Cos(Rotation.Y));
            float directionSign = Vector3.Dot(forward, Speed) >= 0 ? 1f : -1f;
            
            Vector3 lookDirection = new Vector3((float)Math.Sin(Rotation.Y), 0f, (float)Math.Cos(Rotation.Y));

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Speed -= Vector3.Multiply(lookDirection, 0.05f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                Speed += Vector3.Multiply(lookDirection, 0.05f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                Rotation += Vector3.Multiply(Vector3.Up, directionSign * 0.05f * Speed.Length());
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Rotation += Vector3.Multiply(Vector3.Down, directionSign * 0.05f * Speed.Length());
            }
            Position += Speed;
            if(Speed!=Vector3.Zero)
            {
                Speed = new Vector3(MathHelper.Lerp(Speed.X, 0, 0.05f), 0, MathHelper.Lerp(Speed.Z, 0, 0.05f));
            }
        }
        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            base.Draw(viewMatrix, projectionMatrix);
        }
    }
}
