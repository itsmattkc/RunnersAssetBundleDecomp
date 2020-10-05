using System;
using UnityEngine;

namespace Player
{
	public struct HitInfo
	{
		public bool valid;

		public RaycastHit info;

		public void Reset()
		{
			this.valid = false;
		}

		public void Set(RaycastHit hit)
		{
			this.valid = true;
			this.info = hit;
		}

		public bool IsValid()
		{
			return this.valid;
		}
	}
}
