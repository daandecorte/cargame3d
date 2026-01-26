using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dcargame.Interfaces
{
    internal interface IGameObject
    {
        Model Model { get; set; }
        Vector3 Position { get; set; }
        Vector3 Speed { get; set; }
        Vector3 Rotation { get; set; }
        void Update(Vector3 direction);
        void Draw(Matrix viewMatrix, Matrix projectionMatrix);
    }
}
