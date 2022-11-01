namespace ManExe.World
{

    public class TerrainPoint 
    {
        private float _distToSurface = 1f;
        private int _terrainTypeID = 0;
    	

        public float DistToSurface { get => _distToSurface; set => _distToSurface = value; }
        public int TerrainTypeID { get => _terrainTypeID; set => _terrainTypeID = value; }

        public TerrainPoint(float dst, int tex)
        {
            _distToSurface = dst;
            _terrainTypeID = tex;
        }
    }
}
