using System;
using UnityEngine;

namespace Player
{
	public class TrickJumpParameter : StateEnteringParameter
	{
		public bool m_succeed = true;

		public Vector3 m_position = Vector3.zero;

		public Quaternion m_rotation = Quaternion.identity;

		public float m_outOfControlTime;

		public float m_firstSpeed;

		public Quaternion m_succeedRotation = Quaternion.identity;

		public float m_succeedOutOfcontrol;

		public float m_succeedFirstSpeed;

		public override void Reset()
		{
			this.m_succeed = false;
			this.m_outOfControlTime = 0f;
			this.m_firstSpeed = 0f;
		}

		public void Set(Vector3 pos, Quaternion rot, float speed, float time, Quaternion succeedRot, float succeedSpeed, float succeedTime, bool succeed)
		{
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_outOfControlTime = time;
			this.m_firstSpeed = speed;
			this.m_succeedRotation = succeedRot;
			this.m_succeedOutOfcontrol = succeedTime;
			this.m_succeedFirstSpeed = succeedSpeed;
			this.m_succeed = succeed;
		}
	}
}
