using UnityEngine;

namespace ManExe.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New TerrainType", menuName = "Scriptable/TerrainType", order = 0)]
    public class TerrainType : ScriptableObject
    {
        // ===== Data =====
        [SerializeField]
        private int _terreinTypeID;
        [SerializeField]
        private string _name;

        // ===== Texture =====
        [SerializeField]
        private int _topTexID;
        [SerializeField]
        private int _sideTexID;

        public int TerreinTypeID { get => _terreinTypeID; set => _terreinTypeID = value; }
        public string Name { get => _name; set => _name = value; }
        public int TopTexID { get => _topTexID; set => _topTexID = value; }
        public int SideTexID { get => _sideTexID; set => _sideTexID = value; }
    }
}
