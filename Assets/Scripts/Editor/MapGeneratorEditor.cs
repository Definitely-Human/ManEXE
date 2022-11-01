using ManExe.Noise;
using UnityEditor;
using UnityEngine;

namespace ManExe.Editor
{
	[CustomEditor(typeof(MapDisplay))]
	public class MapGeneratorEditor : UnityEditor.Editor
	{

		public override void OnInspectorGUI()
		{
			MapDisplay mapGen = (MapDisplay)target;

			
			if (DrawDefaultInspector())
			{
				if (mapGen.AutoUpdate)
				{
					mapGen.DrawNoiseMap();
				}
			}

			if (GUILayout.Button("Generate"))
			{
				mapGen.DrawNoiseMap();
			}
			if (GUILayout.Button("Create Renderer"))
			{
				mapGen.GenerateRenderer();
			}
			if (GUILayout.Button("Clear"))
			{
				mapGen.Clear();
			}
		}
	}
}