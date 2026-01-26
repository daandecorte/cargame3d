using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dcargame.GameObjects
{
    internal class Terrain
    {
        public static Terrain Instance => instance ??= new Terrain();
        private static Terrain instance;

        VertexPositionColor[] terrainVertices;
        VertexBuffer terrainBuffer;
        int terrainWidth = 100;
        int terrainHeight = 100;
        private Terrain() {}

        public void Init(GraphicsDevice graphics)
        {
            CreateTerrain(graphics);
        }
        public void Draw(GraphicsDevice GraphicsDevice, BasicEffect basicEffect, Matrix worldMatrix)
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.SetVertexBuffer(terrainBuffer);
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, terrainVertices.Length / 3);

                basicEffect.World = worldMatrix;
            }
        }
        public bool Collide(GameObject gameObject)
        {
            if (gameObject.Position.Y <= 0) return true;
            return false;
        }

        private void CreateTerrain(GraphicsDevice GraphicsDevice)
        {
            //6 vertices per squad: twee driehoeken voor vierhoek -> twee keer 3 punten
            terrainVertices = new VertexPositionColor[terrainWidth * terrainHeight * 6];
            int i = 0;
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    Vector3 topLeft = new Vector3(x, 0, y);
                    Vector3 topRight = new Vector3(x + 1, 0, y);
                    Vector3 bottomLeft = new Vector3(x, 0, y + 1);
                    Vector3 bottomRight = new Vector3(x + 1, 0, y + 1);

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
