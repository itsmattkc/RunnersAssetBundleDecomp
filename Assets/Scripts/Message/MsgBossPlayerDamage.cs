using System;

namespace Message
{
	public class MsgBossPlayerDamage : MessageBase
	{
		public bool m_dead;

		public MsgBossPlayerDamage(bool dead) : base(12326)
		{
			this.m_dead = dead;
		}
	}
}
