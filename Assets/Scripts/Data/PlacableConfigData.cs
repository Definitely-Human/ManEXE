using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

    [System.Serializable]
    public struct PlacableConfigData 
    {
        public PlacementSettings PlacementSettings;
        [SerializeField]
        private int _density;
        [SerializeField]
        private int _minHeight;
        [SerializeField]
        private int _maxHeight;

        public int MaxHeight { get => _maxHeight;
            set
            {
                if (value >= 0 && value >= MinHeight)
                    _maxHeight = value;
            }
        }
        public int MinHeight { get => _minHeight;
            set
            {
                if (value >= 0 && value <= MaxHeight)
                    _minHeight = value;
            }
        }

        public int Density { get => _density;
            set
            {
                if (value >= 0 )
                    _minHeight = value;
            }
        }
    }
}
