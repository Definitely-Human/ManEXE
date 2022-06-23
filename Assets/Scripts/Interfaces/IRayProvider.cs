using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

	public interface IRayProvider
	{
		public Ray CreateRay();
	}
}
