using System;
using System.Collections;
using System.Collections.Generic;
using ManExe.Core;
using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe
{
    public class DebugOutliners : MonoBehaviour
    {
        [SerializeField] private GameObject chunkOutliner;
        [SerializeField] private GameObject blockOutliner;
        [SerializeField] private GameObject outlinersContainer;
        [SerializeField] private LayerMask canOutlineLayerMask;
        private InputReader _inputReader;
        private Transform _player;
        private Camera _camera;
        private bool _isActive;
        
        public bool IsActive {
            get => _isActive;
            private set
            {
                _isActive = value;
                if(outlinersContainer!= null)
                    outlinersContainer.gameObject.SetActive(value);
            }
        }
        
        private void Awake()
        {
            
            _inputReader = Resources.Load<InputReader>("Input/Default Input Reader");
        }
        void Start()
        {
            
            _player =  GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            IsActive = false;
            if(chunkOutliner!= null)
                chunkOutliner.GetComponent<MeshFilter>().mesh = CreateCube(GameData.ChunkWidth,GameData.ChunkHeight);
            
            _camera = Camera.main.transform.gameObject.GetComponent<Camera>();
        }
        
        private Mesh CreateCube (int cubeWidth, int cubeHeight) {
            Vector3[] vertices = {
                new Vector3 (0*cubeWidth, 0*cubeHeight, 0*cubeWidth),
                new Vector3 (1*cubeWidth, 0*cubeHeight, 0*cubeWidth),
                new Vector3 (1*cubeWidth, 1*cubeHeight, 0*cubeWidth),
                new Vector3 (0*cubeWidth, 1*cubeHeight, 0*cubeWidth),
                new Vector3 (0*cubeWidth, 1*cubeHeight, 1*cubeWidth),
                new Vector3 (1*cubeWidth, 1*cubeHeight, 1*cubeWidth),
                new Vector3 (1*cubeWidth, 0*cubeHeight, 1*cubeWidth),
                new Vector3 (0*cubeWidth, 0*cubeHeight, 1*cubeWidth),
            };

            int[] triangles = {
                0, 2, 1, //face front
                0, 3, 2,
                1, 2, 5, //face right
                1, 5, 6,
                0, 7, 4, //face left
                0, 4, 3,
                5, 4, 7, //face back
                5, 7, 6
            };

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.Optimize ();
            mesh.RecalculateNormals ();
            return mesh;
        }

        void Update()
        {
            if(!IsActive)return;
            UpdateOuliners();
        }

        private void OnEnable()
        {
            _inputReader.OutlinersEvent += ManageOutliners;
        }

        private void OnDisable()
        {
            _inputReader.OutlinersEvent -= ManageOutliners;
        }

        private void ManageOutliners()
        {
            if(chunkOutliner==null||blockOutliner==null||outlinersContainer==null) return;

            IsActive = !IsActive;
        }


        private void UpdateOuliners()
        {
            var pPos = _player.transform.position;
            // To determine the chunk chunk player is in throw away the % 16 part of the coord, than if player has negative coord
            // subtract 16 to shift coords correctly
            int x = ((int)pPos.x / GameData.ChunkWidth) * GameData.ChunkWidth - (pPos.x < 0 ? 16 : 0);
            int z = (((int)pPos.z / GameData.ChunkWidth) * GameData.ChunkWidth - (pPos.z < 0 ? 16 : 0));
            chunkOutliner.transform.position = new Vector3(x, 0, z);
            
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            if (Physics.Raycast(ray, out var hit, 255, canOutlineLayerMask))
                blockOutliner.transform.position = new Vector3(Mathf.Floor(hit.point.x),Mathf.Floor(hit.point.y),Mathf.Floor(hit.point.z));
        }
    }
}
