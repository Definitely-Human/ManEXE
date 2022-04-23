using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ManExe
{
	[CustomEditor(typeof(MapDisplay))]
	public class MapGeneratorEditor : Editor
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