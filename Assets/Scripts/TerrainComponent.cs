using System;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace DynamicLOD
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
    public class TerrainComponent : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform[] _effectors;
        [SerializeField] private MeshWireframeComputor _wireframeComputor;
        [SerializeField] private int _lodLevel;
        [SerializeField] private int _size;

        [SerializeField] private TextMeshProUGUI _mousePosText;
        [SerializeField] private TextMeshProUGUI _instertionsText;
        [SerializeField] private TextMeshProUGUI _depthText;
        private Terrain _terrain;

        private int _effectorCount;

        private void Start()
        {
            _terrain = new Terrain(transform.position, _size, _lodLevel);
            GetComponent<BoxCollider>().size = new Vector3(_size, 0, _size);
            _depthText.text = $"Depth: {_lodLevel}";
            _mousePosText.text = $"Mouse Pos: X: 0 Y: 0";
            _instertionsText.text = $"Insertions: 0";
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var screenPointToRay = MousePointer.GetWorldRay(Camera.main);
                float3 position = float3.zero;
                if (Physics.Raycast(screenPointToRay, out RaycastHit hit))
                {
                    position = hit.point;
                    _mousePosText.text = $"Mouse Pos: X:{position.x:0.##} Y: {position.z::0.##}";

                    _terrain.SetTerrainEffector(new Vector2(position.x, position.z), 10);
                    _instertionsText.text = $"Insertions: {++_effectorCount}";
                }
            }

            TerrainMeshGenerator meshGenerator = new TerrainMeshGenerator(_terrain);
            _meshFilter.mesh = meshGenerator.BuildMesh();
            _wireframeComputor.UpdateMesh();
        }

        private Vector3 GetMousePositionWorldSpace() =>
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}