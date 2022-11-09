using ManExe.Data;
using UnityEditor;
using UnityEngine;

namespace ManExe.World
{
    public class PlacementGenerator : MonoBehaviour
    {
        [SerializeField] private World _world;


        [Header("Raycast Settings")]
        [SerializeField] private float _density;

        [SerializeField] private Vector2 _xRange;
        [SerializeField] private Vector2 _zRange;


        public PlacableConfigData[] Placements { get => _world.Settings.PlacableConfigData; }
        public World World { get => _world; set => _world = value; }
        public float Density { get => _density; set => _density = value; }
        public Vector2 XRange { get => _xRange; set => _xRange = value; }
        public Vector2 ZRange { get => _zRange; set => _zRange = value; }

        private void Awake()
        {
            XRange = new Vector2(0, World.WorldSizeInVoxelsX);
            ZRange = new Vector2(0, World.WorldSizeInVoxelsZ);
        }

        public void Generate()
        {
            Clear();
            
            for (int n = 0; n < Placements.Length; n++)
            {
                int densityCalculated = Mathf.FloorToInt(Density * Placements[n].Density * World.Settings.WorldSizeInChunksX * World.Settings.WorldSizeInChunksZ);
                for (int i = 0; i < densityCalculated; i++)
                {
                    float sampleX = Random.Range(XRange.x, XRange.y);
                    float sampleY = Random.Range(ZRange.x, ZRange.y);
                    Vector3 rayStart = new Vector3(sampleX, Placements[n].MaxHeight, sampleY);

                    if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                        continue;

                    if (hit.point.y < Placements[n].MinHeight)
                        continue;
                    var placementSettings =
                        _world.Settings.GetPlacableSettingsFromDatabase(Placements[n].placementSettingsId);
                    GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(placementSettings.Prefab, World.Placements.transform);
                    instantiatedPrefab.transform.position = hit.point;
                    instantiatedPrefab.transform.Rotate(Vector3.up, Random.Range(placementSettings.RotationRange.x, placementSettings.RotationRange.y), Space.Self);
                    instantiatedPrefab.transform.rotation = Quaternion.Lerp(transform.rotation,
                        transform.rotation * Quaternion.FromToRotation(instantiatedPrefab.transform.up, hit.normal), placementSettings.RotateTwardsNormal);
                    instantiatedPrefab.transform.localScale = new Vector3(
                        Random.Range(placementSettings.MinScale.x, placementSettings.MaxScale.x),
                        Random.Range(placementSettings.MinScale.y, placementSettings.MaxScale.y),
                        Random.Range(placementSettings.MinScale.z, placementSettings.MaxScale.z)
                    );
                }
            }
        }

        public void Clear()
        {
            while (World.Placements.transform.childCount != 0)
            {
                DestroyImmediate(World.Placements.transform.GetChild(0).gameObject);
            }
        }

    }
}
