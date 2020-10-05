using System;
using UnityEngine;

namespace Message
{
	public class MsgHitDamageSucceed : MessageBase
	{
		public readonly GameObject m_sender;

		public readonly int m_score;

		public readonly Vector3 m_position;

		public readonly Quaternion m_rotation;

		public MsgHitDamageSucceed(GameObject sender, int score) : base(16385)
		{
			this.m_sender = sender;
			this.m_score = score;
		}

		public MsgHitDamageSucceed(GameObject sender, int score, Vector3 position, Quaternion rotation) : base(16385)
		{
			this.m_sender = sender;
			this.m_score = score;
			this.m_position = position;
			this.m_rotation = rotation;
		}
	}
}
