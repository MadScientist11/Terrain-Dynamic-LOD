using System;
using UnityEngine;

namespace DynamicLOD
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class TerrainComponent : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform[] _effectors;
        [SerializeField] private MeshWireframeComputor _wireframeComputor;
        [SerializeField] private int _lodLevel;
        [SerializeField] private int _size;

        private void Start()
        {
            
        }

        private void Update()
        {
            Terrain terrain = new Terrain(transform.position,_size, _lodLevel);
            
            foreach (Transform effector in _effectors)
            {
                terrain.SetTerrainEffector(new Vector2(effector.position.x, effector.position.z));
            }
           
            TerrainMeshGenerator meshGenerator = new TerrainMeshGenerator(terrain);
            _meshFilter.mesh = meshGenerator.BuildMesh();
            _wireframeComputor.UpdateMesh();

        }
    }
}