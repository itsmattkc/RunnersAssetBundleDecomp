using System;

namespace Message
{
	public class MsgChaoAbilityStart : MessageBase
	{
		public readonly ChaoAbility[] m_ability;

		public bool m_flag;

		public MsgChaoAbilityStart(ChaoAbility[] ability) : base(21761)
		{
			this.m_ability = ability;
			this.m_flag = false;
		}

		public MsgChaoAbilityStart(ChaoAbility ability) : base(21761)
		{
			this.m_ability = new ChaoAbility[1];
			this.m_ability[0] = ability;
			this.m_flag = false;
		}
	}
}
