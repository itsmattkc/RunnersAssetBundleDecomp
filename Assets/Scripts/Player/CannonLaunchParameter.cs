using System;
using UnityEngine;

namespace Player
{
	public class CannonLaunchParameter : StateEnteringParameter
	{
		public Vector3 m_position = Vector3.zero;

		public Quaternion m_rotation = Quaternion.identity;

		public float m_firstSpeed;

		public float m_height;

		public float m_outOfControlTime;

		public override void Reset()
		{
		}

		public void Set(Vector3 pos, Quaternion rot, float firstSpeed, float height, float outOfcontrol)
		{
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_firstSpeed = firstSpeed;
			this.m_height = height;
			this.m_outOfControlTime = outOfcontrol;
		}
	}
}
