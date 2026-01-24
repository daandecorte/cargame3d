using _3dcargame.GameObjects;
using _3dcargame.Interfaces;
using Microsoft.Xna.Framework;
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

        public Level()
        {
            Init();
        }
        public void Init()
        {
            //GameObjects.Add(new Player());
            GameObjects.Add(new GameObject());
        }
        public void Update()
        {
            foreach (var gameObject in GameObjects)
            {
                gameObject.Update();
            }
        }
        public void Draw()
        {
            foreach (var gameObject in GameObjects)
            {
                gameObject.Draw();
            }
        }
    }
}
