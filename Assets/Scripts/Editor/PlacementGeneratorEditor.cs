using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManExe
{
    [CustomEditor(typeof(PlacementGenerator))]
    public class PlacementGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PlacementGenerator placementGenerator = (PlacementGenerator)target;
            if (GUILayout.Button("Generate"))
            {
                placementGenerator.Generate();
            }
            if (GUILayout.Button("Clear"))
            {
                placementGenerator.Clear();
            }
        }
    }
}
