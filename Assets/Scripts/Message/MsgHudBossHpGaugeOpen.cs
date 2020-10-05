using System;

namespace Message
{
	public class MsgHudBossHpGaugeOpen : MessageBase
	{
		public BossType m_bossType;

		public int m_level;

		public int m_hp;

		public int m_hpMax;

		public MsgHudBossHpGaugeOpen(BossType bossType, int level, int hp, int hpMax) : base(49152)
		{
			this.m_bossType = bossType;
			this.m_level = level;
			this.m_hp = hp;
			this.m_hpMax = hpMax;
		}
	}
}
