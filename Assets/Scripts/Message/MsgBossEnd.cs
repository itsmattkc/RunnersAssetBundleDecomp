using System;

namespace Message
{
	public class MsgBossEnd : MessageBase
	{
		public bool m_dead;

		public MsgBossEnd(bool dead) : base(12307)
		{
			this.m_dead = dead;
		}
	}
}
