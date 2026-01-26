using _3dcargame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

        public GameObject(Model model, Vector3 position)
        {
            
            this.Model = model;
            this.Position = position;
            this.Rotation = Vector3.Zero;
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Position = new Vector3(Position.X, 0, Position.Z);
            Matrix worldMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(-90)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(180)) *
                Matrix.CreateRotationZ(Rotation.Z) *
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

        public void Update(Vector3 direction)
        {
            Position = direction;
        }
    }
}
