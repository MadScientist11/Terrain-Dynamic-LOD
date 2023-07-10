using System.Collections.Generic;
using UnityEngine;

namespace DynamicLOD
{
    public class Terrain
    {
        private readonly float _size;
        private readonly QuadTree _terrainQuadTree;

        public Terrain(float size, int initialDepth = 0)
        {
            _size = size;
            _terrainQuadTree = new QuadTree(Vector2.one, size, initialDepth);
        }

        public void SetTerrainEffector(Vector2 position)
        {
            _terrainQuadTree.Insert(position);
        }

        public HashSet<QuadTree.QuadTreeNode> GetTerrainChunks()
        {
            return _terrainQuadTree.GetLeafs();
        }
    }
}