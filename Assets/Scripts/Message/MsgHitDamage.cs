using System;
using UnityEngine;

namespace Message
{
	public class MsgHitDamage : MessageBase
	{
		public readonly GameObject m_sender;

		public int m_attackPower;

		public uint m_attackAttribute;

		public MsgHitDamage(GameObject sender, AttackPower attack) : base(16384)
		{
			this.m_sender = sender;
			this.m_attackPower = (int)attack;
		}
	}
}
