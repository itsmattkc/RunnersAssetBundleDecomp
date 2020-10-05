using System;
using UnityEngine;

namespace Message
{
	public class MsgWarpPlayer : MessageBase
	{
		public Vector3 m_position;

		public Quaternion m_rotation;

		public bool m_hold;

		public MsgWarpPlayer(Vector3 pos, Quaternion rot, bool hold) : base(20485)
		{
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_hold = hold;
		}
	}
}
