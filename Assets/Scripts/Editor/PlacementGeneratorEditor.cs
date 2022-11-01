using ManExe.World;
using UnityEditor;
using UnityEngine;

namespace ManExe.Editor
{
    [CustomEditor(typeof(PlacementGenerator))]
    public class PlacementGeneratorEditor : UnityEditor.Editor
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
