using System;

namespace Message
{
	public class MsgHudBossHpGaugeSet : MessageBase
	{
		public int m_hp;

		public int m_hpMax;

		public MsgHudBossHpGaugeSet(int hp, int hpMax) : base(49153)
		{
			this.m_hp = hp;
			this.m_hpMax = hpMax;
		}
	}
}
