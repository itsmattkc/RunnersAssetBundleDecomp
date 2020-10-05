using System;

namespace Message
{
	public class MsgBossBoostLevel : MessageBase
	{
		public WispBoostLevel m_wispBoostLevel;

		public string m_wispBoostEffect;

		public MsgBossBoostLevel(WispBoostLevel wispBoostLevel, string effect) : base(12325)
		{
			this.m_wispBoostLevel = wispBoostLevel;
			this.m_wispBoostEffect = effect;
		}
	}
}
