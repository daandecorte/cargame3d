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

        private Texture2D heightMap;
        private Color[] heightMapData;

        private float scale = 2f;
        private Terrain() {}

        public void Init(GraphicsDevice graphics, Texture2D heightMap)
        {
            this.heightMap = heightMap;
            this.heightMapData = new Color[heightMap.Width * heightMap.Height];
            this.heightMap.GetData(this.heightMapData);

            CreateTerrain(graphics);
        }
        public void Draw(GraphicsDevice GraphicsDevice, BasicEffect basicEffect, Matrix worldMatrix)
        {
            GraphicsDevice.SetVertexBuffer(terrainBuffer);
            basicEffect.World = worldMatrix;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, terrainVertices.Length / 3);

            }
        }
        public float GroundLevel(Vector3 position)
        {
            float gridX = position.X / scale;
            float gridZ = position.Z / scale;
            int x1 = (int)MathHelper.Clamp(gridX , 0, heightMap.Width - 2) ;
            int y1 = (int)MathHelper.Clamp(gridZ , 0, heightMap.Height -2);
            int x2 = x1 + 1;
            int y2 = y1 + 1;

            float h1 = heightMapData[x1 + y1 * heightMap.Width].R / 10f;
            float h2 = heightMapData[x2 + y1 * heightMap.Width].R / 10f;
            float h3 = heightMapData[x1 + y2 * heightMap.Width].R / 10f;
            float h4 = heightMapData[x2 + y2 * heightMap.Width].R / 10f;

            float xAmount = gridX - x1;
            float yAmount = gridZ - y1;

            return MathHelper.Lerp(MathHelper.Lerp(h1, h2, xAmount), MathHelper.Lerp(h3, h4, xAmount), yAmount);
        }
        private void CreateTerrain(GraphicsDevice GraphicsDevice)
        {
            int width = heightMap.Width;
            int height = heightMap.Height;
            terrainVertices = new VertexPositionColor[(width-1) * (height-1) * 6];

            int i = 0;
            for (int x = 0; x < width-1; x++)
            {
                for (int y = 0; y < height-1; y++)
                {
                    int heightTopLeft = heightMapData[x + y * width].R/10;
                    int heightTopRight = heightMapData[(x+1) + y * width].R/10;
                    int heightBottomLeft = heightMapData[x + (y+1) * width].R/10;
                    int heightBottomRight = heightMapData[(x + 1) + (y+1) * width].R/10;

                    Vector3 topLeft     = new Vector3(x*scale, heightTopLeft, y * scale);
                    Vector3 topRight    = new Vector3((x + 1) * scale, heightTopRight, y * scale);
                    Vector3 bottomLeft  = new Vector3(x * scale, heightBottomLeft, (y + 1) * scale);
                    Vector3 bottomRight = new Vector3((x + 1) * scale, heightBottomRight, (y + 1) * scale);

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
