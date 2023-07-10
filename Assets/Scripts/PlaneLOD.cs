using UnityEngine;

namespace DynamicLOD
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PlaneLOD : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _effector;
        [SerializeField] private int _lodLevel;

        private void Start()
        {
            Terrain terrain = new Terrain(1, _lodLevel);
            terrain.SetTerrainEffector(_effector.position);
            TerrainMeshGenerator meshGenerator = new TerrainMeshGenerator(terrain);
            _meshFilter.mesh = meshGenerator.BuildMesh();
        }
    }
}