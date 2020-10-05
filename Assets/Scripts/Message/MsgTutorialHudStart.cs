using System;

namespace Message
{
	public class MsgTutorialHudStart : MessageBase
	{
		public HudTutorial.Id m_id;

		public BossType m_bossType;

		public MsgTutorialHudStart(HudTutorial.Id id, BossType bossType) : base(12348)
		{
			this.m_id = id;
			this.m_bossType = bossType;
		}
	}
}
