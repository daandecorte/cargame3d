using _3dcargame.GameObjects;
using _3dcargame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dcargame.Levels
{
    internal class Level
    {
        public List<IGameObject> GameObjects { get; set; }

        public Level(Model model, Vector3 position)
        {
            Init(model, position);
        }
        public void Init(Model playerModel, Vector3 position)
        {
            Player.Init(playerModel);
            GameObjects = new List<IGameObject>();
            GameObjects.Add(Player.Instance);
        }
        public void Update(Vector3 direction)
        {
            foreach (var gameObject in GameObjects)
            {
                gameObject.Update(direction);
            }
        }
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (var gameObject in GameObjects)
            {
                gameObject.Draw(viewMatrix, projectionMatrix);
            }
        }
    }
}
