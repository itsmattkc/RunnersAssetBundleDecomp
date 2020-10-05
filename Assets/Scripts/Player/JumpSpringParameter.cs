using System;
using UnityEngine;

namespace Player
{
	public class JumpSpringParameter : StateEnteringParameter
	{
		public Vector3 m_position = Vector3.zero;

		public Quaternion m_rotation = Quaternion.identity;

		public float m_outOfControlTime;

		public float m_firstSpeed;

		public override void Reset()
		{
			this.m_outOfControlTime = 0f;
			this.m_firstSpeed = 0f;
		}

		public void Set(Vector3 pos, Quaternion rot, float speed, float time)
		{
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_outOfControlTime = time;
			this.m_firstSpeed = speed;
		}
	}
}
