using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class UpdatableData : ScriptableObject
    {
		public event System.Action OnValuesUpdated;
		public bool autoUpdate;
#if UNITY_EDITOR
		protected virtual void OnValidate()
		{
			if (autoUpdate)
			{
				NotifyOfUpdatedValues();
			}
		}

		public void NotifyOfUpdatedValues()
		{
			if (OnValuesUpdated != null)
			{
				OnValuesUpdated();
			}
		}
#endif

	}
}
