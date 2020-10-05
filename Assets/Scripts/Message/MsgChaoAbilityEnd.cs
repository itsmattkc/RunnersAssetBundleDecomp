using System;

namespace Message
{
	public class MsgChaoAbilityEnd : MessageBase
	{
		public readonly ChaoAbility[] m_ability;

		public MsgChaoAbilityEnd(ChaoAbility[] ability) : base(21762)
		{
			this.m_ability = ability;
		}

		public MsgChaoAbilityEnd(ChaoAbility ability) : base(21762)
		{
			this.m_ability = new ChaoAbility[1];
			this.m_ability[0] = ability;
		}
	}
}
