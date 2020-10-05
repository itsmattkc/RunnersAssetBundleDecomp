using System;
using UnityEngine;

namespace Message
{
	public class MsgOnCannonImpulse : MessageBase
	{
		public Vector3 m_position;

		public Quaternion m_rotation;

		public float m_firstSpeed;

		public float m_outOfControl;

		public bool m_roulette;

		public bool m_succeed;

		public MsgOnCannonImpulse(Vector3 pos, Quaternion rot, float firstSpeed, float outOfControl, bool roulette) : base(24578)
		{
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_firstSpeed = firstSpeed;
			this.m_outOfControl = outOfControl;
			this.m_roulette = roulette;
			this.m_succeed = false;
		}
	}
}
