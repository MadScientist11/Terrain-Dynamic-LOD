using System.Collections.Generic;
using UnityEngine;

namespace DynamicLOD
{
    public class TerrainMeshGenerator
    {
        private readonly Terrain _terrain;
        private readonly Mesh _mesh;
        private readonly Dictionary<Vector2, int> _vertexIndex;
        private readonly List<int> _triangles;
        private readonly List<Vector3> _vertices;

        private readonly Vector2[] _vertexOrder =
        {
            new(1, 1), // bottom left
            new(1, -1), // top left
            new(-1, -1), // top right
            new(-1, 1) // bottom right
        };

        private readonly int[] _trianglesLookUp =
        {
            0, 1, 2,
            2, 3, 0
        };

        public TerrainMeshGenerator(Terrain terrain)
        {
            _terrain = terrain;
            _mesh = new Mesh();
            _vertexIndex = new Dictionary<Vector2, int>();
            _triangles = new List<int>();
            _vertices = new List<Vector3>();
        }

        public Mesh BuildMesh()
        {
            HashSet<QuadTree.QuadTreeNode> terrainChunks = _terrain.GetTerrainChunks();

            foreach (QuadTree.QuadTreeNode chunk in terrainChunks)
            {
                AddChunkData(chunk);
            }
            
            _mesh.SetVertices(_vertices);
            _mesh.SetTriangles(_triangles, 0);
            _mesh.RecalculateNormals();
            return _mesh;
        }

        private void AddChunkData(QuadTree.QuadTreeNode chunk)
        {
            Vector2[] vertices = new Vector2[4];
            for (var i = 0; i < 4; i++)
            {
                vertices[i] = chunk.Position - new Vector2(chunk.Size, chunk.Size) * _vertexOrder[i];
                if (!_vertexIndex.ContainsKey(vertices[i]))
                {
                    _vertices.Add(vertices[i]);
                    _vertexIndex[vertices[i]] = _vertices.Count - 1;
                }
            }

            for (int t = 0; t < 6; t++)
            {
                _triangles.Add(_vertexIndex[vertices[_trianglesLookUp[t]]]);
            }
        }
    }
}