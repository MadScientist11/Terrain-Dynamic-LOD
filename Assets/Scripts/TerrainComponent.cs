using System;
using UnityEngine;

namespace DynamicLOD
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class TerrainComponent : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _effector;
        [SerializeField] private int _lodLevel;
        [SerializeField] private int _size;

        private void Start()
        {
            
        }

        private void Update()
        {
            Terrain terrain = new Terrain(transform.position,_size, _lodLevel);
            terrain.SetTerrainEffector(new Vector2(_effector.position.x, _effector.position.z));
            TerrainMeshGenerator meshGenerator = new TerrainMeshGenerator(terrain);
            _meshFilter.mesh = meshGenerator.BuildMesh();

        }
    }
}