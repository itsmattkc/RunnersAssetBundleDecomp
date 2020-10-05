using System;
using UnityEngine;

namespace Message
{
	public class MsgOnDashRingImpulse : MessageBase
	{
		public Vector3 m_position;

		public Quaternion m_rotation;

		public float m_firstSpeed;

		public float m_outOfControl;

		public bool m_succeed;

		public MsgOnDashRingImpulse(Vector3 pos, Quaternion rot, float firstSpeed, float outOfControl) : base(24577)
		{
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_firstSpeed = firstSpeed;
			this.m_outOfControl = outOfControl;
			this.m_succeed = false;
		}
	}
}
