using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class GrassGenerator : MonoBehaviour
    {
       

        [Tooltip("The grass geometry creating compute shader")]
        [SerializeField] private ComputeShader grassComputeShader = default;
        [Tooltip("The material to render the grass mesh")]
        [SerializeField] private Material material = default;

        [SerializeField] private ProceduralGrassRenderer.GrassSettings grassSettings = default;

        public void GenerateGrass(GameObject chunk)
        {
            Mesh mesh = chunk.GetComponent<MeshFilter>().mesh;

            ProceduralGrassRenderer proceduralGrassRenderer = chunk.AddComponent<ProceduralGrassRenderer>();
            proceduralGrassRenderer.SetupVars(mesh, grassComputeShader, material, grassSettings);
        }
    }
}
