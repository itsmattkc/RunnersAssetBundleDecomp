using System;
using UnityEngine;

namespace Message
{
	public class MsgTutorialResetForRetry : MessageBase
	{
		public Vector3 m_position;

		public Quaternion m_rotation;

		public int m_ring;

		public bool m_blink;

		public MsgTutorialResetForRetry(Vector3 position, Quaternion rotation, bool blink) : base(12347)
		{
			this.m_position = position;
			this.m_rotation = rotation;
			this.m_blink = blink;
		}

		public MsgTutorialResetForRetry(int ring, bool blink) : base(12347)
		{
			this.m_ring = ring;
			this.m_blink = blink;
		}
	}
}
