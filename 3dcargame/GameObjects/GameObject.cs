using _3dcargame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dcargame.GameObjects
{
    internal class GameObject : IGameObject
    {
        public Model Model { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Speed { get; set; }
        public Vector3 Rotation { get; set; }
        private float fallSpeed = 0;
        public GameObject(Model model, Vector3 position)
        {
            
            this.Model = model;
            this.Position = position;
            this.Rotation = Vector3.Zero;
        }

        public virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.CreateTranslation(0, -1f, 0) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) *
            Matrix.CreateRotationY(Rotation.Y) *
            Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
            Matrix.CreateTranslation(Position);

            foreach (var mesh in Model.Meshes)
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
        }

        public virtual void Update()
        {
            float y = Terrain.Instance.GroundLevel(this.Position);
            if(Position.Y<=y)
            {
                this.Position = new Vector3(this.Position.X, y, this.Position.Z);
                if (fallSpeed > 0) fallSpeed -= 0.2f;
            }
            if(Position.Y>y)
            {
                fallSpeed += 0.02f;
                this.Position += Vector3.Multiply(Vector3.Down, fallSpeed);
            }
        }
    }
}
